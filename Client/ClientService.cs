

using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdaCredit.Client
{
    internal class ClientService
    {
        private ClientRepository clientRepository = ClientRepository.GetInstance();

        internal bool CreateClient(string name, string phoneNumber, string document)
        {
            // TODO: deixar consistente com os outros métodos
            var accountNumber = UniqueAccountNumber();
            var client = new ClientEntity(name, phoneNumber, document, accountNumber);
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
        internal bool DisableClient(ClientEntity client)
        {
            try
            {
                client.Disable();
                clientRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                // TODO: mudar print da mensagem
                Console.WriteLine(ex);
                return false;
            }
        }
        internal bool UpdateClientPhone(ClientEntity userClient, string newPhoneNumber)
        {
            try
            {
                userClient.UpdatePhone(newPhoneNumber);
                clientRepository.Save();
                return true;
            } catch (Exception ex) 
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private string UniqueAccountNumber()
        {
            bool unique = false;
            string accountNumber = string.Empty;
            Faker faker = new();

            while(!unique)
            {
                accountNumber = faker.Random.ReplaceNumbers("#####-#");
                unique = !clientRepository.GetClients().Select(cli => cli.AccountNumber == accountNumber).Any();
            }

            return accountNumber;
        }

        // TODO: olhar isso aqui
        internal ClientEntity GetClientByAccNumber(string accountNumber) => GetClients().FirstOrDefault(cli => cli.AccountNumber == accountNumber);

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
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
