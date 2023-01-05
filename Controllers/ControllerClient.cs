using AdaCredit.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Controllers
{
    public class ControllerClient
    {
        private ClientService clientService;
        public ControllerClient()
        {
            this.clientService = new ClientService();
        }
        internal bool CreateClient(string name, string phoneNumber, string document, string username) 
            => clientService.CreateClient(name, phoneNumber, document, username);
        internal bool DisableClient(ClientEntity client, string username) 
            => clientService.DisableClient(client, username);
        internal ClientEntity GetClient(string accountNumber) 
            => clientService.GetClientByAccNumber(accountNumber);
        internal List<ClientEntity> GetClients() 
            => clientService.GetClients();
        internal List<ClientEntity> GetClients(bool isActive) 
            => clientService.GetClients(isActive);
        internal void UpdateClientBalance(ClientEntity client, decimal value) 
            => clientService.UpdateClientBalance(client, value);
        internal bool UpdateClientPhone(ClientEntity userClient, string newPhoneNumber, string username) 
            => clientService.UpdateClientPhone(userClient, newPhoneNumber, username);
    }
}
