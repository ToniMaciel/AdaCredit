﻿using System.IO;
using System;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using AdaCredit.Client;
using System.Linq;
using AdaCredit.Controllers;
using System.Collections.Generic;

namespace AdaCredit.Transaction
{
    internal class TransactionService
    {
        private const string adaCreditCode = "777";
        private static readonly string transactionsDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar + "Transactions";
        private static readonly DateTime dateWithoutTaxes = new(2022, 11, 30);
        internal bool ProcessTransactions(ControllerClient controllerClient)
        {
            try
            {
                string pendingTransactionsPath = transactionsDir + Path.DirectorySeparatorChar + "Pending";
                if (Directory.Exists(pendingTransactionsPath))
                {
                    var filesToProcess = Directory.GetFiles(pendingTransactionsPath);
                    foreach ( var file in filesToProcess )
                    {
                        int prefixPosition = file.LastIndexOf(Path.DirectorySeparatorChar);
                        int position = file.LastIndexOf("-");
                        int finalDot = file.LastIndexOf(".");
                        
                        var bankName = file[(prefixPosition + 1)..position];
                        var date = file[(position + 1)..(finalDot-1)];

                        ProcessTransaction(file, bankName, date, controllerClient);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        private static void ProcessTransaction(string file, string bankName, string date, ControllerClient controllerClient)
        {
            DateTime transactionDate = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
            
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                IncludePrivateMembers = true,
                HasHeaderRecord = false,
            };

            using var reader = new StreamReader(file);
            using var csv = new CsvReader(reader, config);
            var trasanctions = csv.GetRecords<TransactionEntity>().ToList();

            List<TransactionEntity> validTransections = new();
            List<FailedTransactionEntity> failedTransactions = new();

            // TODO: refact

            foreach(var transaction in trasanctions)
            {
                bool validOperation = true;
                string failedTransactionDetail = String.Empty;
                ClientEntity source = null, target = null;

                if (transaction.BankCodeSource != transaction.BankCodeTarget && transaction.TransactionType == "TEF")
                {
                    validOperation = false;
                    failedTransactionDetail = "Transação incompatível (TEF entre diferentes bancos)";
                }

                if(validOperation && transaction.BankCodeSource == adaCreditCode)
                {
                    source = controllerClient.GetClient(transaction.AccountNumberSource);

                    if(source is null)
                    {
                        validOperation = false;
                        failedTransactionDetail = "Número de conta inexistente";
                    } 
                    
                    else if (!source.IsActive)
                    {
                        validOperation = false;
                        failedTransactionDetail = "Número de conta desativado";
                    } 
                    
                    else if (source.AccountBalance < transaction.Value)
                    {
                        validOperation = false;
                        failedTransactionDetail = "Saldo insuficiente";
                    }
                }

                if (validOperation && transaction.BankCodeTarget == adaCreditCode)
                {
                    target = controllerClient.GetClient(transaction.AccountNumberTarget);

                    if (target is null)
                    {
                        validOperation = false;
                        failedTransactionDetail = "Número de conta inexistente";
                    }

                    else if (!target.IsActive)
                    {
                        validOperation = false;
                        failedTransactionDetail = "Número de conta desativado";
                    }

                    else if (target.AccountBalance < transaction.Value)
                    {
                        validOperation = false;
                        failedTransactionDetail = "Saldo insuficiente";
                    }
                }

                if (validOperation)
                {
                    decimal value = CalculateTotalValue(transactionDate, transaction.Value, transaction.TransactionType);
                    if (source is not null)
                        controllerClient.UpdateClientBalance(source, -value);
                    if (target is not null)
                        controllerClient.UpdateClientBalance(target, transaction.Value);
                    validTransections.Add(transaction);
                    continue;
                }

                failedTransactions.Add(new FailedTransactionEntity(transaction, failedTransactionDetail));
            }

            WriteSuccessTransaction(bankName, date, validTransections);
            WriteFailedTransaction(bankName, date, failedTransactions);
        }

        private static void WriteFailedTransaction(string bankName, string date, List<FailedTransactionEntity> failedTransactions)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            string failedTransactionPath = transactionsDir + Path.DirectorySeparatorChar + "Failed";

            if (!Directory.Exists(failedTransactionPath))
                Directory.CreateDirectory(failedTransactionPath);

            try
            {
                using var writer = new StreamWriter(failedTransactionPath + Path.DirectorySeparatorChar + bankName + "-" + date + "-failed.csv");
                using var csv = new CsvWriter(writer, config);
                csv.WriteRecords(failedTransactions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void WriteSuccessTransaction(string bankName, string date, List<TransactionEntity> validTransections)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            string successTransactionPath = transactionsDir + Path.DirectorySeparatorChar + "Completed";

            if(!Directory.Exists(successTransactionPath))
                Directory.CreateDirectory(successTransactionPath);

            try
            {
                using var writer = new StreamWriter(successTransactionPath + Path.DirectorySeparatorChar + bankName + "-" + date + "-completed.csv");
                using var csv = new CsvWriter(writer, config);
                csv.WriteRecords(validTransections);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static decimal CalculateTotalValue(DateTime date, decimal value, string transactionType)
        {
            if(date <= dateWithoutTaxes)
                return value;

            if (transactionType == "TED")
                return value + 5.00m;
            else if (transactionType == "DOC")
                return value + 1.00m + Math.Min(5.00m, value * 0.01m);
            
            return value;
        }

        internal static List<FailedTransactionEntity> GetAllFailedTransactions()
        {
            string failedTransactionPath = transactionsDir + Path.DirectorySeparatorChar + "Failed";
            List<FailedTransactionEntity> allFailedTransactions = new();
            
            if (Directory.Exists(failedTransactionPath))
            {
                var filesToProcess = Directory.GetFiles(failedTransactionPath);
                foreach (var file in filesToProcess)
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        IncludePrivateMembers = true,
                    };

                    using var reader = new StreamReader(file);
                    using var csv = new CsvReader(reader, config);
                    var transactions = csv.GetRecords<FailedTransactionEntity>().ToList();

                    allFailedTransactions.AddRange(transactions);
                }
            }

            return allFailedTransactions;
        }
    }
}