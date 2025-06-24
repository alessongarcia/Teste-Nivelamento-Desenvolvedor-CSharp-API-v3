using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Resources;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Xunit;

namespace Questao5.Tests.Application.Handlers
{
    public class CriarMovimentacaoCommandHandlerTests
    {
        private readonly IMovimentoCommandStore _commandStore = Substitute.For<IMovimentoCommandStore>();
        private readonly IContaQueryStore _queryStore = Substitute.For<IContaQueryStore>();
        private readonly CriarMovimentacaoCommandHandler _handler;

        public CriarMovimentacaoCommandHandlerTests()
        {
            _handler = new CriarMovimentacaoCommandHandler(_commandStore, _queryStore);
        }

        [Fact]
        public async Task Deve_CriarMovimento_QuandoDadosValidos()
        {
            var command = new CriarMovimentacaoCommand
            {
                Idempotencia = "REQ-001",
                IdContaCorrente = Guid.NewGuid(),
                Valor = 100,
                Tipo = (char)TipoMovimento.Credito
            };

            _commandStore.ObterResultadoIdempotenteAsync(command.Idempotencia)
                         .Returns((string?)null);

            _queryStore.ObterContaAsync(command.IdContaCorrente)
                       .Returns(new ContaCorrente
                       {
                           Id = command.IdContaCorrente.ToString(),
                           Ativo = true,
                           Nome = "Fulano",
                           Numero = 123
                       });

            _commandStore.InserirMovimentoAsync(Arg.Any<Movimento>())
                         .Returns(Guid.NewGuid());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.IdMovimento);
        }

        [Fact]
        public async Task Deve_LancarErro_QuandoContaInvalida()
        {
            var command = new CriarMovimentacaoCommand
            {
                Idempotencia = "REQ-002",
                IdContaCorrente = Guid.NewGuid(),
                Valor = 50,
                Tipo = (char)TipoMovimento.Credito
            };

            _commandStore.ObterResultadoIdempotenteAsync(command.Idempotencia).Returns((string?)null);
            _queryStore.ObterContaAsync(command.IdContaCorrente).Returns((ContaCorrente?)null);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal(MensagensErro.ContaInvalida, ex.Message);
        }
    }
}
