using System.Globalization;
using System.Security.Cryptography;

namespace Questao1
{
    class ContaBancaria
    {

        public int NumeroConta { get; set; }

        public string TitularConta { get; set; }

        public double Saldo { get; set; }

        public ContaBancaria(int numero, string titular, double? deposito)
        {
            NumeroConta = numero;   
            TitularConta = titular;
            Saldo = deposito.Value;
        }

        public void Deposito(double quantia)
        {
            Saldo += quantia;
        }

        public void Saque(double quantia)
        {
            Saldo -= (quantia + 3.50);
        }

        public string DisplayConta()
        {
            string containfo = "Conta: " + NumeroConta + ", Titular: " +TitularConta + ", Saldo: $" +Saldo.ToString("F");

            return containfo;
        }
    }

}
