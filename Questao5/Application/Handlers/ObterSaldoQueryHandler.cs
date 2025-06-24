using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Resources;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace Questao5.Application.Handlers
{
    public class ObterSaldoQueryHandler : IRequestHandler<ObterSaldoQuery, ObterSaldoResponse>
    {
        private readonly IContaQueryStore _contaQueryStore;

        public ObterSaldoQueryHandler(IContaQueryStore contaQueryStore)
        {
            _contaQueryStore = contaQueryStore;
        }

        public async Task<ObterSaldoResponse> Handle(ObterSaldoQuery request, CancellationToken cancellationToken)
        {
            ContaCorrente? conta = await _contaQueryStore.ObterContaAsync(request.IdContaCorrente);

            if (conta == null)
                throw new ArgumentException(MensagensErro.ContaInvalida);

            if (!conta.Ativo)
                throw new ArgumentException(MensagensErro.ContaInativa);

            var credito = await _contaQueryStore.ObterCreditoAsync(request.IdContaCorrente);
            var debito = await _contaQueryStore.ObterDebitoAsync(request.IdContaCorrente);

            return new ObterSaldoResponse
            {
                Numero = conta.Numero,
                Nome = conta.Nome,
                DataHoraConsulta = DateTime.Now,
                Saldo = credito - debito
            };
        }
    }
}
