using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Database
{
    public interface IContaBancariaRepository
    {
        Task<ContaCorrenteResponse?> GetContaCorrenteAsync(
            ContaCorrenteRequest request, 
            CancellationToken cancellationToken
        );
        Task<IEnumerable<MovimentoResponse>> GetMovimentacoesAsync(
            MovimentoRequest request,
            CancellationToken cancellationToken
        );
        Task<IdempotenciaResponse?> GetImpotenciaAsync(
            IdempotenciaRequest request, 
            CancellationToken cancellationToken
        );
        Task<InserirMovimentacaoResponse> InserirMovimentacaoAsync(
            InserirMovimentacaoRequest request, 
            CancellationToken cancellationToken
        );
    }
}
