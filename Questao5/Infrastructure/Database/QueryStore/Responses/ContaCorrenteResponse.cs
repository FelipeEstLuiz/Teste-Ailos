namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class ContaCorrenteResponse
    {
        public string Idcontacorrente { get; set; }
        public long Numero { get; set; }
        public string Nome { get; set; }
        public long Ativo { get; set; }

        public bool ContaAtiva => Ativo == 1;
    }
}
