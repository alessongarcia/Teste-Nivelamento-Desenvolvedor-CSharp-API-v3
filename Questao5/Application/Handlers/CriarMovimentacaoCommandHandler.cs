using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using MediatR;
using System.Text.Json;
using Questao5.Domain.Resources;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Domain.Entities;

namespace Questao5.Application.Handlers
{
    public class CriarMovimentacaoCommandHandler : IRequestHandler<CriarMovimentacaoCommand, CriarMovimentacaoResponse>
    {
        private readonly IMovimentoCommandStore _movimentoCommandStore;
        private readonly IContaQueryStore _contaQueryStore;

        public CriarMovimentacaoCommandHandler(
            IMovimentoCommandStore movimentoCommandStore,
            IContaQueryStore contaQueryStore)
        {
            _movimentoCommandStore = movimentoCommandStore;
            _contaQueryStore = contaQueryStore;
        }

        public async Task<CriarMovimentacaoResponse> Handle(CriarMovimentacaoCommand request, CancellationToken cancellationToken)
        {
            // Verificar idempotência
            var resultadoExistente = await _movimentoCommandStore.ObterResultadoIdempotenteAsync(request.Idempotencia);
            if (resultadoExistente != null)
                return JsonSerializer.Deserialize<CriarMovimentacaoResponse>(resultadoExistente);

            // Validações
            if (request.Tipo != (char)TipoMovimento.Credito && request.Tipo != (char)TipoMovimento.Debito)
                throw new ArgumentException(MensagensErro.TipoInvalido);

            var conta = await _contaQueryStore.ObterContaAsync(request.IdContaCorrente);
            if (conta == null)
                throw new ArgumentException(MensagensErro.ContaInvalida);

            if (!conta.Ativo)
                throw new ArgumentException(MensagensErro.ContaInativa);

            if (request.Valor <= 0)
                throw new ArgumentException(MensagensErro.ValorInvalido);

            // Inserir movimento
            var movimento = new Movimento
            {
                Id = Guid.NewGuid(),
                IdContaCorrente = request.IdContaCorrente,
                Data = DateTime.Now,
                Tipo = request.Tipo,
                Valor = request.Valor
            };

            var idMovimento = await _movimentoCommandStore.InserirMovimentoAsync(movimento);

            var response = new CriarMovimentacaoResponse { IdMovimento = idMovimento };

            // Salvar idempotência
            await _movimentoCommandStore.SalvarIdempotenciaAsync(
                request.Idempotencia,
                JsonSerializer.Serialize(request),
                JsonSerializer.Serialize(response));

            return response;
        }
    }
}
