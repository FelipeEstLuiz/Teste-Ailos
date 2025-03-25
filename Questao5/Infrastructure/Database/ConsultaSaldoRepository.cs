using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database
{
    public class ConsultaSaldoRepository : IConsultaSaldoRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public ConsultaSaldoRepository(DatabaseConfig databaseConfig) => _databaseConfig = databaseConfig;

        public async Task<ContaCorrenteResponse> GetContaCorrenteAsync(ContaCorrenteRequest request)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            return await connection.QueryFirstOrDefaultAsync<ContaCorrenteResponse>(@"
                SELECT idcontacorrente, numero, nome, ativo
                FROM contacorrente 
                WHERE numero = @Numero
            ", new
            {
                request.Numero
            });
        }

        public async Task<IEnumerable<MovimentoResponse>> GetMovimentacoesAsync(MovimentoRequest request)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
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
    }
}
