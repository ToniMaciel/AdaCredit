using AdaCredit.Controllers;
using AdaCredit.Employee;
using ConsoleTools;
using Sharprompt;
using System;

namespace AdaCredit.UI
{
    public static class UserInterfaceEmployee
    {
        public static void Run(string[] args, ControllerEmployee controllerEmployee, string username)
        {
            var employeeMenu = new ConsoleMenu(args, level: 1)
              .Add("Cadastrar Novo Funcionário", () => CreateNewEmployee(controllerEmployee))
              .Add("Alterar Senha de um Funcionário existente", () => ChangeEmployeePassword(facade))
              .Add("Desativar Cadastro de um Funcionário existente", () => DisableEmployee(facade))
              .Add("Voltar", ConsoleMenu.Close)
              .Configure(config =>
              {
                  config.Selector = "--> ";
                  config.Title = "Funcionário";
                  config.EnableBreadcrumb = true;
                  config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
              });

            employeeMenu.Show();
        }

        private static void DisableEmployee(Facade facade)
        {
            var userEmployee = Prompt.Select("Selecione o funcionário", facade.GetEmployeesUsers());
            // TODO: colocar mensagem de confirmação
            bool success = facade.DisableEmployee(userEmployee);
            // TODO: printar mensagem de sucesso
        }

        private static void ChangeEmployeePassword(Facade facade)
        {
            // TODO: pode passar os funcionários logo, só precisa fazer um toString
            var userEmployee = Prompt.Select("Selecione o funcionário", facade.GetEmployeesUsers());
            // TODO: verificar senha
            var newPassword = Prompt.Password("Insira a nova senha");
            // TODO: colocar quem é que tá mudando
            bool success = facade.ChangeEmployeePassword(userEmployee, newPassword);
            // TODO: printar mensagem de sucesso
        }

        internal static EmployeeEntity? CreateNewEmployee(ControllerEmployee controllerEmployee)
        {
            // TODO: checar login != user
            var login = Prompt.Input<string>("Insira o login do funcionário");
            var name = Prompt.Input<string>("Insira o nome do funcionário");
            var document = Prompt.Input<string>("Insira o CPF do funcionário");

            var employee = controllerEmployee.CreateEmployee(login, name, document);

            if (employee is not null)
                Console.WriteLine("\nFuncionário adicionado com sucesso!");
            else
                Console.WriteLine("\nNão foi possível adicionar o funcionário");

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();

            return employee;
        }

        internal static void UpdateNewPassword(EmployeeEntity employee, ControllerEmployee controllerEmployee)
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
            // TODO: colocar quem é que tá mudando
            bool success = controllerEmployee.ChangeEmployeePassword(employee, newPassword);
            // TODO: printar mensagem de sucesso
        }
    }
}
