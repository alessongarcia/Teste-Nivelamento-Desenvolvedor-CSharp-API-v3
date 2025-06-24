using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public interface IMovimentoCommandStore
    {
        Task<Guid> InserirMovimentoAsync(Movimento movimento);
        Task SalvarIdempotenciaAsync(string chave, string requisicao, string resultado);
        Task<string?> ObterResultadoIdempotenteAsync(string chave);
    }
}
