namespace Questao5.Infrastructure.Models
{
    public class Movimentacao
    {
        public string IdMovimentacao { get; set; }
        public string IdContaCorrente { get; set; }
        public string DataMovimento { get; set; }
        public char TipoMovimento { get; set; }
        public decimal Valor { get; set; }

        public Conta conta { get; set; }
    }
}
