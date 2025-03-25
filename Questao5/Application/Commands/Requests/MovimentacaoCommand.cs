using MediatR;
using Questao5.Domain.Language;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoCommand : IRequest<Result<string>>
    {
        public string IdentificacaoRequisiacao { get; set; }
        public long Numero { get; set; }
        public string TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}
