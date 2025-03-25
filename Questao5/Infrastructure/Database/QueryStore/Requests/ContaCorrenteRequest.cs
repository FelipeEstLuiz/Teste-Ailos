namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class ContaCorrenteRequest
    {
        public ContaCorrenteRequest(long numero)
        {
            Numero = numero;
        }

        public long Numero { get; }
    }
}
