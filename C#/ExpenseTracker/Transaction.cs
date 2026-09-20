namespace ExpenseTracker {
    /// <summary>
    /// Defines the type of financial transaction.
    /// </summary>
    public enum TransactionType {
        Income,
        Expense
    }

    /// <summary>
    /// Represents a single financial transaction (income or expense).
    /// </summary>
    public class Transaction {
        public int Id {
            get; set;
        }

        /// <summary>
        /// Date when the transaction occurred.
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;

        /// <summary>
        /// Monetary amount of the transaction.
        /// </summary>
        public decimal Amount {
            get; set;
        } = 0;

        /// <summary>
        /// Whether this is Income or Expense.
        /// </summary>
        public TransactionType Type {
            get; set;
        }

        /// <summary>
        /// Category of the transaction (e.g., Food, Salary, Transport).
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Optional additional notes about the transaction.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Returns a formatted string representation of the transaction for display.
        /// </summary>
        public override string? ToString() {
            string sign = Type == TransactionType.Income ? "+" : "-";
            return $"{Date:yyyy-MM-dd} | {sign}{Amount,9:C} | {Type,-8} | {Category,-12} | {Description}";
        }
    }
}
