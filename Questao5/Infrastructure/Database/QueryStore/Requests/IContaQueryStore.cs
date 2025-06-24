using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public interface IContaQueryStore
    {
        Task<ContaCorrente?> ObterContaAsync(Guid id);
        Task<decimal> ObterCreditoAsync(Guid id);
        Task<decimal> ObterDebitoAsync(Guid id);
    }
}
