using Newtonsoft.Json;

namespace Questao5.Application.Queries.Responses
{
    public class ConsultaSaldoResponse
    {
        public ConsultaSaldoResponse(string numeroConta, string titularNome, decimal saldo)
        {
            NumeroConta = numeroConta;
            TitularNome = titularNome;
            Saldo = saldo;
        }

        public string NumeroConta { get; private set; }
        public string TitularNome { get; private set; }
        public string DataConsulta => DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        public decimal Saldo { get; private set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
