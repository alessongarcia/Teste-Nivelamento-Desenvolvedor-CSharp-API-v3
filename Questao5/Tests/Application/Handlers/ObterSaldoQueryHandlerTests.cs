using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Resources;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Xunit;

namespace Questao5.Tests.Application.Handlers
{
    public class ObterSaldoQueryHandlerTests
    {
        private readonly IContaQueryStore _queryStore = Substitute.For<IContaQueryStore>();
        private readonly ObterSaldoQueryHandler _handler;

        public ObterSaldoQueryHandlerTests()
        {
            _handler = new ObterSaldoQueryHandler(_queryStore);
        }

        [Fact]
        public async Task Deve_RetornarSaldo_Correto()
        {
            var id = Guid.NewGuid();

            _queryStore.ObterContaAsync(id).Returns(new ContaCorrente
            {
                Id = id.ToString(),
                Nome = "Teste",
                Numero = 123,
                Ativo = true
            });

            _queryStore.ObterCreditoAsync(id).Returns(200);
            _queryStore.ObterDebitoAsync(id).Returns(50);

            var result = await _handler.Handle(new ObterSaldoQuery(id), CancellationToken.None);

            Assert.Equal("Teste", result.Nome);
            Assert.Equal(150, result.Saldo);
        }

        [Fact]
        public async Task Deve_LancarErro_QuandoContaNaoExiste()
        {
            var id = Guid.NewGuid();
            _queryStore.ObterContaAsync(id).Returns((ContaCorrente?)null);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
                _handler.Handle(new ObterSaldoQuery (id), CancellationToken.None));

            Assert.Equal(MensagensErro.ContaInvalida, ex.Message);
        }
    }
}
