using NSubstitute;
using Questao5.Application;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Xunit;

namespace Questao5.Tests
{
    public class ObterSaldoUseCaseTests
    {
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly ObterSaldoUseCase _obterSaldoUseCase;

        public ObterSaldoUseCaseTests()
        {
            _contaBancariaRepository = Substitute.For<IContaBancariaRepository>();
            _obterSaldoUseCase = new ObterSaldoUseCase(_contaBancariaRepository);
        }

        [Fact]
        public async Task GetSaldoContaCorrenteAsync_Deve_Retornar_Zero_Se_Sem_Movimentacoes()
        {
            string idContaCorrente = "000";
            CancellationToken cancellationToken = CancellationToken.None;

            List<MovimentoResponse> creditos = new();
            List<MovimentoResponse> debitos = new();

            _contaBancariaRepository.GetMovimentacoesAsync(
                Arg.Is<MovimentoRequest>(r => r.IdContaCorrente == idContaCorrente && r.TipoMovimento == "C"),
                cancellationToken
            ).Returns(Task.FromResult<IEnumerable<MovimentoResponse>>(creditos));

            _contaBancariaRepository.GetMovimentacoesAsync(
                Arg.Is<MovimentoRequest>(r => r.IdContaCorrente == idContaCorrente && r.TipoMovimento == "D"),
                cancellationToken
            ).Returns(Task.FromResult<IEnumerable<MovimentoResponse>>(debitos));

            decimal saldo = await _obterSaldoUseCase.GetSaldoContaCorrenteAsync(idContaCorrente, cancellationToken);

            Assert.Equal(0, saldo);
        }

        [Fact]
        public async Task GetSaldoContaCorrenteAsync_Deve_Retornar_Saldo_Correto()
        {
            string idContaCorrente = "000";
            CancellationToken cancellationToken = CancellationToken.None;

            List<MovimentoResponse> creditos = new()
            {
                new() { Valor = 100m },
                new(){ Valor = 50m }
            };

            List<MovimentoResponse> debitos = new()
            {
                new() { Valor = 30m }
            };

            _contaBancariaRepository.GetMovimentacoesAsync(
                Arg.Is<MovimentoRequest>(r => r.IdContaCorrente == idContaCorrente && r.TipoMovimento == "C"),
                cancellationToken
            ).Returns(Task.FromResult<IEnumerable<MovimentoResponse>>(creditos));

            _contaBancariaRepository.GetMovimentacoesAsync(
                Arg.Is<MovimentoRequest>(r => r.IdContaCorrente == idContaCorrente && r.TipoMovimento == "D"),
                cancellationToken
            ).Returns(Task.FromResult<IEnumerable<MovimentoResponse>>(debitos));

            decimal saldo = await _obterSaldoUseCase.GetSaldoContaCorrenteAsync(idContaCorrente, cancellationToken);

            Assert.Equal(120m, saldo);
        }

    }
}
