using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Language;
using Questao5.Infrastructure.Services.Controllers;
using Xunit;

namespace Questao5.Tests
{
    public class ContaCorrenteControllerTests
    {
        private readonly IMediator _mediatorMock;
        private readonly ContaCorrenteController _controller;

        public ContaCorrenteControllerTests()
        {
            _mediatorMock = Substitute.For<IMediator>();
            _controller = new ContaCorrenteController(_mediatorMock);
        }

        [Fact]
        public async Task ObterSaldo_DeveRetornar200ComSaldo_QuandoSucesso()
        {
            long numeroConta = 123;
            ConsultaSaldoResponse saldoResponse = new(numeroConta, "Cliente Teste");
            saldoResponse.AtualizarSaldo(150.00m);

            Result<ConsultaSaldoResponse> resultadoMock = Result<ConsultaSaldoResponse>.Success(saldoResponse);

            _mediatorMock
                .Send(Arg.Any<ConsultaSaldoQuery>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(resultadoMock));

            OkObjectResult? resultado = await _controller.ConsultarSaldoAsync(numeroConta) as OkObjectResult;

            Assert.NotNull(resultado);
            Assert.Equal(200, resultado.StatusCode);

            ConsultaSaldoResponse? retorno = resultado.Value as ConsultaSaldoResponse;
            Assert.NotNull(retorno);
            Assert.Equal(150.00m, retorno.Saldo);
        }

        [Fact]
        public async Task ObterSaldo_DeveRetornar400_QuandoContaInvalida()
        {
            long numeroConta = 999;
            Result<ConsultaSaldoResponse> resultadoMock = Result<ConsultaSaldoResponse>.Failure(
                "Conta não cadastrada",
                TipoResponse.INVALID_ACCOUNT
            );

            _mediatorMock
                .Send(Arg.Any<ConsultaSaldoQuery>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(resultadoMock));

            BadRequestObjectResult? resultado = await _controller
                .ConsultarSaldoAsync(numeroConta) as BadRequestObjectResult;

            Assert.NotNull(resultado);
            Assert.Equal(400, resultado.StatusCode);

            ResponseError? retorno = resultado.Value as ResponseError;
            Assert.NotNull(retorno);
            Assert.Equal("Conta não cadastrada", retorno?.Mensagem);
            Assert.Equal(TipoResponse.INVALID_ACCOUNT.ToString(), retorno?.Tipo);
        }
    }
}
