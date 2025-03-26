using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Application
{
    public class ObterSaldoUseCase : IObterSaldoUseCase
    {
        private readonly IContaBancariaRepository _contaBancariaRepository;

        public ObterSaldoUseCase(IContaBancariaRepository contaBancariaRepository)

            => _contaBancariaRepository = contaBancariaRepository;
        public async Task<decimal> GetSaldoContaCorrenteAsync(string idContaCorrente, CancellationToken cancellationToken)
        {
            IEnumerable<MovimentoResponse> creditos = await _contaBancariaRepository
               .GetMovimentacoesAsync(new MovimentoRequest(idContaCorrente, "C"), cancellationToken: cancellationToken);

            IEnumerable<MovimentoResponse> debitos = await _contaBancariaRepository
                .GetMovimentacoesAsync(new MovimentoRequest(idContaCorrente, "D"), cancellationToken: cancellationToken);

            decimal somaCreditos = creditos?.Sum(v => v.Valor) ?? 0;
            decimal somaDebitos = debitos?.Sum(v => v.Valor) ?? 0;

            return somaCreditos - somaDebitos;
        }
    }
}
