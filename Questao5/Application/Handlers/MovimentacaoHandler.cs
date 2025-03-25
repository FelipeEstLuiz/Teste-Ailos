using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoHandler : IRequestHandler<MovimentacaoCommand, Result<string>>
    {
        private readonly string[] TiposPermitidos = new string[]
        {
            "C", "D"
        };

        private readonly IContaBancariaRepository _contaBancariaRepository;

        public MovimentacaoHandler(IContaBancariaRepository contaBancariaRepository)
            => _contaBancariaRepository = contaBancariaRepository;

        public async Task<Result<string>> Handle(MovimentacaoCommand request, CancellationToken cancellationToken)
        {
            if (!TiposPermitidos.Contains(request.TipoMovimento))
                return Result<string>.Failure("Tipo de movimentacao invalido", TipoResponse.INVALID_TYPE);
            else if (request.Valor <= 0)
                return Result<string>.Failure("Valor deve ser maior que 0", TipoResponse.INVALID_VALUE);

            ContaCorrenteResponse conta = await _contaBancariaRepository
                .GetContaCorrenteAsync(new ContaCorrenteRequest(request.Numero));

            if (conta is null)
                return Result<string>.Failure("Conta nao cadastrada", TipoResponse.INVALID_ACCOUNT);
            else if (!conta.ContaAtiva)
                return Result<string>.Failure("Conta inativa", TipoResponse.INACTIVE_ACCOUNT);

            IdempotenciaResponse movimentacao = await _contaBancariaRepository
                .GetImpotenciaAsync(new IdempotenciaRequest(
                    request.IdentificacaoRequisiacao
                ));

            InserirMovimentacaoResponse idempotencia;

            if (movimentacao is null)
            {
                idempotencia = await _contaBancariaRepository
                    .InserirMovimentacaoAsync(new InserirMovimentacaoRequest(
                        identificacaoRequisiacao: request.IdentificacaoRequisiacao,
                        idcontacorrente: conta.Idcontacorrente,
                        tipoMovimento: request.TipoMovimento,
                        valor: request.Valor
                    ));
            }
            else
                idempotencia = JsonConvert.DeserializeObject<InserirMovimentacaoResponse>(movimentacao.resultado);

            return Result<string>.Success(idempotencia.IdMovimento);
        }
    }
}
