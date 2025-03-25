namespace Questao5.Infrastructure.Database.CommandStore.Responses
{
    public class InserirMovimentacaoResponse
    {
        public string IdentificacaoRequisiacao { get; set; }
        public string IdMovimento { get; set; }
        public string Idcontacorrente { get; set; }
        public string TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}
