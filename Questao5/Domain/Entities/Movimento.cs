namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public Movimento(string id, string idContaCorrente, string dataMovimento, string tipoMovimento, decimal valor)
        {
            Id = id;
            IdContaCorrente = idContaCorrente;
            DataMovimento = dataMovimento;
            TipoMovimento = tipoMovimento;
            Valor = valor;
        }

        public string Id { get; private set; }
        public string IdContaCorrente { get; private set; }
        public string DataMovimento { get; private set; }
        public string TipoMovimento { get; private set; }
        public decimal Valor { get; private set; }
    }
}
