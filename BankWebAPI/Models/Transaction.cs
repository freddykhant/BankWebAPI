namespace BankWebAPI.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountNumber { get; set; }
        public TransactionType Type { get; set; }  // deposit/withdrawal
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; } 
        public Account Account { get; set; }
    }
}
