namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class MovimentoResponse
    {
        public string Idmovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public string DataMovimento { get; set; }
        public string TipoMovimento { get; set; }
        public decimal Valor { get; set; }
    }
}
