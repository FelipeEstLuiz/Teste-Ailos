using NSubstitute;
using Questao5.Application;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Xunit;

namespace Questao5.Tests
{
    public class ConsultaSaldoHandlerTests
    {
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly IObterSaldoUseCase _obterSaldoUseCase;
        private readonly ConsultaSaldoHandler _consultaSaldoHandler;

        public ConsultaSaldoHandlerTests()
        {
            _contaBancariaRepository = Substitute.For<IContaBancariaRepository>();
            _obterSaldoUseCase = Substitute.For<IObterSaldoUseCase>();
            _consultaSaldoHandler = new(_contaBancariaRepository, _obterSaldoUseCase);
        }

        [Theory]
        [InlineData(100.50)]
        [InlineData(250.75)]
        [InlineData(0)]
        public async Task Handle_Deve_Retornar_Dados_Bancario_Correto_Com_Saldo_Correto(decimal saldoEsperado)
        {
            long numeroConta = 123;
            string titularNome = "Felipe Estevam";

            ConsultaSaldoQuery query = new(numeroConta);
            CancellationToken cancellationToken = CancellationToken.None;

            ContaCorrenteResponse contaCorrenteResponse = new()
            {
                Idcontacorrente = Guid.NewGuid().ToString(),
                Numero = numeroConta,
                Nome = titularNome,
                Ativo = 1
            };

#pragma warning disable CS8620
            _contaBancariaRepository.GetContaCorrenteAsync(
                Arg.Any<ContaCorrenteRequest>(),
                cancellationToken
            ).Returns(Task.FromResult(contaCorrenteResponse));
#pragma warning restore CS8620

            _obterSaldoUseCase
                .GetSaldoContaCorrenteAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(saldoEsperado));

            Result<ConsultaSaldoResponse> resultado = await _consultaSaldoHandler.Handle(query, cancellationToken);

            Assert.True(resultado.IsSuccess);
            Assert.NotNull(resultado.Data);
            Assert.Equal(numeroConta, resultado.Data.NumeroConta);
            Assert.Equal(titularNome, resultado.Data.TitularNome);
            Assert.Equal(saldoEsperado, resultado.Data.Saldo);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Falha_Quando_Conta_Inativa()
        {
            long numeroConta = 123;
            string idcontacorrente = Guid.NewGuid().ToString();
            CancellationToken cancellationToken = CancellationToken.None;

            var contaCorrenteResponse = new ContaCorrenteResponse
            {
                Idcontacorrente = idcontacorrente,
                Numero = numeroConta,
                Nome = "Maria Souza",
                Ativo = 0
            };

#pragma warning disable CS8620
            _contaBancariaRepository
                .GetContaCorrenteAsync(Arg.Any<ContaCorrenteRequest>(), cancellationToken)
                .Returns(Task.FromResult(contaCorrenteResponse));
#pragma warning restore CS8620

            ConsultaSaldoQuery query = new(numeroConta);

            Result<ConsultaSaldoResponse> resultado = await _consultaSaldoHandler.Handle(query, cancellationToken);

            Assert.False(resultado.IsSuccess);
            Assert.Equal("Conta inativa", resultado.Mensagem);
            Assert.Equal(TipoResponse.INACTIVE_ACCOUNT, resultado.Tipo);
        }

        [Fact]
        public async Task Handle_Deve_Retornar_Falha_Quando_Conta_Inexistente()
        {
            long numeroConta = 123;
            CancellationToken cancellationToken = CancellationToken.None;

            _contaBancariaRepository
                .GetContaCorrenteAsync(Arg.Any<ContaCorrenteRequest>(), cancellationToken)
                .Returns(Task.FromResult((ContaCorrenteResponse?)null));

            ConsultaSaldoQuery query = new(numeroConta);

            Result<ConsultaSaldoResponse> resultado = await _consultaSaldoHandler.Handle(query, cancellationToken);

            Assert.False(resultado.IsSuccess);
            Assert.Equal("Conta não cadastrada", resultado.Mensagem);
            Assert.Equal(TipoResponse.INVALID_ACCOUNT, resultado.Tipo);
        }
    }
}
