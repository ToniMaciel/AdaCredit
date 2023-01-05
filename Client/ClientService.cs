

using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdaCredit.Client
{
    internal class ClientService
    {
        private ClientRepository clientRepository = ClientRepository.GetInstance();

        internal bool CreateClient(string name, string phoneNumber, string document, string username)
        {
            if(GetClients().Any(c => c.Document == document))
            {
                Console.Write($"\nO CPF {document} já existe.");
                return false;
            }
            
            var accountNumber = UniqueAccountNumber();
            var client = new ClientEntity(name, phoneNumber, document, accountNumber, username);
            return clientRepository.AddClient(client);
        }

        internal List<ClientEntity> GetClients()
        {
            return this.clientRepository.GetClients();
        }
        internal List<ClientEntity> GetClients(bool isActive)
        {
            return this.clientRepository.GetClients().Where(cli => cli.IsActive == isActive).ToList();
        }
        internal bool DisableClient(ClientEntity client, string username)
        {
            try
            {
                client.Disable(username);
                clientRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        internal bool UpdateClientPhone(ClientEntity userClient, string newPhoneNumber, string username)
        {
            try
            {
                userClient.UpdatePhone(newPhoneNumber, username);
                clientRepository.Save();
                return true;
            } catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private string UniqueAccountNumber()
        {
            bool unique = false;
            string accountNumber = string.Empty;

            while(!unique)
            {
                accountNumber = new Faker().Random.ReplaceNumbers("#####-#");
                unique = !clientRepository.GetClients().Any(cli => cli.AccountNumber == accountNumber);
            }

            return accountNumber;
        }

        internal ClientEntity GetClientByAccNumber(string accountNumber) 
            => GetClients().FirstOrDefault(cli => cli.AccountNumber == accountNumber);

        internal bool UpdateClientBalance(ClientEntity client, decimal value)
        {
            try
            {
                client.UpdateBalance(value);
                clientRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
