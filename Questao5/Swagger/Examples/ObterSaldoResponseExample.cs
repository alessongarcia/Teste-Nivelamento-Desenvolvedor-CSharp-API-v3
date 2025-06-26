using Questao5.Application.Queries.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace Questao5.Swagger.Examples
{
    public class ObterSaldoResponseExample : IExamplesProvider<ObterSaldoResponse>
    {
        public ObterSaldoResponse GetExamples()
        {
            return new ObterSaldoResponse
            {
                Numero = 1234,
                Nome = "João da Silva",
                DataHoraConsulta = DateTime.Now,
                Saldo = 345.89M
            };
        }
    }
}
