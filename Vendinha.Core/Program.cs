using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using Vendinha.Core.Models;
using Vendinha.Core.Services;

var clienteService = new ClienteService();
var dividaService = new DividaService();

while (true)
{
    Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection",
        "Server=localhost;Port=5432;User Id=postgres;Password=bmols123;Database=vendinha");

    Console.WriteLine("Digite uma opção");
    Console.WriteLine("- 1: Listar");
    Console.WriteLine("- 2: Criar");
    Console.WriteLine("- 3: Excluir cliente");
    Console.WriteLine("- 0: Encerrar");
    Console.Write("Opção: ");

    var opcao = int.Parse(Console.ReadLine());

    if (opcao == 0)
    {
        Console.WriteLine("--- Programa encerrado ---");
        return 0;
    }
    else if (opcao == 1)
    {
        Console.Write("1 - Clientes | 2 - Dívidas: ");
        var opcao_listar = int.Parse(Console.ReadLine());

        if (opcao_listar == 1)
        {
            var clientes = clienteService.Listar();
            Console.WriteLine("--- Clientes ---");
            foreach (var cliente in clientes)
            {
                Console.WriteLine("> Id: {0} \n  Nome: {1} \n  Idade: {2} anos \n  Status: {3} \n",
                    cliente.Id, 
                    cliente.Nome, 
                    cliente.Idade,
                    cliente.Status);
            }
        }
        else if (opcao_listar == 2)
        {
            var dividas = dividaService.Listar();
            Console.WriteLine("--- Dívidas ---");
            foreach (var divida in dividas)
            {
                if (divida.Situacao == false)
                {
                    Console.WriteLine("> Id: {0} \n  Valor: R$ {1} \n  Data criação: {2} \n  Situação: {3} \n  Id cliente: {4} \n",
                        divida.Id,
                        divida.Valor,
                        divida.DataCriacao,
                        "Não paga",
                        divida.ClienteId);
                }
                else
                {
                    Console.WriteLine("> Id: {0} \n  Valor: R$ {1} \n  Data criação: {2} \n  Situação: {3} \n  Id cliente: {4} \n",
                        divida.Id,
                        divida.Valor,
                        divida.DataCriacao,
                        ("Paga" + $" ({divida.DataPagamento})"),
                        divida.ClienteId);
                }
            }
        }
    }
    else if (opcao == 2)
    {
        Console.Write("1 - Clientes | 2 - Dívidas: ");
        var opcao_listar = int.Parse(Console.ReadLine());

        if (opcao_listar == 1)
        {
            Console.Write("> Nome: ");
            var nome = Console.ReadLine();

            Console.Write("> CPF: ");
            var cpf = Console.ReadLine();

            Console.Write("> Data de Nascimento: ");
            var data_nascimento = DateOnly.Parse(Console.ReadLine());

            Console.Write("> Email: ");
            var email = Console.ReadLine();

            var clientes = new Cliente() { Nome = nome, Cpf = cpf, DataNascimento = data_nascimento, Email = email, Status = true};
            clienteService.Criar(clientes, out _);
            Console.WriteLine("Cliente cadastrado com sucesso!");
        }
        else if (opcao_listar == 2)
        {
            Console.Write("> Id do Cliente: ");
            var id_cliente = int.Parse(Console.ReadLine());

            var clienteExistente = clienteService.Listar().FirstOrDefault(c => c.Id == id_cliente);

            if (clienteExistente == null)
            {
                Console.WriteLine("Erro: cliente não encontrado!");
                continue;
            }

            DateTime data_criacao = DateTime.UtcNow;

            Console.Write("> Valor: ");
            var valor = Decimal.Parse(Console.ReadLine());

            Console.Write("> Situação (S - Paga / N - Não paga): ");
            var opcao_situacao = Console.ReadLine();

            bool situacao;
            DateTime? data_pagamento;

            if (opcao_situacao == "s")
            {
                situacao = true;
                data_pagamento = DateTime.UtcNow;
            } else
            {
                situacao = false;
                Console.Write(" > Data de pagamento (Ex: DD/MM/AAAA HH:MM): ");
                data_pagamento = null;
            }

            var dividas = new Divida() { ClienteId = id_cliente , Valor = valor, Situacao = situacao, DataPagamento = data_pagamento, DataCriacao = data_criacao};
            dividaService.Criar(dividas, out _);
            Console.WriteLine("Dívida cadastrada com sucesso!");
        }
    }
    else if (opcao == 3)
    {
        Console.Write("> Digite o Id do cliente para excluí-lo: ");
        var id_cliente = int.Parse(Console.ReadLine());

        clienteService.Excluir(id_cliente, out _);
    }
}