
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
                  config.WriteHeaderAction = () => Console.WriteLine("Escolha uma opção:");
                  config.Selector = "--> ";
                  config.Title = "Relatórios";
                  config.EnableBreadcrumb = false;
                  config.EnableWriteTitle = true;
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
                new ColumnHeader("Saldo", Alignment.Center, Alignment.Center),
                new ColumnHeader("Último funcionário a alterar", Alignment.Center, Alignment.Center),
            };

            Table table = new(headers);
            foreach (var client in controllerClient.GetClients(isActive))
                table.AddRow(client.Name, client.PhoneNumber, client.Document, client.AccountNumber, $"{client.AccountBalance,0:F2}", client.LastEmployeeToChange);
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
                new ColumnHeader("Último login", Alignment.Center, Alignment.Center),
                new ColumnHeader("Último funcionário a alterar", Alignment.Center, Alignment.Center)
            };

            Table table = new(headers);
            foreach (var employee in controllerEmployee.GetEmployees(isActive))
                table.AddRow(employee.Username, employee.Name, employee.Document, employee.LastLogin, employee.LastEmployeeToChange);
            table.Config = TableConfiguration.Unicode();

            Console.Write(table.ToString());

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }

        private static void ShowFailedTransactions(ControllerTransaction controllerTransaction)
        {
            ColumnHeader[] headers = new[]
            {
                new ColumnHeader("Cód. Banco (Origem)", Alignment.Center, Alignment.Center),
                new ColumnHeader("N. conta (Origem)", Alignment.Center, Alignment.Center),
                new ColumnHeader("Cód. Banco (Destino)", Alignment.Center, Alignment.Center),
                new ColumnHeader("N. conta (Destino)", Alignment.Center, Alignment.Center),
                new ColumnHeader("Transação", Alignment.Center, Alignment.Center),
                new ColumnHeader("Valor", Alignment.Center, Alignment.Center),
                new ColumnHeader("Falha", Alignment.Center, Alignment.Center)
            };

            Table table = new(headers);
            foreach (var transaction in controllerTransaction.GetAllFailedTransactions())
                table.AddRow(transaction.BankCodeSource, 
                            transaction.AccountNumberSource, 
                            transaction.BankCodeTarget, 
                            transaction.AccountNumberTarget,
                            transaction.TransactionType,
                            $"{transaction.Value, 0:F2}",
                            transaction.FailedTransactionDetail);
            table.Config = TableConfiguration.Unicode();

            Console.Write(table.ToString());

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }
    }
}
