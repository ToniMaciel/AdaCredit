using AdaCredit.Controllers;
using ConsoleTools;
using Sharprompt;
using System;

namespace AdaCredit.UI
{
    public static class UserInterface
    {
        private static Facade _facade = new();
        public static void Login(string[] args)
        {
            // TODO: Verificar se é o primeiro login ou se não há funcionários
            bool successfulLogin = false;
            string username = "", secret;
            
            while (!successfulLogin)
            {
                username = Prompt.Input<string>("Insira seu login");
                secret = Prompt.Password("Insira sua senha");

                successfulLogin = _facade.ValidLogin(username, secret);
                
                if (!successfulLogin)
                {
                    Console.Clear();
                    Console.WriteLine("Login ou senha inválidos, por favor tente novamente!");
                }
            }

            // TODO: print msg de boas vindas e esperar no prompt
            // TODO: atualizar login ao logar
            Console.WriteLine("\nLogin feito com sucesso!\n<<Pressione qualquer tecla para continuar>>");
            Console.ReadKey();
            Run(args, username);
        }
        public static void Run(string[] args, string username)
        {
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
              .Add("Clientes", () => UserInterfaceClient.Run(args, _facade.GetClientController(), username))
              .Add("Funcionários", () => UserInterfaceEmployee.Run(args, _facade, username))
              .Add("Transações", () => UserInterfaceTransaction.Run(args, _facade.GetTransactionController()))
              .Add("Relatórios", reportsMenu.Show)
              .Add("Fechar", ConsoleMenu.Close)
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
