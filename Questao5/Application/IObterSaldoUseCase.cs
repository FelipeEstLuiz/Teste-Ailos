namespace Questao5.Application
{
    public interface IObterSaldoUseCase
    {
        Task<decimal> GetSaldoContaCorrenteAsync(string idContaCorrente, CancellationToken cancellationToken);
    }
}
