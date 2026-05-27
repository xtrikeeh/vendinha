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
    Console.WriteLine("- 4: Pesquisar cliente");
    Console.WriteLine("- 5: Atualizar dados cliente");
    Console.WriteLine("- 6: Pagar dívida");
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
                Console.WriteLine("> Id: {0} \n  Nome: {1} \n  Idade: {2} anos \n",
                    cliente.Id, 
                    cliente.Nome, 
                    cliente.Idade);
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

            Console.Write("> Deseja adicionar email para contato? (S - Sim | N - Não): ");
            var opcao_email = Console.ReadLine();

            string email = null;

            if (opcao_email.ToLower() == "s")
            {
                Console.Write("> Email: ");
                email = Console.ReadLine();
            }

            var clientes = new Cliente() { Nome = nome, Cpf = cpf, DataNascimento = data_nascimento, Email = email };
            var resultado = clienteService.Criar(clientes, out _);

            if (!resultado)
            {
                Console.WriteLine("Erro ao cadastrar o cliente.");
            } else
            {
                Console.WriteLine("Cliente cadastrado com sucesso!");
            }
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

            Console.Write("> Valor: ");
            var valor = Decimal.Parse(Console.ReadLine());

            var dividas = new Divida() { ClienteId = id_cliente , Valor = valor};
            var resultado = dividaService.Criar(dividas, out _);

            if (!resultado)
            {
                Console.WriteLine("Erro ao cadastrar a dívida.");
            } else
            {
                Console.WriteLine("Dívida cadastrada com sucesso!");
            }
        }
    }
    else if (opcao == 3)
    {
        var clientes = clienteService.Listar().Where(e => e.Status == true);
        foreach (var cliente in clientes)
        {
            Console.WriteLine("> Id: {0} \n  Nome: {1} \n",
                cliente.Id,
                cliente.Nome);
        }

        Console.Write("> Digite o Id do cliente para excluí-lo: ");
        var id_cliente = int.Parse(Console.ReadLine());

        clienteService.Excluir(id_cliente, out _);
    }
    else if (opcao == 4)
    {
        Console.Write("> Pesquisar por nome: ");
        var nome = Console.ReadLine();

        var clienteBuscados = clienteService.Pesquisar(nome);

        foreach (var cliente in clienteBuscados)
        {
            Console.WriteLine("> Id: {0} \n  Nome: {1} \n",
                cliente.Id,
                cliente.Nome);
        }
    }
    else if (opcao == 5)
    {
        Console.Write("> Digite o Id do cliente: ");
        var id_cliente = int.Parse(Console.ReadLine());

        Console.Write("> Nome (deixe vazio caso não queira alterar): ");
        var novo_nome = Console.ReadLine();

        Console.Write("> Email (deixe vazio caso não queira alterar): ");
        var novo_email= Console.ReadLine();

        var resultado = clienteService.Atualizar(id_cliente, novo_nome, novo_email, out _);

        if (!resultado)
        {
            Console.WriteLine("Erro ao atualizar os dados do cliente.");
        }
        else
        {
            Console.WriteLine("Cliente atualizado com sucesso!");
        }
    }
    else if (opcao == 6)
    {
        Console.Write("> Digite o Id da dívida para paga-lá: ");
        var id_divida = int.Parse(Console.ReadLine());

        var resultado = dividaService.Pagar(id_divida, out _);

        if (!resultado)
        {
            Console.WriteLine("Erro ao pagar dívida.");
        }
        else
        {
            Console.WriteLine("Dívida paga com sucesso!");
        }
    }
    else if (opcao == 10)
    {
        Console.Clear();
    }
}