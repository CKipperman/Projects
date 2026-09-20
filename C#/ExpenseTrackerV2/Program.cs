/*
 * Project:     Expense Tracker V2
 * Author:      Chava Kipperman
 * 
 * Description: An improved console-based personal finance application with multi-user support.
 *              Users can create accounts, log in, track income and expenses, generate reports,
 *              and export data. All user data is persisted between sessions.
 * 
 * Key Features:
 *   • Multi-user system with account creation and login
 *   • Add, view, edit, and delete transactions
 *   • Real-time balance calculation
 *   • Category-based spending report
 *   • Monthly income/expense summary
 *   • CSV export for external analysis
 *   • Persistent JSON storage per user
 *   • Input validation and user-friendly interface
 * 
 * Technologies: C#, .NET, System.Text.Json, Console Application
 */
namespace ExpenseTrackerV2 {
    /// <summary>
    /// Main entry point for the Expense Tracker V2 console application.
    /// Manages user authentication, menu navigation, and coordinates with the service layer.
    /// </summary>
    internal class Program {
        private static TransactionService service = new TransactionService();

        static void Main(string[] args) {
            service.LoadFile();
            ShowLoginScreen();                    // Start with login

            bool running = true;

            while (running) {
                DisplayMenu();
                string? choice = Console.ReadLine()?.Trim();

                switch (choice) {
                    case "1":
                        AddTransaction();
                        break;
                    case "2":
                        ViewTransactions();
                        break;
                    case "3":
                        EditTransaction();
                        break;
                    case "4":
                        DeleteTransaction();
                        break;
                    case "5":
                        ViewBalance();
                        break;
                    case "6":
                        ShowCategoryReport();
                        break;
                    case "7":
                        ShowMonthlyReport();
                        break;
                    case "8":
                        ExportToCsv();
                        break;
                    case "9":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.\n");
                        break;
                }
            }

            service.SaveToFile();
            Console.WriteLine("\nThank you for using Expense Tracker! Goodbye!");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays the login / registration screen and authenticates the user.
        /// </summary>
        static void ShowLoginScreen() {
            Console.WriteLine("=== Welcome to Expense Tracker ===\n");

            while (true) {
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Create New User");
                Console.WriteLine("3. Exit");
                Console.Write("\nChoose an option: ");

                string? option = Console.ReadLine()?.Trim();

                if (option == "1") {
                    Console.Write("Enter username: ");
                    string? username = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(username) && service.Login(username))
                        break;
                } else if (option == "2") {
                    Console.Write("Choose a username: ");
                    string? username = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(username)) {
                        service.RegisterUser(username);
                        break;
                    }
                } else if (option == "3") {
                    Environment.Exit(0);
                } else {
                    Console.WriteLine("Invalid option.");
                }
            }
        }

        /// <summary>
        /// Displays the main menu with the currently logged-in username.
        /// </summary>
        public static void DisplayMenu() {
            string username = service.CurrentUser?.Username ?? "User";
            Console.WriteLine($"\n=== Expense Tracker - {username} ===");
            Console.WriteLine("1. Add Transaction");
            Console.WriteLine("2. View Transactions");
            Console.WriteLine("3. Edit Transaction");
            Console.WriteLine("4. Delete Transaction");
            Console.WriteLine("5. View Balance");
            Console.WriteLine("6. Category Report");
            Console.WriteLine("7. Monthly Report");
            Console.WriteLine("8. Export to CSV");
            Console.WriteLine("9. Exit");
            Console.Write("\nEnter your choice (1-9): ");
        }

        /// <summary>
        /// Guides the user to add a new income or expense transaction.
        /// </summary>
        public static void AddTransaction() {
            Console.WriteLine("\n--- Add New Transaction ---");

            DateTime transactionDate = service.GetTransactionDate();

            Console.Write("Enter Amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0) {
                Console.WriteLine("Invalid amount!");
                return;
            }

            TransactionType type;
            while (true) {
                Console.Write("Type (I)ncome or (E)xpense: ");
                string? typeInput = Console.ReadLine()?.Trim().ToUpper();

                if (typeInput == "I") {
                    type = TransactionType.Income;
                    break;
                } else if (typeInput == "E") {
                    type = TransactionType.Expense;
                    break;
                } else
                    Console.WriteLine("Please enter 'I' or 'E'.");
            }

            Console.Write("Category: ");
            string? category = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(category))
                category = "General";

            Console.Write("Description: ");
            string? description = Console.ReadLine()?.Trim();

            service.AddTransaction(transactionDate, amount, type, category, description ?? "");
            Console.WriteLine("Transaction added successfully!\n");
        }

        /// <summary>
        /// Displays all transactions for the current user.
        /// </summary>
        public static void ViewTransactions() {
            List<Transaction> transactions = service.GetAllTransactions();

            if (transactions.Count == 0) {
                Console.WriteLine("\nNo transactions found.");
                return;
            }

            Console.WriteLine("\nID | Date       | Amount     | Type     | Category     | Description");
            Console.WriteLine("----------------------------------------------------------------------");
            foreach (Transaction t in transactions) {
                Console.WriteLine($"{t.Id,-2} | {t}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Allows editing of an existing transaction.
        /// </summary>
        public static void EditTransaction() {
            ViewTransactions();
            List<Transaction> transactions = service.GetAllTransactions();

            if (transactions.Count == 0) {
                return;
            }

            Console.Write("\nEnter ID to edit: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out int id)) {
                Console.WriteLine("Invalid ID!");
                return;
            }

            Transaction? transaction = transactions.FirstOrDefault(t => t.Id == id);
            if (transaction == null) {
                Console.WriteLine("Transaction not found.");
                return;
            }

            Console.WriteLine($"\nEditing Transaction #{id}");
            Console.WriteLine("Press Enter to keep current value.\n");

            // Edit Date
            Console.Write($"New Date (current: {transaction.Date:yyyy-MM-dd}) (MM-dd-yyyy): ");
            string? dateInput = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(dateInput) && DateTime.TryParse(dateInput, out DateTime newDate)) {
                transaction.Date = newDate;
            }

            // Edit Amount
            Console.Write($"New Amount (current: {transaction.Amount:C}): ");
            string? amountInput = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(amountInput) && decimal.TryParse(amountInput, out decimal newAmount) && newAmount > 0) {
                transaction.Amount = newAmount;
            }

            // Edit Type (Income / Expense)
            Console.Write($"New Type (current: {transaction.Type}) (I/E): ");
            string? typeInput = Console.ReadLine()?.Trim().ToUpper();
            if (typeInput == "I")
                transaction.Type = TransactionType.Income;
            else if (typeInput == "E")
                transaction.Type = TransactionType.Expense;

            // Edit Category
            Console.Write($"New Category (current: {transaction.Category}): ");
            string? newCategory = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(newCategory))
                transaction.Category = newCategory;

            // Edit Description
            Console.Write($"New Description (current: {transaction.Description}) to clear write \"clear\": ");
            string? newDescription = Console.ReadLine()?.Trim();
            if (newDescription?.ToLower() == "clear")
                transaction.Description = string.Empty;
            else if (!string.IsNullOrEmpty(newDescription))
                transaction.Description = newDescription;

            Console.WriteLine($"Transaction #{id} updated successfully!\n");
            service.SaveToFile();
        }

        /// <summary>
        /// Deletes a transaction by ID.
        /// </summary>
        public static void DeleteTransaction() {
            ViewTransactions();
            if (service.GetAllTransactions().Count == 0) {
                return;
            }

            Console.Write("Enter ID to delete: ");
            if (int.TryParse(Console.ReadLine()?.Trim(), out int id)) {
                if (service.RemoveTransaction(id)) {
                    Console.WriteLine($"Transaction #{id} deleted successfully.");
                    service.SaveToFile();
                } else {
                    Console.WriteLine("Transaction not found.");
                }
            } else {
                Console.WriteLine("Invalid ID.");
            }

        }

        /// <summary>
        /// Shows the current balance (Income - Expenses) for the logged-in user.
        /// </summary>
        public static void ViewBalance() {
            decimal balance = service.GetBalance();
            string sign = balance >= 0 ? "" : "-";
            Console.WriteLine($"\nBalance: {sign}{balance:C}");
        }

        /// <summary>
        /// Generates a spending report grouped by category.
        /// </summary>
        public static void ShowCategoryReport() {
            List<Transaction> transactions = service.GetAllTransactions();

            if (transactions.Count == 0) {
                Console.WriteLine("\nNo transactions found.");
                return;
            }

            // Group by Category
            var categoryGroups = transactions
                .GroupBy(t => t.Category)
                .OrderByDescending(g => g.Sum(t => t.Type == TransactionType.Expense ? t.Amount : 0));

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("                 CATEGORY SPENDING REPORT");
            Console.WriteLine(new string('=', 60));

            decimal totalExpenses = 0;

            foreach (var group in categoryGroups) {
                decimal income = group.Where(t => t.Type == TransactionType.Income)
                                      .Sum(t => t.Amount);
                decimal expense = group.Where(t => t.Type == TransactionType.Expense)
                                       .Sum(t => t.Amount);

                totalExpenses += expense;

                Console.WriteLine($"\n{group.Key,-15} ");
                if (income > 0)
                    Console.WriteLine($"    Income     : {income,12:C}");
                if (expense > 0)
                    Console.WriteLine($"    Expenses   : {expense,12:C}");
            }

            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"Total Expenses : {totalExpenses,12:C}\n");
        }

        /// <summary>
        /// Generates a monthly summary of income and expenses.
        /// </summary>
        public static void ShowMonthlyReport() {
            List<Transaction> transactions = service.GetAllTransactions();
            if (transactions.Count == 0) {
                Console.WriteLine("\nNo transactions found.");
                return;
            }

            var groups = transactions.GroupBy(t => new { t.Date.Year, t.Date.Month })
                                     .OrderByDescending(g => g.Key.Year)
                                     .OrderByDescending(g => g.Key.Month);

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("                    MONTHLY REPORT");
            Console.WriteLine(new string('=', 60));

            foreach (var group in groups) {
                int year = group.Key.Year;
                int month = group.Key.Month;
                string monthName = new DateTime(year, month, 1).ToString("MMMM yyyy");

                decimal income = 0;
                decimal expense = 0;

                foreach (Transaction t in group) {
                    if (t.Type == TransactionType.Income)
                        income += t.Amount;
                    else
                        expense += t.Amount;
                }

                decimal balance = income - expense;
                string sign = balance >= 0 ? "" : "-";

                Console.WriteLine($"\n{monthName}");
                Console.WriteLine($"Income     : {income,12:C}");
                Console.WriteLine($"Expenses   : {expense,12:C}");
                Console.WriteLine($"Balance    : {sign}{balance,12:C}");
                Console.WriteLine(new string('-', 40));
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Exports the current user's transactions to a CSV file.
        /// </summary>
        public static void ExportToCsv() {
            List<Transaction> transactions = service.GetAllTransactions();

            if (transactions.Count == 0) {
                Console.WriteLine("\nNo transactions to export.");
                return;
            }

            string fileName = $"Expense_Export_{DateTime.Now:yyyy-MM-dd_HH-mm}.csv";
            string filePath = $"../../../{fileName}";

            try {
                using (StreamWriter writer = new StreamWriter(filePath)) {
                    // Write header
                    writer.WriteLine("ID,Date,Amount,Type,Category,Description");

                    // Write data
                    foreach (Transaction t in transactions) {
                        string description = t.Description.Replace(",", " "); // Avoid breaking CSV
                        writer.WriteLine($"{t.Id},{t.Date:yyyy-MM-dd},{t.Amount},{t.Type},{t.Category},{description}");
                    }
                }

                Console.WriteLine($"\nSuccessfully exported {transactions.Count} transactions!");
                Console.WriteLine($"File saved as: {fileName}");
                Console.WriteLine("You can open this file with Excel or Google Sheets.\n");
            } catch (Exception ex) {
                Console.WriteLine($"Error exporting to CSV: {ex.Message}");
            }
        }
    }
}
