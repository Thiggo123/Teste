namespace Questao5.Infrastructure.Models
{
    public class Conta
    {
        public string IdConta { get; set; }
        public int NumeroConta { get; set; }
        public string NomeTitular {  get; set; }    
        public bool Ativo { get; set; }

        public Decimal Saldo {  get; set; }


    }
}
