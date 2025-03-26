using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Application.Handlers
{
    public class ConsultaSaldoHandler : IRequestHandler<ConsultaSaldoQuery, Result<ConsultaSaldoResponse>>
    {
        private readonly IObterSaldoUseCase _obterSaldoUseCase;
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public ConsultaSaldoHandler(
            IContaBancariaRepository contaBancariaRepository,
            IObterSaldoUseCase obterSaldoUseCase
        )
        {
            _obterSaldoUseCase = obterSaldoUseCase;
            _contaBancariaRepository = contaBancariaRepository;
        }

        public async Task<Result<ConsultaSaldoResponse>> Handle(
            ConsultaSaldoQuery request,
            CancellationToken cancellationToken
        )
        {
            ContaCorrenteResponse conta = await _contaBancariaRepository
                .GetContaCorrenteAsync(new ContaCorrenteRequest(request.Numero), cancellationToken: cancellationToken);

            if (conta is null)
                return Result<ConsultaSaldoResponse>.Failure("Conta não cadastrada", TipoResponse.INVALID_ACCOUNT);
            else if (!conta.ContaAtiva)
                return Result<ConsultaSaldoResponse>.Failure("Conta inativa", TipoResponse.INACTIVE_ACCOUNT);

            ConsultaSaldoResponse consultaSaldoResponse = new(conta.Numero, conta.Nome);

            decimal saldo = await _obterSaldoUseCase.GetSaldoContaCorrenteAsync(
                conta.Idcontacorrente,
                cancellationToken
            );

            consultaSaldoResponse.AtualizarSaldo(saldo);

            return Result<ConsultaSaldoResponse>.Success(consultaSaldoResponse);
        }
    }
}
