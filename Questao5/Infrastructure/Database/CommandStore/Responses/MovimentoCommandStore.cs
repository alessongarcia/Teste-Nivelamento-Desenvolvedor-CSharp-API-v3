using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore.Responses
{
    public class MovimentoCommandStore : Requests.IMovimentoCommandStore
    {
        private readonly string _conn = "Data Source=database.sqlite";

        public async Task<Guid> InserirMovimentoAsync(Movimento movimento)
        {
            var id = Guid.NewGuid();

            using var connection = new SqliteConnection(_conn);
            await connection.ExecuteAsync(@"
                INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                VALUES (@id, @idConta, @data, @tipo, @valor)",
                new
                {
                    id = movimento.Id,
                    idConta = movimento.IdContaCorrente,
                    data = movimento.Data.ToString("dd/MM/yyyy"),
                    tipo = movimento.Tipo,
                    valor = movimento.Valor
                });

            return movimento.Id;
        }

        public async Task SalvarIdempotenciaAsync(string chave, string requisicao, string resultado)
        {
            using var connection = new SqliteConnection(_conn);
            await connection.ExecuteAsync(@"
                INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                VALUES (@chave, @req, @res)",
                new { chave, req = requisicao, res = resultado });
        }

        public async Task<string?> ObterResultadoIdempotenteAsync(string chave)
        {
            using var connection = new SqliteConnection(_conn);
            return await connection.QueryFirstOrDefaultAsync<string?>(
                "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @chave",
                new { chave });
        }
    }
}
