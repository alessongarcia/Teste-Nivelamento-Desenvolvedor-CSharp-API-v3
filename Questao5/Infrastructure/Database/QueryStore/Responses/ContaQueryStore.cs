using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class ContaQueryStore : Requests.IContaQueryStore
    {
        private readonly string _conn = "Data Source=database.sqlite";

        public async Task<ContaCorrente?> ObterContaAsync(Guid id)
        {
            using var connection = new SqliteConnection(_conn);
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(
                @"SELECT idcontacorrente AS Id, numero, nome, 
                         CASE ativo WHEN 1 THEN 1 ELSE 0 END AS Ativo 
                  FROM contacorrente WHERE idcontacorrente = @id", new { id });
        }

        public async Task<decimal> ObterCreditoAsync(Guid id)
        {
            using var connection = new SqliteConnection(_conn);
            return await connection.ExecuteScalarAsync<decimal>(
                "SELECT IFNULL(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @id AND tipomovimento = 'C'", new { id });
        }

        public async Task<decimal> ObterDebitoAsync(Guid id)
        {
            using var connection = new SqliteConnection(_conn);
            return await connection.ExecuteScalarAsync<decimal>(
                "SELECT IFNULL(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @id AND tipomovimento = 'D'", new { id });
        }
    }
}
