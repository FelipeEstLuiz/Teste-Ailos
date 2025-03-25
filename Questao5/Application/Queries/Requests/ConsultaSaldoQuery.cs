using MediatR;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Language;

namespace Questao5.Application.Queries.Requests
{
    public class ConsultaSaldoQuery : IRequest<Result<ConsultaSaldoResponse>>
    {
        public ConsultaSaldoQuery(long numero)
        {
            Numero = numero;
        }

        public long Numero { get; private set; }
    }
}
