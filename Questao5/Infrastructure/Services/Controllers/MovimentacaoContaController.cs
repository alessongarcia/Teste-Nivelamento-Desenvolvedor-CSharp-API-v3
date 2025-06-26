using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Swagger.Examples;
using Swashbuckle.AspNetCore.Filters;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimentacaoContaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentacaoContaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Realiza uma movimentação em uma conta corrente.
        /// </summary>
        /// <param name="command">Dados da movimentação</param>
        /// <returns>Id do movimento gerado</returns>
        /// <response code="200">Movimentação realizada com sucesso</response>
        /// <response code="400">Erro de validação</response>
        [HttpPost("movimentacao")]
        [SwaggerRequestExample(typeof(CriarMovimentacaoCommand), typeof(CriarMovimentacaoCommandExample))]
        [ProducesResponseType(typeof(CriarMovimentacaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MovimentarConta([FromBody] CriarMovimentacaoCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}