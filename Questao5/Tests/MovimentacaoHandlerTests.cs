using Newtonsoft.Json;
using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Xunit;

namespace Questao5.Tests
{
    public class MovimentacaoHandlerTests
    {
        private readonly IContaBancariaRepository _contaBancariaRepositoryMock;
        private readonly MovimentacaoHandler _movimentacaoHandler;

        public MovimentacaoHandlerTests()
        {
            _contaBancariaRepositoryMock = Substitute.For<IContaBancariaRepository>();
            _movimentacaoHandler = new(_contaBancariaRepositoryMock);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoTipoMovimentoInvalido()
        {
            MovimentacaoCommand movimentacaoCommand = new()
            {
                IdentificacaoRequisiacao = Guid.NewGuid().ToString(),
                Numero = 123,
                TipoMovimento = "A",
                Valor = 10
            };

            Result<string> resultado = await _movimentacaoHandler.Handle(
                movimentacaoCommand,
                CancellationToken.None
            );

            Assert.False(resultado.IsSuccess);
            Assert.Equal("Tipo de movimentacao invalido", resultado.Mensagem);
            Assert.Equal(TipoResponse.INVALID_TYPE, resultado.Tipo);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Handle_DeveRetornarErro_QuandoValorMenorOuIgualAZero(decimal valor)
        {
            MovimentacaoCommand movimentacaoCommand = new()
            {
                IdentificacaoRequisiacao = Guid.NewGuid().ToString(),
                Numero = 123,
                TipoMovimento = "C",
                Valor = valor
            };

            Result<string> resultado = await _movimentacaoHandler.Handle(
                movimentacaoCommand,
                CancellationToken.None
            );

            Assert.False(resultado.IsSuccess);
            Assert.Equal("Valor deve ser maior que 0", resultado.Mensagem);
            Assert.Equal(TipoResponse.INVALID_VALUE, resultado.Tipo);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoContaNaoCadastrada()
        {
            MovimentacaoCommand movimentacaoCommand = new()
            {
                IdentificacaoRequisiacao = Guid.NewGuid().ToString(),
                Numero = 123,
                TipoMovimento = "C",
                Valor = 10
            };

            _contaBancariaRepositoryMock
                .GetContaCorrenteAsync(Arg.Any<ContaCorrenteRequest>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<ContaCorrenteResponse?>(null));

            Result<string> resultado = await _movimentacaoHandler.Handle(
                movimentacaoCommand,
                CancellationToken.None
            );

            Assert.False(resultado.IsSuccess);
            Assert.Equal("Conta nao cadastrada", resultado.Mensagem);
            Assert.Equal(TipoResponse.INVALID_ACCOUNT, resultado.Tipo);
        }

        [Fact]
        public async Task Handle_DeveRetornarErro_QuandoContaInativa()
        {
            ContaCorrenteResponse contaInativa = new()
            {
                Numero = 123,
                Ativo = 0,
                Idcontacorrente = "1",
                Nome = "Maria Souza"
            };

            MovimentacaoCommand movimentacaoCommand = new()
            {
                IdentificacaoRequisiacao = Guid.NewGuid().ToString(),
                Numero = 123,
                TipoMovimento = "C",
                Valor = 10
            };

#pragma warning disable CS8620
            _contaBancariaRepositoryMock
                .GetContaCorrenteAsync(Arg.Any<ContaCorrenteRequest>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(contaInativa));
#pragma warning restore CS8620

            Result<string> resultado = await _movimentacaoHandler.Handle(
               movimentacaoCommand,
               CancellationToken.None
            );

            Assert.False(resultado.IsSuccess);
            Assert.Equal("Conta inativa", resultado.Mensagem);
            Assert.Equal(TipoResponse.INACTIVE_ACCOUNT, resultado.Tipo);
        }

        [Fact]
        public async Task Handle_DeveRetornarIdMovimento_QuandoMovimentacaoBemSucedida()
        {
            string idMovimento = Guid.NewGuid().ToString();

            ContaCorrenteResponse contaAtiva = new()
            {
                Numero = 123,
                Nome = "Maria Souza",
                Idcontacorrente = "1",
                Ativo = 1
            };

            MovimentacaoCommand movimentacaoCommand = new()
            {
                IdentificacaoRequisiacao = Guid.NewGuid().ToString(),
                Numero = 123,
                TipoMovimento = "C",
                Valor = 10
            };

            InserirMovimentacaoResponse inserirMovimentacaoResponse = new()
            {
                Idcontacorrente = contaAtiva.Idcontacorrente,
                Valor = 100,
                TipoMovimento = "C",
                IdentificacaoRequisiacao = Guid.NewGuid().ToString(),
                IdMovimento = idMovimento
            };


#pragma warning disable CS8620
            _contaBancariaRepositoryMock
                .GetContaCorrenteAsync(Arg.Any<ContaCorrenteRequest>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(contaAtiva));
#pragma warning restore CS8620

            _contaBancariaRepositoryMock
                .GetImpotenciaAsync(Arg.Any<IdempotenciaRequest>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IdempotenciaResponse?>(null));

            _contaBancariaRepositoryMock
                .InserirMovimentacaoAsync(Arg.Any<InserirMovimentacaoRequest>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(inserirMovimentacaoResponse));

            Result<string> resultado = await _movimentacaoHandler.Handle(
               movimentacaoCommand,
               CancellationToken.None
            );

            Assert.True(resultado.IsSuccess);
            Assert.Equal(idMovimento, resultado.Data);
        }

        [Fact]
        public async Task Handle_DeveRetornarIdMovimento_QuandoTiverImpotencia()
        {
            string identificacaoRequisiacao = Guid.NewGuid().ToString();

            string idMovimento = Guid.NewGuid().ToString();

            ContaCorrenteResponse contaAtiva = new()
            {
                Numero = 123,
                Nome = "Maria Souza",
                Idcontacorrente = "1",
                Ativo = 1
            };

            MovimentacaoCommand movimentacaoCommand = new()
            {
                IdentificacaoRequisiacao = identificacaoRequisiacao,
                Numero = 123,
                TipoMovimento = "C",
                Valor = 10
            };

            InserirMovimentacaoResponse inserirMovimentacaoResponse = new()
            {
                Idcontacorrente = contaAtiva.Idcontacorrente,
                Valor = 100,
                TipoMovimento = "C",
                IdentificacaoRequisiacao = Guid.NewGuid().ToString(),
                IdMovimento = idMovimento
            };

            IdempotenciaResponse idempotenciaResponse = new()
            {
                chave_idempotencia = identificacaoRequisiacao,
                requisicao = movimentacaoCommand.IdentificacaoRequisiacao,
                resultado = JsonConvert.SerializeObject(inserirMovimentacaoResponse)
            };

#pragma warning disable CS8620
            _contaBancariaRepositoryMock
                .GetContaCorrenteAsync(Arg.Any<ContaCorrenteRequest>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(contaAtiva));
#pragma warning restore CS8620

            _contaBancariaRepositoryMock
                .GetImpotenciaAsync(Arg.Any<IdempotenciaRequest>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IdempotenciaResponse?>(idempotenciaResponse));

            _contaBancariaRepositoryMock
                .InserirMovimentacaoAsync(Arg.Any<InserirMovimentacaoRequest>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(inserirMovimentacaoResponse));

            Result<string> resultado = await _movimentacaoHandler.Handle(
               movimentacaoCommand,
               CancellationToken.None
            );

            Assert.True(resultado.IsSuccess);
            Assert.Equal(idMovimento, resultado.Data);
        }
    }
}
