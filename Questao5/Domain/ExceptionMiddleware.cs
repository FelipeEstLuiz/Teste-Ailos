using Questao5.Application.Queries.Responses;
using Questao5.Domain.Language;

namespace Questao5.Domain
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidacaoException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ResponseError(ex.Message, ex.TipoResponse));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ResponseError("Ocorreu um erro inesperado", TipoResponse.INTERNAL_ERROR));
            }
        }
    }
}
