using AdaCredit.Transaction;
using System;
using System.Collections.Generic;

namespace AdaCredit.Controllers
{
    public class ControllerTransaction
    {
        internal List<FailedTransactionEntity> GetAllFailedTransactions() 
            => TransactionService.GetAllFailedTransactions();

        internal bool ProcessTransactions(ControllerClient controllerClient) 
            => TransactionService.ProcessTransactions(controllerClient);
    }
}
