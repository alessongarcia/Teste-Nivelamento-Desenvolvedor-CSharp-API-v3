using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class CriarMovimentacaoCommand : IRequest<CriarMovimentacaoResponse>
    {
        public string Idempotencia { get; set; }
        public Guid IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public char Tipo { get; set; }
    }
}
