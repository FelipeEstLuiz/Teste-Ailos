using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Database
{
    public interface IConsultaSaldoRepository
    {
        Task<ContaCorrenteResponse> GetContaCorrenteAsync(ContaCorrenteRequest request);
        Task<IEnumerable<MovimentoResponse>> GetMovimentacoesAsync(MovimentoRequest request);
    }
}
