namespace Questao1
{
    class ContaBancaria
    {

        public int Numero { get; private set; }
        public string Titular { get; private set; }
        public double Saldo { get; private set; }

        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            Numero = numero;
            Titular = titular;
            Saldo = depositoInicial;
        }

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
            Saldo = 0;
        }

        public void Deposito(double valor) => Saldo += valor;

        public void Saque(double valor) => Saldo = (Saldo - valor) - 3.50;

        public override string ToString() => $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo:F2}";
    }
}
