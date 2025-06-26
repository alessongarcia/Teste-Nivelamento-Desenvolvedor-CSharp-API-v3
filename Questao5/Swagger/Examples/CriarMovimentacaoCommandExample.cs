using Questao5.Application.Commands.Requests;
using Questao5.Domain.Enumerators;
using Swashbuckle.AspNetCore.Filters;

namespace Questao5.Swagger.Examples
{
    public class CriarMovimentacaoCommandExample : IExamplesProvider<CriarMovimentacaoCommand>
    {
        public CriarMovimentacaoCommand GetExamples() => new()
        {
            Idempotencia = "REQ-001",
            IdContaCorrente = Guid.Parse("B6BAFC09-6967-ED11-A567-055DFA4A16C9"),
            Tipo = (char)TipoMovimento.Credito,
            Valor = 150.75M
        };
    }
}
