namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class IdempotenciaRequest
    {
        public IdempotenciaRequest( string identificacaoRequisiacao)
        {
            IdentificacaoRequisiacao = identificacaoRequisiacao;
        }

        public string IdentificacaoRequisiacao { get; }
    }
}
