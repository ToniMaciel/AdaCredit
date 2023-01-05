using CsvHelper;
using System;

namespace AdaCredit.Client
{
    internal class ClientEntity
    {
        // TODO: Criar uma classe conta que terá essas infos
        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Document { get; private set; }
        public string AccountNumber { get; private set; }
        public string BankBranch { get; private set; }
        public decimal AccountBalance { get; private set; }
        public bool IsActive { get; private set; }

        public ClientEntity() { }

        public ClientEntity(string name, string phoneNumber, string document, string accountNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Document = document;
            AccountNumber = accountNumber;
            BankBranch = "0001";
            AccountBalance = 0.0m;
            IsActive = true;
        }

        internal void UpdatePhone(string newPhoneNumber)
        {
            this.PhoneNumber = newPhoneNumber;
        }

        internal void Disable()
        {
            this.IsActive = false;
        }
        
        internal void UpdateBalance (decimal value)
        {
            this.AccountBalance += value;
        }

        public override string ToString()
        {
            return $"{this.Name}({this.AccountNumber})";
        }

        public string ShowInfos()
        {
            return
                $"Nome: {this.Name}\n" +
                $"Telefone: {this.PhoneNumber}\n" +
                $"CPF: {this.Document}\n" +
                $"Número de conta: {this.AccountNumber}\n" +
                $"Saldo: {this.AccountBalance,0:F2}\n" +
                $"Ativo: {(this.IsActive ? "Sim" : "Não")}";
        }
    }
}
