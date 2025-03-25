namespace Questao5.Domain.Entities
{
    public class ContaCorrente
    {
        public ContaCorrente(string id, int numero, string nome, int ativo)
        {
            Id = id;
            Numero = numero;
            Nome = nome;
            Ativo = ativo;
        }

        public string Id { get; private set; }
        public int Numero { get; private set; }
        public string Nome { get; private set; }
        public int Ativo { get; private set; }
    }
}
