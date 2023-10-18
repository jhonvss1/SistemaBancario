using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaBancário
{
    public class Titular
    {

        public string? Nome { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public int Conta { get; set; }
        public Double Saldo { get; set; }

        //Construtor
        public Titular(string? nome, string? cpf, string rg, int conta, double saldo)
        {
            Nome = nome;
            CPF = cpf;
            RG = rg;
            Conta = conta;
            Saldo = saldo;
        }


    }
}
