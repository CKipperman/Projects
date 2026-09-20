using System.Text.Json;

namespace ExpenseTrackerV2 {
    /// <summary>
    /// Core service layer handling user management, transactions, and data persistence.
    /// Supports multiple independent user accounts.
    /// </summary>
    public class TransactionService {
        private List<User> users = new List<User>();
        private User? currentUser = null;
        private int nextUserId = 1;
        private static readonly string FilePath = "../../../users.json";

        public User? CurrentUser => currentUser;

        // ====================== USER MANAGEMENT ======================

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        public void RegisterUser(string username) {
            if (string.IsNullOrWhiteSpace(username)) {
                Console.WriteLine("Username cannot be empty.");
                return;
            }

            if (users.Any(u => u.Username.ToLower() == username.ToLower())) {
                Console.WriteLine("Username already exists!");
                return;
            }

            User newUser = new User {
                Id = nextUserId++,
                Username = username.Trim()
            };

            users.Add(newUser);
            currentUser = newUser;
            Console.WriteLine($"Account created! Welcome, {username}!");
        }

        /// <summary>
        /// Logs in an existing user by username.
        /// </summary>
        public bool Login(string username) {
            User? user = users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());

            if (user != null) {
                currentUser = user;
                Console.WriteLine($"Welcome back, {username}!");
                return true;
            }

            Console.WriteLine("User not found.");
            return false;
        }

        // ====================== TRANSACTION MANAGEMENT ======================

        /// <summary>
        /// Adds a new transaction for the currently logged-in user.
        /// </summary>
        public void AddTransaction(DateTime date, decimal amount, TransactionType type, string category, string description) {
            if (currentUser == null)
                return;

            int newId = currentUser.Transactions.Count > 0
                ? currentUser.Transactions.Max(t => t.Id) + 1
                : 1;

            Transaction transaction = new Transaction {
                Id = newId,
                Date = date,
                Amount = amount,
                Type = type,
                Category = category,
                Description = description
            };

            currentUser.Transactions.Add(transaction);
        }

        public List<Transaction> GetAllTransactions() {
            return currentUser?.Transactions ?? new List<Transaction>();
        }

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

        public bool RemoveTransaction(int id) {
            if (currentUser == null)
                return false;
            Transaction? transaction = currentUser.Transactions.FirstOrDefault(t => t.Id == id);
            if (transaction == null)
                return false;

            currentUser.Transactions.Remove(transaction);
            return true;
        }

        public decimal GetBalance() {
            if (currentUser == null)
                return 0;

            decimal income = currentUser.Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            decimal expense = currentUser.Transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            return income - expense;
        }

        // ====================== PERSISTENCE ======================

        /// <summary>
        /// Saves all users and their transactions to JSON.
        /// </summary>
        public void SaveToFile() {
            try {
                JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(users, options);
                File.WriteAllText(FilePath, json);
            } catch (Exception ex) {
                Console.WriteLine($"Warning: Could not save data. {ex.Message}");
            }
        }

        /// <summary>
        /// Loads users and transactions from the JSON file.
        /// </summary>
        public void LoadFile() {
            if (!File.Exists(FilePath))
                return;

            try {
                string json = File.ReadAllText(FilePath);
                List<User>? loadedUsers = JsonSerializer.Deserialize<List<User>>(json);

                if (loadedUsers != null) {
                    users = loadedUsers;
                    if (users.Count > 0) {
                        nextUserId = users.Max(u => u.Id) + 1;
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Warning: Could not load data. {ex.Message}");
            }
        }
    }
}
