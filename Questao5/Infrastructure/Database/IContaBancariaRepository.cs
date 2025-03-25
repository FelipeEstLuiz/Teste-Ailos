using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Database
{
    public interface IContaBancariaRepository
    {
        Task<ContaCorrenteResponse> GetContaCorrenteAsync(ContaCorrenteRequest request);
        Task<IEnumerable<MovimentoResponse>> GetMovimentacoesAsync(MovimentoRequest request);
        Task<IdempotenciaResponse> GetImpotenciaAsync(IdempotenciaRequest request);
        Task<InserirMovimentacaoResponse> InserirMovimentacaoAsync(InserirMovimentacaoRequest request);
    }
}
