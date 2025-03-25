using Dapper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Database
{
    public class ContaBancariaRepository : IContaBancariaRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public ContaBancariaRepository(DatabaseConfig databaseConfig) => _databaseConfig = databaseConfig;

        public async Task<ContaCorrenteResponse> GetContaCorrenteAsync(ContaCorrenteRequest request)
        {
            using SqliteConnection connection = new(_databaseConfig.Name);
            return await connection.QueryFirstOrDefaultAsync<ContaCorrenteResponse>(@"
                SELECT idcontacorrente, numero, nome, ativo
                FROM contacorrente 
                WHERE numero = @Numero
            ", new
            {
                request.Numero
            });
        }

        public async Task<IdempotenciaResponse> GetImpotenciaAsync(IdempotenciaRequest request)
        {
            using SqliteConnection connection = new(_databaseConfig.Name);
            return await connection.QueryFirstOrDefaultAsync<IdempotenciaResponse>(@"
                SELECT chave_idempotencia, requisicao, resultado
                FROM idempotencia 
                WHERE chave_idempotencia = @chave_idempotencia
            ", new
            {
                chave_idempotencia = request.IdentificacaoRequisiacao
            });
        }

        public async Task<IEnumerable<MovimentoResponse>> GetMovimentacoesAsync(MovimentoRequest request)
        {
            using SqliteConnection connection = new(_databaseConfig.Name);
            return await connection.QueryAsync<MovimentoResponse>(@"
                SELECT idmovimento, idContaCorrente, datamovimento, tipomovimento, valor
                FROM movimento
                WHERE idContaCorrente = @IdContaCorrente
                AND tipomovimento = @TipoMovimento
            ", new
            {
                request.IdContaCorrente,
                request.TipoMovimento
            });
        }

        public async Task<InserirMovimentacaoResponse> InserirMovimentacaoAsync(InserirMovimentacaoRequest request)
        {
            using SqliteConnection connection = new(_databaseConfig.Name);

            if (connection.State == ConnectionState.Closed)
                await connection.OpenAsync();

            using IDbTransaction transaction = await connection.BeginTransactionAsync();
            try
            {
                string idMovimento = Guid.NewGuid().ToString();
                string dataMovimento = DateTime.UtcNow.ToString("dd/MM/yyyy");

                string query = @"
                    INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                    VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)
                ";

                InserirMovimentacaoResponse response = new ()
                {
                    IdMovimento = idMovimento,
                    IdentificacaoRequisiacao = request.IdentificacaoRequisiacao,
                    Idcontacorrente = request.Idcontacorrente,
                    TipoMovimento = request.TipoMovimento,
                    Valor = request.Valor
                };

                await connection.ExecuteAsync(query, new
                {
                    IdMovimento = idMovimento,
                    IdContaCorrente = request.Idcontacorrente,
                    DataMovimento = dataMovimento,
                    request.TipoMovimento,
                    request.Valor
                }, transaction: transaction);

                await connection.ExecuteAsync(@"
                    INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                    VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)
                ", new
                {
                    ChaveIdempotencia = request.IdentificacaoRequisiacao,
                    Requisicao = JsonConvert.SerializeObject(request),
                    Resultado = JsonConvert.SerializeObject(response)
                }, transaction: transaction);


                transaction.Commit();

                return response;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
