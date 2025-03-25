using Microsoft.AspNetCore.Mvc;
using MediatR;
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

        [HttpGet("saldo/{numeroConta:int}")]
        public async Task<IActionResult> ConsultarSaldo(int numeroConta)
        {
            Result<ConsultaSaldoResponse> response = await _mediator.Send(new ConsultaSaldoQuery(numeroConta));

            if (response.IsSuccess)
                return Ok(response.Data);

            return BadRequest(new ResponseError(response.Mensagem, response.Tipo));
        }
    }
}
