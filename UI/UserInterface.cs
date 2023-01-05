using AdaCredit.Controllers;
using AdaCredit.Employee;
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

            Console.WriteLine("\nLogin feito com sucesso!\n<<Pressione qualquer tecla para continuar>>");
            Console.ReadKey();
            Console.Clear();

            EmployeeEntity? employee = _facade.GetEmployeeControler().GetEmployee(username);

            if(employee is null)
            {
                Console.WriteLine("Será necessário adicionar um funcionário para continuar.");
                employee = UserInterfaceEmployee.CreateNewEmployee(_facade.GetEmployeeControler());
            }
            else if(employee.IsFirstLogin)
            {
                Console.WriteLine("Será necessário mudar sua senha:");
                UserInterfaceEmployee.UpdateNewPassword(employee, _facade.GetEmployeeControler());
            }

            _facade.GetEmployeeControler().UpdateLogin(employee, DateTime.Now);
            Run(args, employee);
        }
        public static void Run(string[] args, EmployeeEntity loginUser)
        {
            var mainMenu = new ConsoleMenu(args, level: 0)
              .Add("Clientes", () => UserInterfaceClient.Run(args, _facade.GetClientController(), loginUser))
              .Add("Funcionários", () => UserInterfaceEmployee.Run(args, _facade.GetEmployeeControler(), loginUser))
              .Add("Transações", () => UserInterfaceTransaction.Run(args, _facade.GetTransactionController(), _facade.GetClientController()))
              .Add("Relatórios", () => UserInterfaceReport.Run(args, _facade))
              .Add("Fechar", ConsoleMenu.Close)
              .Configure(config =>
              {
                  config.WriteHeaderAction = () => Console.WriteLine("Escolha uma opção:");
                  config.Selector = "--> ";
                  config.Title = "Menu principal";
                  config.EnableWriteTitle = false;
                  config.EnableBreadcrumb = false;
              });

            mainMenu.Show();
        }
    }
}
