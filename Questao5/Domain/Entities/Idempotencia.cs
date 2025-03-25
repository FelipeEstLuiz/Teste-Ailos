namespace Questao5.Domain.Entities
{
    public class Idempotencia
    {
        public Idempotencia(string chaveIdempotencia, string requisicao, string resultado)
        {
            ChaveIdempotencia = chaveIdempotencia;
            Requisicao = requisicao;
            Resultado = resultado;
        }

        public string ChaveIdempotencia { get; private set; }
        public string Requisicao { get; private set; }
        public string Resultado { get; private set; }
    }
}
