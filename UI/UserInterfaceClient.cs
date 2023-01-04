using AdaCredit.Controllers;
using ConsoleTools;
using Sharprompt;
using System;

namespace AdaCredit.UI
{
    public static class UserInterfaceClient
    {
        public static void Run(string[] args, ControllerClient controllerClient, string username)
        {
            var clientMenu = new ConsoleMenu(args, level: 1)
              .Add("Cadastrar Novo Cliente", () => AddClient(controllerClient, username))
              .Add("Consultar os Dados de um Cliente", () => ConsultClient(controllerClient))
              .Add("Alterar o Cadastro de um Cliente", () => UpdateClientPhone(controllerClient))
              .Add("Desativar Cadastro de um Cliente", () => DisableClient(controllerClient))
              .Add("Voltar", ConsoleMenu.Close)
              .Configure(config =>
              {
                  config.Selector = "--> ";
                  config.Title = "Cliente";
                  config.EnableBreadcrumb = true;
                  config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
              });

            clientMenu.Show();
        }
        private static void DisableClient(ControllerClient controllerClient)
        {
            var userClient = Prompt.Select("Selecione o cliente", controllerClient.GetClients());
            bool success = controllerClient.DisableClient(userClient);
            if (success)
                Console.WriteLine($"\n{userClient} desativado com sucesso!");
            else
                Console.WriteLine("\nNão foi possível desativar o cliente");

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }

        private static void UpdateClientPhone(ControllerClient controllerClient)
        {
            var userClient = Prompt.Select("Selecione o cliente", controllerClient.GetClients());
            var newPhoneNumber = Prompt.Input<string>($"Digite o novo número de telefone do usuário {userClient}");
            bool success = controllerClient.UpdateClientPhone(userClient, newPhoneNumber);
            if (success)
                Console.WriteLine("\nCliente atualizado com sucesso!");
            else
                Console.WriteLine("\nNão foi possível atualizar o cliente");

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }
        private static void ConsultClient(ControllerClient controllerClient)
        {
            var userClient = Prompt.Select("Selecione o cliente", controllerClient.GetClients());
            Console.WriteLine(userClient.ShowInfos());
            Console.WriteLine("\n<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }

        private static void AddClient(ControllerClient controllerClient, string username)
        {
            var name = Prompt.Input<string>("Insira o nome do cliente");
            var phoneNumber = Prompt.Input<string>("Insira o telefone do cliente");
            var document = Prompt.Input<string>("Insira o CPF do cliente");

            bool success = controllerClient.CreateClient(name, phoneNumber, document);

            if (success)
                Console.WriteLine("\nCliente adicionado com sucesso!");
            else
                Console.WriteLine("\nNão foi possível adicionar o cliente");

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }
    }
}
