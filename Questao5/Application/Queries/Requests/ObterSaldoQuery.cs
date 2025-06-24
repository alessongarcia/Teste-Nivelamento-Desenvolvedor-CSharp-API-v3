using Questao5.Application.Queries.Responses;
using MediatR;
using System;

namespace Questao5.Application.Queries.Requests
{
    public class ObterSaldoQuery : IRequest<ObterSaldoResponse>
    {
        public Guid IdContaCorrente { get; set; }

        public ObterSaldoQuery(Guid idContaCorrente)
        {
            IdContaCorrente = idContaCorrente;
        }
    }
}
