﻿using AdaCredit.Client;
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
        internal bool CreateClient(string name, string phoneNumber, string document) => clientService.CreateClient(name, phoneNumber, document);

        internal bool DisableClient(ClientEntity client) => clientService.DisableClient(client);

        internal List<ClientEntity> GetClients() => clientService.GetClients();

        internal bool UpdateClientPhone(ClientEntity userClient, string newPhoneNumber) => clientService.UpdateClientPhone(userClient, newPhoneNumber);
    }
}