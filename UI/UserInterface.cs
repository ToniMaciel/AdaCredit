using ConsoleTools;
using System;

namespace AdaCredit.UI
{
    public static class UserInterface
    {
        public static void run(string[] args)
        {
            var clientMenu = new ConsoleMenu(args, level: 1)
              .Add("Cadastrar Novo Cliente", () => SomeAction("Sub_One"))
              .Add("Consultar os Dados de um Cliente existente", () => SomeAction("Sub_Two"))
              .Add("Alterar o Cadastro de um Cliente existente", () => SomeAction("Sub_Three"))
              .Add("Desativar Cadastro de um Cliente existente", () => SomeAction("Sub_Four"))
              .Add("Voltar", ConsoleMenu.Close)
              .Configure(config =>
              {
                  config.Selector = "--> ";
                  config.Title = "Cliente";
                  config.EnableBreadcrumb = true;
                  config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
              });

            var employeeMenu = new ConsoleMenu(args, level: 1)
              .Add("Cadastrar Novo Funcionário", () => SomeAction("Sub_One"))
              .Add("Alterar Senha de um Funcionário existente", () => SomeAction("Sub_Two"))
              .Add("Desativar Cadastro de um Funcionário existente", () => SomeAction("Sub_Three"))
              .Add("Voltar", ConsoleMenu.Close)
              .Configure(config =>
              {
                  config.Selector = "--> ";
                  config.Title = "Funcionário";
                  config.EnableBreadcrumb = true;
                  config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
              });

            var reportsMenu = new ConsoleMenu(args, level: 1)
              .Add("Exibir Todos os Clientes Ativos com seus Respectivos Saldos", () => SomeAction("Sub_One"))
              .Add("Exibir Todos os Clientes Inativos", () => SomeAction("Sub_Two"))
              .Add("Exibir Todos os Funcionários Ativos", () => SomeAction("Sub_Three"))
              .Add("Exibir Transações com Erro", () => SomeAction("Sub_Four"))
              .Add("Voltar", ConsoleMenu.Close)
              .Configure(config =>
              {
                  config.Selector = "--> ";
                  config.Title = "Relatórios";
                  config.EnableBreadcrumb = true;
                  config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
              });

            var mainMenu = new ConsoleMenu(args, level: 0)
              .Add("Clientes", clientMenu.Show)
              .Add("Funcionários", employeeMenu.Show)
              .Add("Transações", (thisMenu) => { SomeAction("Transações"); thisMenu.CloseMenu(); })
              .Add("Relatórios", reportsMenu.Show)
              .Add("", ConsoleMenu.Close)
              .Configure(config =>
              {
                  config.WriteHeaderAction = () => Console.WriteLine("Escolha uma opção:");
                  config.Selector = "--> ";
                  config.Title = "Menu principal";
                  config.EnableWriteTitle = false;
                  config.EnableBreadcrumb = true;
              });

            mainMenu.Show();
        }

        private static void SomeAction(string text)
        {
            Console.WriteLine(text);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
