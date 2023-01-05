using AdaCredit.Controllers;
using AdaCredit.Transaction;
using Bogus;
using System.Collections.Generic;
using System.Linq;

namespace AdaCredit.Utils
{
    internal class SampleDataGenerator
    {
        internal static void RunGeneration(Facade _facade)
        {
            // Generating 5 employees
            _facade.GetEmployeeControler().GetEmployees().Clear();
            _facade.GetEmployeeControler().CreateEmployee("paulo", new Faker().Person.FullName, new Faker().Random.ReplaceNumbers("###########"), "paulo");
            for (int i = 0; i < 4; i++)
                _facade.GetEmployeeControler().CreateEmployee(new Faker().Person.FirstName, new Faker().Person.FullName, new Faker().Random.ReplaceNumbers("###########"), "paulo");
            
            // Generating 10 clients
            _facade.GetClientController().GetClients().Clear();
            for (int i = 0; i < 10; i++)
                _facade.GetClientController().CreateClient(new Faker().Person.FullName, new Faker().Random.ReplaceNumbers("#########"), new Faker().Random.ReplaceNumbers("###########"), "paulo");

            // Disable 2 random clients
             _facade.GetClientController().DisableClient(_facade.GetClientController().GetClients()[new Faker().Random.Number(9)], "paulo");
             _facade.GetClientController().DisableClient(_facade.GetClientController().GetClients()[new Faker().Random.Number(9)], "paulo");

            // Generating 3 valid deposit operations (TED or DOC)
            // and generating 3 valid deposit/credit operations(TEF)
            // and generating 3 valid credit operations (TED or DOC)
            List<TransactionEntity> transactions = new();
            
            var transectionsType = new[] { "TED", "DOC" };
            var activeClients = _facade.GetClientController().GetClients().Where(cli => cli.IsActive).ToList();
            for (int i = 0; i < 4; i++)
            {
                var client = activeClients[new Faker().Random.Number(7)];
                var tt = transectionsType[new Faker().Random.Number(1)];
                var secondClient = activeClients[new Faker().Random.Number(7)];
                while(client.AccountNumber == secondClient.AccountNumber)
                    secondClient = activeClients[new Faker().Random.Number(7)];
                transactions.Add(new TransactionEntity(new Faker().Random.ReplaceNumbers("###"), new Faker().Random.ReplaceNumbers("####"), new Faker().Random.ReplaceNumbers("######"), "777", "0001", client.AccountNumber.Replace("-",""), tt, new Faker().Finance.Amount(1, 1000)));
                transactions.Add(new TransactionEntity("777", "0001", client.AccountNumber.Replace("-",""), "777", "0001", secondClient.AccountNumber.Replace("-",""), tt, new Faker().Finance.Amount(1, 20)));
                transactions.Add(new TransactionEntity("777", "0001", client.AccountNumber.Replace("-", ""), new Faker().Random.ReplaceNumbers("###"), new Faker().Random.ReplaceNumbers("####"), new Faker().Random.ReplaceNumbers("######"), tt, new Faker().Finance.Amount(1, 30)));
            }

            // Generating 1 invalid operation of insufficient balance
            transactions.Add(new TransactionEntity("777", "0001", activeClients[new Faker().Random.Number(7)].AccountNumber.Replace("-",""), new Faker().Random.ReplaceNumbers("###"), new Faker().Random.ReplaceNumbers("####"), new Faker().Random.ReplaceNumbers("######"), transectionsType[new Faker().Random.Number(1)], 10000.0m));

            // Generating 1 invalid operation of inexistent account
            transactions.Add(new TransactionEntity("777", "0001", new Faker().Random.ReplaceNumbers("######"), "777", "0001", new Faker().Random.ReplaceNumbers("######"), transectionsType[new Faker().Random.Number(1)], 10.0m));

            // Generating 1 invalid operation of deactivated account
            var deactivatedClient = _facade.GetClientController().GetClients().First(cli => !cli.IsActive);
            transactions.Add(new TransactionEntity("777", "0001", deactivatedClient.AccountNumber.Replace("-",""), "777", "0001", new Faker().Random.ReplaceNumbers("######"), transectionsType[new Faker().Random.Number(1)], 10.0m));

            TransactionService.WriteData("bank-fake", transactions);
        }
    }
}
