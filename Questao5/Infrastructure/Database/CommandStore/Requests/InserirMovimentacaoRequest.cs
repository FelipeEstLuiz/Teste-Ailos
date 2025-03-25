using Questao5.Domain;
using Questao5.Domain.Language;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{

    public class InserirMovimentacaoRequest
    {
        private readonly string[] TiposPermitidos = new string[]
        {
            "C", "D"
        };

        public InserirMovimentacaoRequest(
            string identificacaoRequisiacao,
            string idcontacorrente,
            string tipoMovimento,
            decimal valor
        )
        {
            IdentificacaoRequisiacao = identificacaoRequisiacao;
            Idcontacorrente = idcontacorrente;

            if (valor <= 0)
                throw new ValidacaoException("Valor deve ser maior que 0", TipoResponse.INVALID_VALUE);

            Valor = valor;

            if (!TiposPermitidos.Contains(tipoMovimento))
                throw new ValidacaoException("Tipo de movimentacao invalido", TipoResponse.INVALID_TYPE);

            TipoMovimento = tipoMovimento;
        }

        public string IdentificacaoRequisiacao { get; set; }
        public string Idcontacorrente { get; set; }
        public string TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}
