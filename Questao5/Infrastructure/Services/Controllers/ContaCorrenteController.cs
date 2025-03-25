using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Language;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("saldo/{numeroConta:long}")]
        public async Task<IActionResult> ConsultarSaldoAsync(long numeroConta)
        {
            Result<ConsultaSaldoResponse> response = await _mediator.Send(new ConsultaSaldoQuery(numeroConta));

            if (response.IsSuccess)
                return Ok(response.Data);

            return BadRequest(new ResponseError(response.Mensagem, response.Tipo));
        }

        [HttpPost("inserir-movimentacao")]
        public async Task<IActionResult> InserirMovimentacaoAsync([FromBody] MovimentacaoCommand command)
        {
            Result<string> response = await _mediator.Send(command);

            if (response.IsSuccess)
                return Ok(response.Data);

            return BadRequest(new ResponseError(response.Mensagem, response.Tipo));
        }
    }
}
