using AdaCredit.Employee;
using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AdaCredit.Client
{
    internal class ClientRepository
    {
        private static ClientRepository? instance;
        private List<ClientEntity> clients = new();
        private string pathDir = BuildPathDir(Environment.CurrentDirectory);
        private string pathFile = String.Empty;

        private ClientRepository()
        {
            if(!Directory.Exists(pathDir))
                Directory.CreateDirectory(pathDir);

            this.pathFile = pathDir + Path.DirectorySeparatorChar + "clients.csv";

            if (!File.Exists(pathFile))
            {
                Save();
            }
            else
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    IncludePrivateMembers = true,
                };

                using var reader = new StreamReader(pathFile);
                using var csv = new CsvReader(reader, config);
                clients = csv.GetRecords<ClientEntity>().ToList();
            }
        }

        public static ClientRepository GetInstance()
        {
            instance ??= new ClientRepository();
            return instance;
        }

        private static string BuildPathDir(string currentDirectory)
        {
            // TODO: rever esse método aqui -> Pode pegar o último bin
            var baseDir = currentDirectory.Split("bin")[0];
            return baseDir + "Client" + Path.DirectorySeparatorChar + "Resources";
        }

        internal void Save()
        {
            try
            {
                using var writer = new StreamWriter(pathFile);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(clients);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public List<ClientEntity> GetClients()
        {
            return this.clients;
        }

        internal bool AddClient(ClientEntity client)
        {
            try
            {
                this.clients.Add(client);
                Save();
                return true;
            } catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
