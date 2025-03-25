namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class IdempotenciaResponse
    {
        public string chave_idempotencia { get; set; }
        public string requisicao { get; set; }
        public string resultado { get; set; }
    }
}
