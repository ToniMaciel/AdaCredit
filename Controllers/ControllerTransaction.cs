using AdaCredit.Transaction;
using System;

namespace AdaCredit.Controllers
{
    public class ControllerTransaction
    {
        private TransactionService transactionService = new();
        internal bool ProcessTransactions(ControllerClient controllerClient) => transactionService.ProcessTransactions(controllerClient);
    }
}
