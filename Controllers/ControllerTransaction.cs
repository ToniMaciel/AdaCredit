using AdaCredit.Transaction;
using System;
using System.Collections.Generic;

namespace AdaCredit.Controllers
{
    public class ControllerTransaction
    {
        public ControllerTransaction() 
        {
            TransactionService.PutDirInDesktop();
        }
        internal List<FailedTransactionEntity> GetAllFailedTransactions() 
            => TransactionService.GetAllFailedTransactions();

        internal bool ProcessTransactions(ControllerClient controllerClient) 
            => TransactionService.ProcessTransactions(controllerClient);
    }
}
