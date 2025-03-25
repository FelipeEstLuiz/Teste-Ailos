using MediatR;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Language;

namespace Questao5.Application.Queries.Requests
{
    public class ConsultaSaldoQuery : IRequest<Result<ConsultaSaldoResponse>>
    {
        public ConsultaSaldoQuery(int numero)
        {
            Numero = numero;
        }

        public int Numero { get; private set; }
    }
}
