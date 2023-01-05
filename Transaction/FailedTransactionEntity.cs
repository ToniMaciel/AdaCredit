using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaCredit.Transaction
{
    internal class FailedTransactionEntity : TransactionEntity
    {
        private string failedTransactionDetail;
        public FailedTransactionEntity(TransactionEntity transaction) 
        {
            this.AccountNumberSource = transaction.AccountNumberSource;
            this.AccountNumberTarget = transaction.AccountNumberTarget;
            this.BankBranchSource = transaction.BankBranchSource;
            this.BankBranchTarget = transaction.BankBranchTarget;
            this.BankCodeSource = transaction.BankCodeSource;
            this.BankCodeTarget = transaction.BankCodeTarget;
            this.TransactionType = transaction.TransactionType;
            this.Value = transaction.Value;
            this.failedTransactionDetail = String.Empty;
        }

        public FailedTransactionEntity(TransactionEntity transaction, string failedTransactionDetail) : this(transaction)
        {
            this.failedTransactionDetail = failedTransactionDetail;
        }
    }
}
