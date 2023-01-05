using AdaCredit.Controllers;
using AdaCredit.Employee;
using ConsoleTools;
using Sharprompt;
using System;

namespace AdaCredit.UI
{
    public static class UserInterfaceEmployee
    {
        public static void Run(string[] args, ControllerEmployee controllerEmployee, EmployeeEntity userLogged)
        {
            var employeeMenu = new ConsoleMenu(args, level: 1)
              .Add("Cadastrar Novo Funcionário", () => CreateNewEmployee(controllerEmployee, userLogged))
              .Add("Alterar Senha de um Funcionário existente", () => ChangeEmployeePassword(controllerEmployee, userLogged))
              .Add("Desativar Cadastro de um Funcionário existente", () => DisableEmployee(controllerEmployee, userLogged))
              .Add("Voltar", ConsoleMenu.Close)
              .Configure(config =>
              {
                  config.WriteHeaderAction = () => Console.WriteLine("Escolha uma opção:");
                  config.Selector = "--> ";
                  config.Title = "Funcionário";
                  config.EnableBreadcrumb = false;
                  config.EnableWriteTitle = false;
              });

            employeeMenu.Show();
        }

        private static void DisableEmployee(ControllerEmployee controllerEmployee, EmployeeEntity userLogged)
        {
            var userEmployee = Prompt.Select("Selecione o funcionário", controllerEmployee.GetEmployees());
            var answer = Prompt.Confirm($"Tem certeza que deseja desativar o funcionário {userEmployee}?", defaultValue: true);
            
            if (answer)
            {
                bool success = controllerEmployee.DisableEmployee(userEmployee, userLogged.Username);
            
                if (success)
                {
                    Console.WriteLine($"\n{userEmployee} desativado com sucesso!");
                    if(userEmployee.Username == userLogged.Username)
                    {
                        Console.WriteLine("\nATENÇÂO: Seu usuário foi desativado.\n<<Aperte qualquer tecla para sair>>");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
                else
                    Console.WriteLine("\nNão foi possível desativar o cliente");

            }
            else Console.WriteLine("Operação cancelada.");

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }

        private static void ChangeEmployeePassword(ControllerEmployee controllerEmployee, EmployeeEntity userLogged)
        {
            var userEmployee = Prompt.Select("Selecione o funcionário", controllerEmployee.GetEmployees());
            var answer = Prompt.Confirm($"Tem certeza que deseja alterar a senha do funcionário {userEmployee}?", defaultValue: true);
            
            if(answer)
                UpdateNewPassword(userEmployee, controllerEmployee, userLogged);
            else 
                Console.WriteLine("Operação cancelada.");

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }

        internal static EmployeeEntity? CreateNewEmployee(ControllerEmployee controllerEmployee, EmployeeEntity userLogged)
        {
            string login = "user";
            while (login == "user")
            {
                login = Prompt.Input<string>("Insira o login do funcionário");

                if (login == "user")
                {
                    Console.Clear();
                    Console.WriteLine("\"user\" não é um nome válido para o usuário, por favor insira outro nome.");
                }

            }

            var name = Prompt.Input<string>("Insira o nome do funcionário");
            var document = Prompt.Input<string>("Insira o CPF do funcionário");

            var employee = controllerEmployee.CreateEmployee(login, name, document, (userLogged is null ? login : userLogged.Username));

            if (employee is not null)
                Console.WriteLine("\nFuncionário adicionado com sucesso!");
            else
                Console.WriteLine("\nNão foi possível adicionar o funcionário");

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();

            return employee;
        }

        internal static void UpdateNewPassword(EmployeeEntity employee, ControllerEmployee controllerEmployee, EmployeeEntity userLogged)
        {
            bool isEquals = false;
            string newPassword = String.Empty;
            while (!isEquals)
            {
                newPassword = Prompt.Password("Insira a nova senha");
                var confirmNewPassword = Prompt.Password("Confirme a senha");

                isEquals = (newPassword == confirmNewPassword);
                if (!isEquals)
                {
                    Console.Clear();
                    Console.WriteLine("As senhas estão diferentes, por favor informe novamente!");
                }
            }

            bool success = controllerEmployee.ChangeEmployeePassword(employee, newPassword, userLogged.Username);
            if (success)
                Console.WriteLine("\nSenha alterada com sucesso!");
            else
                Console.WriteLine("\nNão foi possível alterar senha.");

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }
    }
}
