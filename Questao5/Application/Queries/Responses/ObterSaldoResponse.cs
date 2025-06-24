namespace Questao5.Application.Queries.Responses
{
    public class ObterSaldoResponse
    {
        public int Numero { get; set; }
        public string Nome { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public decimal Saldo { get; set; }
    }
}
