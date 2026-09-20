namespace ExpenseTrackerV2 {
    /// <summary>
    /// Represents a registered user account with their personal transactions.
    /// </summary>
    public class User {
        public int Id {
            get; set;
        }
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// List of transactions belonging to this user.
        /// </summary>
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
