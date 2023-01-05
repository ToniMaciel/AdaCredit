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
    }
}
