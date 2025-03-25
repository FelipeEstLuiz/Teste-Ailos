namespace Questao5.Domain.Language
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public string? Mensagem { get; private set; }
        public TipoResponse? Tipo { get; private set; }
        public T? Data { get; private set; }

        private Result(T data)
        {
            IsSuccess = true;
            Data = data;
        }

        private Result(string message, TipoResponse tipo)
        {
            IsSuccess = false;
            Mensagem = message;
            Tipo = tipo;
        }

        public static Result<T> Success(T data) => new(data);
        public static Result<T> Failure(string message, TipoResponse tipo) => new(message, tipo);
    }
}
