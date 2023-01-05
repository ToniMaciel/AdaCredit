namespace AdaCredit.Transaction
{
    internal class TransactionEntity
    {
        public string BankCodeSource { get; protected set; }
        public string BankBranchSource { get; protected set; }
        public string AccountNumberSource { get; protected set; }
        public string BankCodeTarget { get; protected set; }
        public string BankBranchTarget { get; protected set; }
        public string AccountNumberTarget { get; protected set; }
        public string TransactionType { get; protected set; }
        public decimal Value { get; protected set; }
        public TransactionEntity() { }

        public TransactionEntity(string bankCodeSource, string bankBranchSource, string accountNumberSource, string bankCodeTarget, string bankBranchTarget, string accountNumberTarget, string type, decimal value)
        {
            BankCodeSource = bankCodeSource;
            BankBranchSource = bankBranchSource;
            AccountNumberSource = accountNumberSource;
            BankCodeTarget = bankCodeTarget;
            BankBranchTarget = bankBranchTarget;
            AccountNumberTarget = accountNumberTarget;
            TransactionType = type;
            Value = value;
        }
    }
}
