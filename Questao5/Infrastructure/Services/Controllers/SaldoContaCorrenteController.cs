using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Swagger.Examples;
using Swashbuckle.AspNetCore.Filters;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaldoContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaldoContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Consulta o saldo atual da conta corrente.
        /// </summary>
        /// <param name="id">ID da conta corrente</param>
        /// <returns>Dados da conta e saldo atual</returns>
        /// <response code="200">Consulta realizada com sucesso</response>
        /// <response code="400">Conta inválida ou inativa</response>
        [HttpGet("{id}/saldo")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ObterSaldoResponseExample))]
        [ProducesResponseType(typeof(ObterSaldoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConsultarSaldo([FromRoute] Guid id)
        {
            try
            {
                var query = new ObterSaldoQuery(id);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
