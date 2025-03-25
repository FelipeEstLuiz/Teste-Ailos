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
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public ConsultaSaldoHandler(IContaBancariaRepository contaBancariaRepository)
            => _contaBancariaRepository = contaBancariaRepository;

        public async Task<Result<ConsultaSaldoResponse>> Handle(
            ConsultaSaldoQuery request,
            CancellationToken cancellationToken
        )
        {
            ContaCorrenteResponse conta = await _contaBancariaRepository
                .GetContaCorrenteAsync(new ContaCorrenteRequest(request.Numero));

            if (conta is null)
                return Result<ConsultaSaldoResponse>.Failure("Conta nao cadastrada", TipoResponse.INVALID_ACCOUNT);
            else if (!conta.ContaAtiva)
                return Result<ConsultaSaldoResponse>.Failure("Conta inativa", TipoResponse.INACTIVE_ACCOUNT);

            ConsultaSaldoResponse consultaSaldoResponse = new(conta.Numero, conta.Nome);

            IEnumerable<MovimentoResponse> creditos = await _contaBancariaRepository
                .GetMovimentacoesAsync(new MovimentoRequest(conta.Idcontacorrente, "C"));

            IEnumerable<MovimentoResponse> debitos = await _contaBancariaRepository
                .GetMovimentacoesAsync(new MovimentoRequest(conta.Idcontacorrente, "D"));

            decimal somaCreditos = creditos?.Sum(v => v.Valor) ?? 0;
            decimal somaDebitos = debitos?.Sum(v => v.Valor) ?? 0;

            consultaSaldoResponse.AtualizarSaldo(somaCreditos - somaDebitos);

            return Result<ConsultaSaldoResponse>.Success(consultaSaldoResponse);
        }
    }
}
