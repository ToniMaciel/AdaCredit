using AdaCredit.Transaction;
using System;
using System.Collections.Generic;

namespace AdaCredit.Controllers
{
    public class ControllerTransaction
    {
        private TransactionService transactionService = new();

        internal List<FailedTransactionEntity> GetAllFailedTransactions() => TransactionService.GetAllFailedTransactions();

        internal bool ProcessTransactions(ControllerClient controllerClient) => transactionService.ProcessTransactions(controllerClient);
    }
}
