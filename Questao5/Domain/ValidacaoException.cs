using Questao5.Domain.Language;

namespace Questao5.Domain
{
    public class ValidacaoException : Exception
    {
        public TipoResponse TipoResponse { get; }

        public ValidacaoException(string message, TipoResponse tipoResponse) : base(message)
        {
            TipoResponse = tipoResponse;
        }
    }
}
