using Newtonsoft.Json;
using Questao5.Domain.Language;

namespace Questao5.Application.Queries.Responses
{
    public class ResponseError
    {
        public ResponseError(string? mensagem, TipoResponse? tipo)
        {
            Mensagem = mensagem;
            Tipo = tipo?.ToString();
        }

        public string? Tipo { get; private set; }
        public string? Mensagem { get; private set; }

        public override string? ToString() => JsonConvert.SerializeObject(this);
    }
}
