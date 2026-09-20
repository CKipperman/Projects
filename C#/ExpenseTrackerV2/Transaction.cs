namespace ExpenseTrackerV2 {
    /// <summary>
    /// Defines whether a transaction is income or expense.
    /// </summary>
    public enum TransactionType {
        Income,
        Expense
    }

    /// <summary>
    /// Represents a single financial transaction belonging to a user.
    /// </summary
    public class Transaction {
        public int Id {
            get; set;
        }
        public DateTime Date { get; set; } = DateTime.Now;

        public decimal Amount {
            get; set;
        } = 0;
        public TransactionType Type {
            get; set;
        }
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Returns a formatted string for displaying the transaction.
        /// </summary>
        public override string? ToString() {
            string sign = Type == TransactionType.Income ? "+" : "-";
            return $"{Date:yyyy-MM-dd} | {sign}{Amount,9:C} | {Type,-8} | {Category,-12} | {Description}";
        }
    }
}
