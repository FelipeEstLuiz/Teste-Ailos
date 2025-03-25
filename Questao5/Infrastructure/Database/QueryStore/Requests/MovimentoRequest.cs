namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class MovimentoRequest
    {
        public MovimentoRequest(string idContaCorrente, string tipoMovimento)
        {
            IdContaCorrente = idContaCorrente;
            TipoMovimento = tipoMovimento;
        }

        public string IdContaCorrente { get; }
        public string TipoMovimento { get; }
    }
}
