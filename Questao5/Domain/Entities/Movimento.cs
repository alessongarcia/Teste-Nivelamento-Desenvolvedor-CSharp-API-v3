using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public Guid Id { get; set; }
        public Guid IdContaCorrente { get; set; }
        public DateTime Data { get; set; }
        public char Tipo { get; set; }
        public decimal Valor { get; set; }
    }
}
