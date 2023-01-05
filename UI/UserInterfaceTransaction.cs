
using AdaCredit.Controllers;
using ConsoleTools;
using Sharprompt;
using System;

namespace AdaCredit.UI
{
    public static class UserInterfaceTransaction
    {
        public static void Run(string[] args, ControllerTransaction controllerTransaction, ControllerClient controllerClient)  
        {
            var transactionMenu = new ConsoleMenu(args, level: 1)
              .Add("Processar Transações (Reconciliação Bancária)", () => ProcessTransactions(controllerTransaction, controllerClient))
              .Add("Voltar", ConsoleMenu.Close)
              .Configure(config =>
              {
                  config.Selector = "--> ";
                  config.Title = "Transações";
                  config.EnableBreadcrumb = true;
                  config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
              });

            transactionMenu.Show();
        }
        private static void ProcessTransactions(ControllerTransaction controllerTransaction, ControllerClient controllerClient)
        {
            var answer = Prompt.Confirm("Deseja processar as transações agora?", defaultValue: true);
            if (answer)
            {
                bool success = controllerTransaction.ProcessTransactions(controllerClient);
                if (success)
                    Console.WriteLine($"\nTransações processadas com sucesso!");
                else
                    Console.WriteLine("\nNão foi possível processar as as transações.");
            } else
                Console.WriteLine("Processamento cancelado.");

            Console.WriteLine("<<Aperte qualquer tecla para continuar>>");
            Console.ReadKey();
        }
    }
}
