﻿
using AdaCredit.Controllers;
using BetterConsoleTables;
using ConsoleTools;
using System;

namespace AdaCredit.UI
{
    internal class UserInterfaceReport
    {
        public static void Run(string[] args, Facade facade)
        {
            var reportsMenu = new ConsoleMenu(args, level: 1)
              .Add("Exibir Todos os Clientes Ativos com seus Respectivos Saldos", () => ShowClients(facade.GetClientController(), true))
              .Add("Exibir Todos os Clientes Inativos", () => ShowClients(facade.GetClientController(), false))
              .Add("Exibir Todos os Funcionários Ativos", () => ShowEmployees(facade.GetEmployeeControler(), true))
              .Add("Exibir Transações com Erro", () => ShowFailedTransactions(facade.GetTransactionController()))
              .Add("Voltar", ConsoleMenu.Close)
              .Configure(config =>
              {
                  config.Selector = "--> ";
                  config.Title = "Relatórios";
                  config.EnableBreadcrumb = true;
                  config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
              });

            reportsMenu.Show(); 
        }

        private static void ShowClients(ControllerClient controllerClient, bool isActive)
        {
            ColumnHeader[] headers = new[]
            {
                new ColumnHeader("Nome", Alignment.Center, Alignment.Center),
                new ColumnHeader("Telefone", Alignment.Center, Alignment.Center),
                new ColumnHeader("CPF", Alignment.Center, Alignment.Center),
                new ColumnHeader("Número de conta", Alignment.Center, Alignment.Center),
                new ColumnHeader("Saldo", Alignment.Center, Alignment.Center)
            };

            Table table = new(headers);
            foreach (var client in controllerClient.GetClients(isActive))
                table.AddRow(client.Name, client.PhoneNumber, client.Document, client.AccountNumber, client.AccountBalance);
            table.Config = TableConfiguration.Unicode();

            Console.Write(table.ToString());

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }

        private static void ShowEmployees(ControllerEmployee controllerEmployee, bool isActive)
        {
            ColumnHeader[] headers = new[]
            {
                new ColumnHeader("Login", Alignment.Center, Alignment.Center),
                new ColumnHeader("Nome", Alignment.Center, Alignment.Center),
                new ColumnHeader("CPF", Alignment.Center, Alignment.Center),
                new ColumnHeader("Último login", Alignment.Center, Alignment.Center)
            };

            Table table = new(headers);
            foreach (var employee in controllerEmployee.GetEmployees(isActive))
                table.AddRow(employee.Username, employee.Name, employee.Document, employee.LastLogin);
            table.Config = TableConfiguration.Unicode();

            Console.Write(table.ToString());

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }

        private static void ShowFailedTransactions(ControllerTransaction controllerTransaction)
        {
            ColumnHeader[] headers = new[]
            {
                new ColumnHeader("Código Banco (Origem)", Alignment.Center, Alignment.Center),
                new ColumnHeader("Agência (Origem)", Alignment.Center, Alignment.Center),
                new ColumnHeader("N. da conta (Origem)", Alignment.Center, Alignment.Center),
                new ColumnHeader("Código Banco (Destino)", Alignment.Center, Alignment.Center),
                new ColumnHeader("Agência (Destino)", Alignment.Center, Alignment.Center),
                new ColumnHeader("N. da conta (Destino)", Alignment.Center, Alignment.Center),
                new ColumnHeader("Tipo de transação", Alignment.Center, Alignment.Center),
                new ColumnHeader("valor", Alignment.Center, Alignment.Center),
                new ColumnHeader("Detalhes da falha", Alignment.Center, Alignment.Center)
            };

            Table table = new(headers);
            foreach (var transaction in controllerTransaction.GetAllFailedTransactions())
                table.AddRow(transaction.BankCodeSource, 
                            transaction.BankBranchSource, 
                            transaction.AccountNumberSource, 
                            transaction.BankCodeTarget, 
                            transaction.BankBranchTarget, 
                            transaction.AccountNumberTarget,
                            transaction.TransactionType,
                            transaction.Value,
                            transaction.FailedTransactionDetail);
            table.Config = TableConfiguration.Unicode();

            Console.Write(table.ToString());

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }
    }
}