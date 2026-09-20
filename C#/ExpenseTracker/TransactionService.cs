using System.Text.Json;

namespace ExpenseTracker {
    /// <summary>
    /// Business logic layer for managing financial transactions.
    /// Handles adding, editing, deleting, reporting, and persistence of transactions.
    /// </summary>
    public class TransactionService {
        private List<Transaction> transactions = new List<Transaction>();
        private int nextId = 1;
        private static readonly string FilePath = "../../../transactions.json";

        /// <summary>
        /// Adds a new transaction to the collection.
        /// </summary>
        public void AddTransaction(DateTime date, decimal amount, TransactionType type, string category, string description) {
            Transaction transaction = new Transaction {
                Id = nextId++,
                Date = date,
                Amount = amount,
                Type = type,
                Category = category,
                Description = description
            };

            transactions.Add(transaction);
        }

        /// <summary>
        /// Prompts the user for a transaction date, defaulting to today if none provided.
        /// </summary>
        public DateTime GetTransactionDate() {
            Console.Write("Enter transaction date (MM-dd-yyyy) or press Enter for today: ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
                return DateTime.Now;

            if (DateTime.TryParse(input, out DateTime parsedDate))
                return parsedDate;

            Console.WriteLine("Invalid date format. Using today's date.");
            return DateTime.Now;
        }

        /// <summary>
        /// Removes a transaction by its ID.
        /// </summary>
        /// <returns>True if the transaction was found and removed.</returns>
        public bool RemoveTransaction(int id) {
            Transaction? transaction = transactions.FirstOrDefault(t => t.Id == id);
            if (transaction == null) {
                return false;
            }
            transactions.Remove(transaction);
            return true;
        }

        /// <summary>
        /// Returns all recorded transactions.
        /// </summary>
        public List<Transaction> GetAllTransactions() {
            return transactions;
        }

        /// <summary>
        /// Calculates the current balance (total Income minus total Expenses).
        /// </summary>
        public decimal GetBalance() {
            decimal income = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            decimal expense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            return income - expense;
        }

        /// <summary>
        /// Saves all transactions to a JSON file.
        /// </summary>
        public void SaveToFile() {
            try {
                JsonSerializerOptions options = new JsonSerializerOptions {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(transactions, options);
                File.WriteAllText(FilePath, json);
            } catch (Exception ex) {
                Console.WriteLine($"Warning: Could not save data. {ex.Message}");
            }
        }

        /// <summary>
        /// Loads transactions from the JSON file on application startup.
        /// </summary>
        public void LoadFile() {
            if (!File.Exists(FilePath))
                return;

            try {
                string json = File.ReadAllText(FilePath);
                List<Transaction>? loaded = JsonSerializer.Deserialize<List<Transaction>>(json);

                if (loaded != null) {
                    transactions = loaded;
                    if (transactions.Any()) {
                        nextId = transactions.Max(t => t.Id) + 1;
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Warning: Could not load data. {ex.Message}");
            }
        }
    }
}
