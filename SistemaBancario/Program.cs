﻿using SistemaBancário;
using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml.Linq;


List<Titular> clientes = new List<Titular>();


bool execution = true;
while (execution)
{

    Console.WriteLine("------------------");
    Console.WriteLine("| BANCO JVSS1 S.A |");
    Console.WriteLine("------------------");

    Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
    Console.WriteLine("1 - Criar Nova conta");
    Console.WriteLine("2 - Listar correntistas");
    Console.WriteLine("3 - Sacar");
    Console.WriteLine("4 - Depositar");
    Console.WriteLine("5 - Transferir");
    Console.WriteLine("6 - Encerrar uma conta");
    Console.WriteLine("7 - Fechar aplicação");
    Console.WriteLine("=-=-=-=-=-=-=-=-=-=-=-=-=-=");
    Console.WriteLine("O que quer fazer? ");
    

    int opção;
    if (!int.TryParse(Console.ReadLine(), out opção) || opção < 1)
    {
        Console.WriteLine("Opção inválida");
        continue;
    }

    switch (opção)
    {
        case 1:
            NovaConta();
            break;
        case 2:
            ListarTitulares();
            break;
        case 3:
            Sacar();
            break;
        case 4:
            Depositar();
            break;
        case 5:
            Transferir();
            break;
        case 6:
            EncerrarConta();
            break;
        case 7:
            FinalizarSistema();
            break;
        default:
            Console.WriteLine("Opção Inválida");
            break;


    }
}

//Método NovaConta - Não me perder na identação
void NovaConta()
{ 
    Console.Write("Informe seu nome e sobrenome: ");
    string? name = Console.ReadLine().ToUpper();

    // Variável pra receber nome sem espaço para dar ao regex e ele validar corretamente
    string NameNoSpace = name.Replace(" ", "");

    //Validação do Regex
    bool namevalidation = Regex.IsMatch(NameNoSpace, "^[a-zA-Z]+$");
    if (!namevalidation)
    {
        Console.WriteLine("Nome Inválido.");
        return;
    }

    //Validação para string nula ou vazia
    if (string.IsNullOrEmpty(name) || name.Length < 2)
    {
        Console.WriteLine("Nome Inválido.");
        return;
    }

    Console.Write("Informe o CPF do titular: ");
    string? cpf = Console.ReadLine();

    bool CpfValidation = Regex.IsMatch(cpf, "^[0-9]{3}[0-9]{3}[0-9]{3}[0-9]{2}$");


    if (CpfValidation == false)
    {
        Console.WriteLine("CPF Inválido.");
        return;
    }

    if (string.IsNullOrEmpty(cpf) || cpf.Length != 11)
    {
        Console.WriteLine("CPF inválido.");
        return;
    }

    Console.Write("Informe o RG do titular: ");
    string? rg = (Console.ReadLine());

    bool RgValidation = Regex.IsMatch(rg, "^[0-9]{7,14}$");

    if (!RgValidation)
    {
        Console.WriteLine("RG inválido.");
        return;
    }

    if (string.IsNullOrEmpty(rg) || rg.ToString().Length < 7)
    {
        Console.WriteLine("RG inválido");
        return;
    }

    long ValorCpf = Convert.ToInt64(cpf);
    long ValorRg = Convert.ToInt64(rg);
    if (ValorCpf == ValorRg || ValorRg == ValorCpf)
    {
        Console.WriteLine($"RG: {rg} CPF: {ValorCpf}");
        Console.WriteLine("RG OU CPF IGUAIS.");
        return;
    }

    Console.Write
        ("Insira uma senha. (Será usada para qualquer tipo de operação (6 dígitos apenas números)): ");
    string Password = Console.ReadLine();
    if (!Regex.IsMatch(Password, "^[0-9]{6}$"))
    {
        Console.WriteLine("SENHA INVÁLIDA.");
        return;
    }

    Random random = new Random();
    int conta = random.Next(10000, 80000);

    // Iniciando a variável
    double saldo = 0;

    Titular titular = new Titular(name, cpf, rg, conta, saldo, Password);

    //Adicionando os dados do cliente a Lista 
    clientes.Add(titular);

    Console.WriteLine("Conta criada com sucesso!");
}

//Método ListarTitulares
void ListarTitulares()
{ 
    if (clientes.Count == 0)
    {
        Console.WriteLine("SEM CORRENTISTAS.");
        return;
    }
    foreach (Titular titularesview in clientes)
    {
        Console.WriteLine("============================");
        Console.WriteLine($"Nome: {titularesview.Nome}");
        Console.WriteLine($"CPF: {titularesview.CPF}");
        Console.WriteLine($"RG: {titularesview.RG}");
        Console.WriteLine($"Conta: {titularesview.Conta}");
        Console.WriteLine($"Saldo: R$ {titularesview.Saldo:F2}");
        Console.WriteLine("============================");
    }
}

//Método Sacar 
void Sacar()
{
    Console.Write("Informe o número da conta a ser sacado: ");
    string ValidationAccount = Console.ReadLine();
    if (!Regex.IsMatch(ValidationAccount, "^[0-9]{5}$"))
    {
        Console.WriteLine("Conta inválida.");
        return;
    }

    int ContaNumero = Convert.ToInt32(ValidationAccount);

    Console.Write("Informe o valor a ser sacado: \n");
    string ValorSacarValidation = Console.ReadLine();
    if (!Regex.IsMatch(ValorSacarValidation, "^[0-9]+$"))
    {
        Console.WriteLine("Valor inválido");
        return;
    }

    Console.Write("Informe a senha: ");
    string Password = Console.ReadLine();

    double ValorSacar = Convert.ToDouble(ValorSacarValidation);

    Titular titular = clientes.Find(cliente => cliente.Conta == ContaNumero && cliente.Senha == Password);

    if (titular == null)
    {
        Console.WriteLine("CONTA ou SENHA INVÁLIDAS");
        return;
    }

    if (titular.Saldo >= ValorSacar)
    {
        titular.Saldo -= ValorSacar;
        Console.WriteLine($"Saque de R${ValorSacar:F2} realizado com sucesso");
    }
    else
    {
        Console.WriteLine("SALDO INSUFICIENTE");
    }
}

// Método Depositar
void Depositar()
{
    Console.Write("Informe o número da conta de quem vai receber o depósito: ");
    string AccountNumberValidation = Console.ReadLine();

    if (!Regex.IsMatch(AccountNumberValidation, "^[0-9]{5}$"))
    {
        Console.WriteLine("Conta inválida.");
        return;
    }

    int AccountNumber = Convert.ToInt32(AccountNumberValidation);

    Console.Write("Informe o valor a ser depositado: ");
    string DepositValueValidation = Console.ReadLine();

    if (!Regex.IsMatch(DepositValueValidation, "^[0-9]+$"))
    {
        Console.WriteLine("Valor inválido.");
        return;
    }

    double DepositValue = Convert.ToDouble(DepositValueValidation);


    Titular titular = clientes.Find(cliente => cliente.Conta == AccountNumber);

    if (titular == null)
    {
        Console.WriteLine("Conta não encontrada.");
        return;
    }

    titular.Saldo += DepositValue;
    Console.WriteLine($"O depósito de R${DepositValue:F2} foi realizado com sucesso para {titular.Nome}");

}

//Método Transferir

void Transferir()
{
    //INFORMAÇÕES DA CONTA ORIGEM
    Console.Write("Informe a conta de origem que vai realizar a transferência: ");
    string ValidaçãoContaOrigem = Console.ReadLine();

    if (!Regex.IsMatch(ValidaçãoContaOrigem, "^[0-9]{5}$"))
    {
        Console.WriteLine("Conta inválida");
        return;
    }

    int ContaOrigem = Convert.ToInt32(ValidaçãoContaOrigem);

    //INFORMAÇÕES DA CONTA DESTINO
    Console.Write("Informe a conta que deseja transferir: ");
    string ValidaçãoContaDestino = Console.ReadLine();

    if (!Regex.IsMatch(ValidaçãoContaDestino, "^[0-9]{5}$"))
    {
        Console.WriteLine("Conta Destino Inválida");
        return;
    }
    int ContaDestino = Convert.ToInt32(ValidaçãoContaDestino);

    //VALOR DESEJAVEL PARA TRANSFERENCIA
    Console.Write("Informe agora o valor que deseja transferir: ");
    string ValidaçãoValorTransfer = Console.ReadLine();

    if (!Regex.IsMatch(ValidaçãoValorTransfer, "^[0-9]+$"))
    {
        Console.WriteLine("Valor inválido.");
        return;
    }

    //RECEBIMENTO DA SENHA PARA AUTORIZAÇÃO DA TRANSFERENCIA
    Console.Write("informe sua senha: ");
    string Password = Console.ReadLine();
    //VALIDAÇÃO DO PASSWORD COM REGEX
    if(!Regex.IsMatch(Password, "^[0-9]{6}$"))
    {
        Console.WriteLine("SENHA INVÁLIDA.");
        return;
    }

    //  CONVERTENDO A VARIAVEL PARA REALIZAR PROCEDIMENTOS CORRETOS
    Double ValorTransfer = Convert.ToDouble(ValidaçãoValorTransfer);

    Titular titularorigem = clientes.Find(clientes => clientes.Conta == ContaOrigem && clientes.Senha == Password);
    Titular titulardestino = clientes.Find(clientes => clientes.Conta == ContaDestino);

    if (titularorigem == null || titulardestino == null)
    {
        Console.WriteLine("CONTAS ou SENHA INVÁLIDAS");
    }

    titularorigem.Saldo -= ValorTransfer;

    titulardestino.Saldo += ValorTransfer;

    Console.WriteLine($"Transferência de R${ValorTransfer:F2} realizada com sucesso para {titulardestino.Nome}");
}
//Método EncerrarConta
void EncerrarConta()
{
    Console.Write("Digite o número da conta que deseja excluir: ");
    string ValidaçãoExcluirConta = Console.ReadLine();
    if (!Regex.IsMatch(ValidaçãoExcluirConta, "^[0-9]{5}$"))
    {
        Console.WriteLine("Conta inválida.");
        return;
    }
    int ExcluirConta = Convert.ToInt32(Console.ReadLine());

    Console.Write("informe sua senha: ");
    string Password = Console.ReadLine();

    Titular titular = clientes.Find(clientes => clientes.Conta == ExcluirConta && clientes.Senha == Password);

    if (titular == null)
    {
        Console.WriteLine("CONTA ou SENHA INVÁLIDAS.");
    }
    else
    {
        clientes.Remove(titular);
        Console.WriteLine($"Cadastro de {titular.Nome} excluído com sucesso.");
    }
}

//Finalizar o Sistema
void FinalizarSistema()
{
    execution = false;
    Console.WriteLine("Sessão Encerrada.");
}

