/*
 * Project:     Library Management System
 * Author:      Chava Kipperman
 * Description: A comprehensive console-based Library Management System built in C#.
 *              Supports full CRUD operations for books and members, loan tracking,
 *              reservation queuing, overdue fine calculations, role-based access
 *              (Member / Librarian), and persistent data storage using JSON.
 *              
 * Features:
 *   • User authentication with PIN hashing
 *   • Complete CRUD operations for Books and Members
 *   • Borrowing, returning, and advanced reservation queue system
 *   • Fine calculation and member blocking
 *   • Role-based access control (Member vs Librarian)
 *   • Real-time notifications for books ready for pickup
 *   • Search and multi-criteria sorting functionality
 *   • Statistics dashboard and overdue reports
 *   • Full CSV import/export for data backup and migration
 *   • Robust error handling and input validation
 *   • Thread-safe data operations with file locking
 *   • Persistent storage using JSON with atomic writes
 * 
 * Technologies: C#, .NET, System.Text.Json, Console UI
 */
namespace LibraryManagementSystem {
    /// <summary>
    /// Main entry point for the Library Management System console application.
    /// Handles user interface, menu navigation, session management, and routes user actions.
    /// </summary>
    internal class Program {
        private static LibraryService service = new LibraryService();
        private static Member? currentUser = null;
        static void Main(string[] args) {
            service.LoadData();
            service.CreateDefaultAdmin();

            // Loop until successful login
            while (currentUser == null) {
                LoginScreen();
            }

            bool running = true;
            while (running) {
                ShowMainMenu();
                string? choice = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(choice)) {
                    continue;
                }

                if (choice == "0") {
                    service.SaveData();
                    Console.WriteLine("Goodbye!");
                    running = false;
                    break;
                }

                ProcessMenuChoice(choice);

                if (running) {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            Console.WriteLine("\nThank you for using the Library System!");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays the login screen and authenticates the user.
        /// </summary>
        private static void LoginScreen() {
            Console.Clear();
            Console.WriteLine("==== Library Management System ====");
            Console.WriteLine("\n---- LIBRARY LOGIN ----");

            Console.Write("Member ID: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out int memberId)) {
                Console.WriteLine("Invalid Member ID.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter PIN: ");
            string? pin = Console.ReadLine()?.Trim();

            currentUser = service.Login(memberId, pin ?? "");

            if (currentUser == null) {
                Console.WriteLine("\nLogin failed. Please try again.");
                Console.ReadKey();
            } else {
                Console.WriteLine($"\nLogin successful! Welcome, {currentUser.Name} ({currentUser.Role})");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Displays the main menu with options based on the current user's role (Member or Librarian).
        /// </summary>
        public static void ShowMainMenu() {
            Console.Clear();
            Console.WriteLine($"---- Main Menu (Logged in as: {currentUser?.Name} - {currentUser?.Role}) ----");

            BooksReadyForPickup();

            Console.WriteLine("1. View All Books");
            Console.WriteLine("2. Borrow / Reserve Book");
            Console.WriteLine("3. Return Book");
            Console.WriteLine("4. Claim Reserved Book");
            Console.WriteLine("5. Reservations");
            Console.WriteLine("6. View Account Details");
            Console.WriteLine("7. Search");
            Console.WriteLine("8. Sort");

            if (service.IsLibrarian(currentUser!)) {
                Console.WriteLine("\n--- Librarian Options ---");
                Console.WriteLine("9.  Add New Book");
                Console.WriteLine("10. Add New Member");
                Console.WriteLine("11. View All Members");
                Console.WriteLine("12. Edit Book");
                Console.WriteLine("13. Edit Member");
                Console.WriteLine("14. Delete Book");
                Console.WriteLine("15. Delete Member");
                Console.WriteLine("16. Overdue Books Report");
                Console.WriteLine("17. OverDue Fines Report");
                Console.WriteLine("18. View All Loans");
                Console.WriteLine("19. Statistics Dashboard");
                Console.WriteLine("20. Export Data to CSV");
                Console.WriteLine("21. Import Data from CSV");
            }
            Console.WriteLine("0. Exit Application");
            Console.Write("\nEnter Your Choice: ");
        }

        /// <summary>
        /// Displays a notification if the logged-in user has books ready for pickup.
        /// </summary>
        public static void BooksReadyForPickup() {
            if (currentUser == null || currentUser.ReadyForPickupBookIds.Count == 0) {
                return;
            }
            List<Book>? books = service.GetReservedBooksByMember(currentUser.Id);
            if (books == null) {
                return;
            }
            List<Book> orderedBooks = books.OrderBy(b => b.Id).ToList();
            foreach (Book book in orderedBooks) {
                Console.WriteLine($"Book '{book.Title}' is ready for pickup");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Routes the user's menu selection to the appropriate handler method.
        /// </summary>
        private static void ProcessMenuChoice(string? choice) {
            bool isLibrarian = currentUser != null && service.IsLibrarian(currentUser);

            switch (choice) {
                case "1":
                    ViewAllBooks();
                    break;
                case "2":
                    BorrowOrReserveBook();
                    break;
                case "3":
                    ReturnBook();
                    break;
                case "4":
                    ClaimReservedBook();
                    break;
                case "5":
                    ViewReservations();
                    break;
                case "6":
                    ViewAccountDetails();
                    break;
                case "7":
                    Search();
                    break;
                case "8":
                    SortData();
                    break;
                default:
                    if (isLibrarian) {
                        switch (choice) {
                            case "9":
                                AddNewBook();
                                break;
                            case "10":
                                AddNewMember();
                                break;
                            case "11":
                                ViewAllMembers();
                                break;
                            case "12":
                                EditBook();
                                break;
                            case "13":
                                EditMember();
                                break;
                            case "14":
                                DeleteBook();
                                break;
                            case "15":
                                DeleteMember();
                                break;
                            case "16":
                                ShowOverdueBooks();
                                break;
                            case "17":
                                ShowAllFines();
                                break;
                            case "18":
                                ViewAllLoans();
                                break;
                            case "19":
                                ShowStatistics();
                                break;
                            case "20":
                                ExportToCsv();
                                break;
                            case "21":
                                ImportFromCsv();
                                break;
                            default:
                                Console.WriteLine("Invalid choice.");
                                break;
                        }
                    } else {
                        Console.WriteLine("Invalid choice.");
                    }
                    break;
            }
        }

        // --------------------
        // Menu Option Handlers
        // --------------------

        //Option #1: View All Books
        public static void ViewAllBooks() {
            Console.Clear();
            Console.WriteLine("---- View All Books ----");
            List<Book> books = service.GetAllBooks();
            if (books.Count <= 0) {
                Console.WriteLine("No Books Found");
                return;
            }
            foreach (Book book in books) {
                Console.WriteLine(book);
            }
        }

        //Option #2: Borrow/Reserve Book
        public static void BorrowOrReserveBook() {
            Console.Clear();
            Console.WriteLine("---- Borrow / Reserve Book ----");

            List<Book> allBooks = service.GetAllBooks();
            if (allBooks.Count == 0) {
                Console.WriteLine("No books in the library yet.");
                return;
            }

            ViewAllBooks();

            Console.Write("\nEnter Book ID: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out int bookId)) {
                Console.WriteLine("Invalid Book ID.");
                return;
            }

            Book? book = service.GetBookById(bookId);
            if (book == null) {
                Console.WriteLine("Book not found.");
                return;
            }

            int memberId;
            Member? targetMember = null;

            if (currentUser?.Role == UserRole.Member) {
                memberId = currentUser.Id;
                targetMember = currentUser;
            } else {
                Console.Write("Enter Member ID: ");
                if (!int.TryParse(Console.ReadLine()?.Trim(), out memberId)) {
                    Console.WriteLine("Invalid Member ID.");
                    return;
                }

                targetMember = service.GetMemberById(memberId);
                if (targetMember == null) {
                    Console.WriteLine($"Member #{memberId} not found.");
                    return;
                }
                Console.WriteLine($"Acting on behalf of: {targetMember.Name}");
            }

            if (service.BlockMember(targetMember.Id)) {
                Console.WriteLine("\n[WARNING] YOUR ACCOUNT IS BLOCKED due to high outstanding fines.");
                Console.WriteLine("Please pay your fines to restore borrowing privileges.");
                return;
            }

            if (targetMember.BorrowedBookIds.Contains(bookId)) {
                Console.WriteLine($"\nYou are already borrowing '{book.Title}'. You cannot reserve it.");
                return;
            }

            if (book.IsAvailable && book.ReservedByMemberIds.Count == 0) {
                Console.WriteLine($"\nBook is available. Borrowing now...");
                service.BorrowBook(bookId, memberId);
            } else {
                Console.WriteLine($"\nBook '{book.Title}' is currently unavailable or reserved.");

                if (book.ReservedByMemberIds.Contains(memberId)) {
                    Console.WriteLine("You have already reserved this book.");
                } else {
                    Console.Write("Would you like to reserve it? (Y/N): ");
                    string? answer = Console.ReadLine()?.Trim().ToUpper();

                    if (answer == "Y") {
                        service.ReserveBook(bookId, memberId);
                    } else {
                        Console.WriteLine("Action cancelled.");
                    }
                }
            }

            service.SaveData();
        }

        //Option #3: Return Book
        public static void ReturnBook() {
            Console.Clear();
            Console.WriteLine("---- Return Books ----");

            if (service.GetAllBorrowedBooks().Count == 0) {
                Console.WriteLine("No borrowed books to return.");
                return;
            }

            int memberId;
            if (currentUser?.Role == UserRole.Member) {
                memberId = currentUser.Id;
            } else {
                Console.Write("Enter Member ID: ");
                if (!int.TryParse(Console.ReadLine()?.Trim(), out memberId)) {
                    Console.WriteLine("Invalid Member ID.");
                    return;
                }

                Member? targetMember = service.GetMemberById(memberId);
                if (targetMember == null) {
                    Console.WriteLine($"Member #{memberId} not found.");
                    return;
                }
                Console.WriteLine($"Acting on behalf of: {targetMember.Name}");
            }

            List<Book> borrowedBooks = service.GetAllBorrowedBooksByMember(memberId);

            if (borrowedBooks.Count == 0) {
                Console.WriteLine("No borrowed books to return.");
                return;
            }

            Console.WriteLine("\nBorrowed Books:");
            foreach (Book book in borrowedBooks) {
                Console.WriteLine(book);
            }

            Console.Write("Enter a book ID to return: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out int bookId)) {
                Console.WriteLine("Invalid Book ID.");
                return;
            }
            service.ReturnBook(bookId);
        }

        //Option #4: Claim Reserved Book
        public static void ClaimReservedBook() {
            Console.Clear();
            Console.WriteLine("---- Claim Reserved Book ----");

            if (service.GetAllReservedBooks()?.Count == 0) {
                Console.WriteLine("No books currently reserved.");
                return;
            }

            int memberId;
            if (currentUser?.Role == UserRole.Member) {
                memberId = currentUser.Id;
            } else {
                Console.Write("Enter Member ID: ");
                if (!int.TryParse(Console.ReadLine()?.Trim(), out memberId)) {
                    Console.WriteLine("Invalid Member ID.");
                    return;
                }
            }

            Member? member = service.GetMemberById(memberId);
            if (member == null) {
                Console.WriteLine($"Member not found.");
                return;
            }

            List<Book>? reservedBooks = service.GetReservedBooksByMember(memberId);
            if (reservedBooks == null) {
                Console.WriteLine("No books reserved.");
                return;
            }

            foreach (Book book in reservedBooks) {
                Console.WriteLine(book);
            }

            if (service.BlockMember(memberId)) {
                Console.WriteLine("\n[WARNING] YOUR ACCOUNT IS BLOCKED due to high outstanding fines.");
                Console.WriteLine("Please pay your fines to restore borrowing privileges.");
                return;
            }

            Console.Write("Enter Book ID to claim: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out int bookId)) {
                Console.WriteLine("Invalid Book ID.");
                return;
            }

            service.ClaimReservedBook(bookId, memberId);
        }

        //Option #5: Reservations
        public static void ViewReservations() {
            Console.Clear();
            Console.WriteLine("---- Reservations ----");

            if (service.GetAllReservedBooks()?.Count == 0) {
                Console.WriteLine("No books currently reserved.");
                return;
            }

            int memberId;
            if (currentUser?.Role == UserRole.Member) {
                memberId = currentUser.Id;
            } else {
                Console.Write("Enter Member ID: ");
                if (!int.TryParse(Console.ReadLine()?.Trim(), out memberId)) {
                    Console.WriteLine("Invalid Member ID.");
                    return;
                }
            }

            Member? member = service.GetMemberById(memberId);
            if (member == null) {
                Console.WriteLine($"Member not found.");
                return;
            }

            List<Book> reservedBooks = service.GetAllBooks()
                .Where(b => member.ReservedBookIds.Contains(b.Id))
                .ToList();

            List<Book> readyBooks = service.GetAllBooks()
                .Where(b => member.ReadyForPickupBookIds.Contains(b.Id))
                .ToList();

            if (reservedBooks.Count == 0 && readyBooks.Count == 0) {
                Console.WriteLine("You have no reservations or books ready for pickup.");
                return;
            }

            if (readyBooks.Count > 0) {
                Console.WriteLine("\nBooks READY FOR PICKUP:");
                foreach (Book book in readyBooks)
                    Console.WriteLine(book);
            }

            if (reservedBooks.Count > 0) {
                Console.WriteLine("\nCurrently Reserved Books:");
                foreach (Book book in reservedBooks)
                    Console.WriteLine(book);
            }
        }

        //Option #6: View Account Details
        public static void ViewAccountDetails() {
            int memberId;
            if (currentUser?.Role == UserRole.Member) {
                memberId = currentUser.Id;
            } else {
                Console.Write("Enter Member ID: ");
                if (!int.TryParse(Console.ReadLine()?.Trim(), out memberId)) {
                    Console.WriteLine("Invalid Member ID.");
                    return;
                }
            }

            Member? account = service.GetMemberById(memberId);
            if (account == null) {
                Console.WriteLine("Member not found.");
                return;
            }

            Console.Clear();
            Console.WriteLine("---- Account Details ----");
            Console.WriteLine($"Name     : {account.Name}");
            Console.WriteLine($"Email    : {account.Email}");
            Console.WriteLine($"Phone    : {account.Phone ?? "Not Provided"}");
            Console.WriteLine($"Member ID: {account.Id}");

            List<Book> borrowed = service.GetAllBorrowedBooksByMember(account.Id);
            Console.WriteLine($"\nBorrowed Books: {borrowed.Count}");
            if (borrowed.Count > 0) {
                foreach (Book book in borrowed)
                    Console.WriteLine($"   * {book}");
            }

            List<Book>? reserved = service.GetReservedBooksByMember(account.Id);

            Console.WriteLine($"\nReserved Books: {reserved?.Count ?? 0}");
            if (reserved?.Count > 0) {
                foreach (Book book in reserved)
                    Console.WriteLine($"   * {book}");
            }

            decimal totalFines = service.GetMemberTotalFines(account.Id);
            Console.WriteLine($"\nOutstanding Fines: {totalFines:C}");

            if (totalFines > 0) {
                Console.Write("\nWould you like to pay your fines now? (Y/N): ");
                if (Console.ReadLine()?.Trim().ToUpper() == "Y") {
                    PayFines(account.Id);
                }
            }

            if (service.BlockMember(account.Id)) {
                Console.WriteLine("\n[WARNING] YOUR ACCOUNT IS BLOCKED due to high outstanding fines.");
                Console.WriteLine("Please pay your fines to restore borrowing privileges.");
            }
        }

        public static void PayFines(int memberId) {
            decimal totalFines = service.GetMemberTotalFines(memberId);

            if (totalFines == 0) {
                Console.WriteLine("No fines to pay.");
                return;
            }

            Console.WriteLine($"Total fines owed: {totalFines:C}");
            Console.Write("Enter amount to pay: $");

            if (decimal.TryParse(Console.ReadLine()?.Trim(), out decimal amount) && amount > 0) {
                if (amount > totalFines)
                    amount = totalFines;

                service.PayFines(memberId, amount);
            } else {
                Console.WriteLine("Invalid amount.");
            }
        }


        //Option #7: Search
        public static void Search() {
            Console.Clear();
            Console.WriteLine("---- Search ----");

            List<Book> books = service.GetAllBooks();
            List<Member> members = service.GetAllMembers();
            if (books.Count == 0 && members.Count == 1) {
                Console.WriteLine("No data available to search.");
                return;
            }

            string? choice = "1";
            if (currentUser?.Role == UserRole.Librarian) {
                Console.WriteLine("1. Search Books");
                Console.WriteLine("2. Search Members");
                Console.Write("Choose (1-2): ");

                choice = Console.ReadLine()?.Trim();
                if (choice != "1" && choice != "2") {
                    Console.WriteLine("Invalid choice.");
                    return;
                }
            }

            if (choice == "1" && books.Count == 0) {
                Console.WriteLine("No books available to search");
                return;
            }

            Console.Write("Enter search term: ");
            string? term = Console.ReadLine()?.Trim().ToLower();
            if (term == null) {
                Console.WriteLine("Search term cannot be empty.");
                return;
            }

            if (choice == "1") {
                List<Book> results = books.Where(b =>
                b.Title.ToLower().Contains(term) ||
                b.Author.ToLower().Contains(term) ||
                b.Genre.ToLower().Contains(term) ||
                b.ISBN.ToLower().Contains(term)
                ).ToList();

                if (results.Count == 0) {
                    Console.WriteLine($"No books found matching '{term}'.");
                    return;
                } else {
                    Console.WriteLine($"\nFound {results.Count} book(s) matching '{term}':\n");
                    foreach (Book book in results) {
                        Console.WriteLine(book);
                    }
                }
            } else {
                List<Member> results = members.Where(m =>
                    m.Name.ToLower().Contains(term) ||
                    m.Email.ToLower().Contains(term)
                ).ToList();

                if (results.Count == 0) {
                    Console.WriteLine($"No members found matching '{term}'.");
                } else {
                    Console.WriteLine($"\nFound {results.Count} member(s) matching '{term}':\n");
                    foreach (Member member in results) {
                        Console.WriteLine(member);
                    }
                }
            }
        }

        //Option #8: Sort
        public static void SortData() {
            Console.Clear();
            Console.WriteLine("---- Sort Data ----");

            List<Book> allBooks = service.GetAllBooks();
            List<Member> allMembers = service.GetAllMembers();
            if (allBooks.Count == 0 && allMembers.Count <= 1) {
                Console.WriteLine("No data available to sort.");
                return;
            }

            if (currentUser?.Role == UserRole.Member) {
                SortBooks(allBooks);
            } else {
                Console.WriteLine("1. Sort Books");
                Console.WriteLine("2. Sort Members");
                Console.Write("Choose (1-2): ");

                string? choice = Console.ReadLine()?.Trim();

                if (choice != "1" && choice != "2") {
                    Console.WriteLine("Invalid choice.");
                    return;
                }

                if (choice == "1") {
                    SortBooks(allBooks);
                } else {
                    if (allMembers.Count == 0) {
                        Console.WriteLine("No members available to sort through.");
                        return;
                    }
                    SortMembers();
                }
            }
        }

        public static void SortBooks(List<Book> allBooks) {
            Console.Clear();
            if (allBooks.Count == 0) {
                Console.WriteLine("No books available to sort.");
                return;
            }
            Console.WriteLine("\nSort Books by:");
            Console.WriteLine("1. Title");
            Console.WriteLine("2. Author");
            Console.WriteLine("3. Genre");
            Console.WriteLine("4. Availability");
            Console.Write("Choose (1-4): ");

            string? sortType = Console.ReadLine()?.Trim();

            List<Book> sortedBooks = service.GetSortedBooks(sortType);

            Console.WriteLine($"\nSorted Books ({sortedBooks.Count} results):\n");
            foreach (Book book in sortedBooks) {
                Console.WriteLine(book);
            }

        }

        public static void SortMembers() {
            Console.Clear();
            Console.WriteLine("\nSort Members by:");
            Console.WriteLine("1. Name");
            Console.WriteLine("2. Email");
            Console.WriteLine("3. Number of Borrowed Books");
            Console.WriteLine("4. Phone Number");
            Console.Write("Choose (1-4): ");

            string? sortType = Console.ReadLine()?.Trim();

            List<Member> sortedMembers = service.GetSortedMembers(sortType);

            Console.WriteLine($"\nSorted Members ({sortedMembers.Count} results):\n");
            foreach (Member member in sortedMembers) {
                Console.WriteLine(member);
            }
        }

        //Option #9: Add New Book
        public static void AddNewBook() {
            Console.Clear();
            Console.WriteLine("---- Add New Book ----");
            Console.Write("Title: ");
            string? title = string.Empty;

            while (string.IsNullOrEmpty(title)) {
                title = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(title)) {
                    Console.Write("Please enter a valid title: ");
                }
            }

            Console.Write("Author: ");
            string? author = string.Empty;

            while (string.IsNullOrEmpty(author)) {
                author = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(author)) {
                    Console.Write("Please enter a valid author: ");
                }
            }

            Console.Write("ISBN: ");
            string? isbn = string.Empty;

            while (string.IsNullOrEmpty(isbn)) {
                isbn = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(isbn)) {
                    Console.Write("Please enter a valid isbn: ");
                }
            }

            Console.Write("Genre: ");
            string? genre = string.Empty;

            while (string.IsNullOrEmpty(genre)) {
                genre = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(genre)) {
                    Console.Write("Please enter a valid genre: ");
                }
            }

            Book newBook = new Book(title, author, isbn, genre);
            service.AddBook(newBook);
        }

        //Option #10: Add New Member
        public static void AddNewMember() {
            Console.Clear();
            Console.WriteLine("---- Add New Member ----");
            Console.Write("Name: ");
            string? name = string.Empty;

            while (string.IsNullOrEmpty(name)) {
                name = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(name)) {
                    Console.Write("Please enter a valid name: ");
                }
            }

            Console.Write("Email: ");
            string? email = string.Empty;

            while (string.IsNullOrEmpty(email)) {
                email = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(email)) {
                    Console.Write("Please enter a valid email: ");
                }
            }

            Console.Write("Phone Number (optional): ");
            string? phone = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(phone)) {
                Console.WriteLine("No phone number added.");
            }

            string? pin = string.Empty;

            while (true) {
                Console.Write("Pin (4-6 digits): ");
                pin = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(pin) || pin.Length < 4 || pin.Length > 6) {
                    Console.WriteLine("Invalid input. Please enter a valid pin between 4 and 6 digits.\n");
                } else {
                    break;
                }
            }

            Console.WriteLine("Select User Role:");
            Console.WriteLine("1. Regular Member");
            Console.WriteLine("2. Librarian");
            Console.Write("Choice (1-2, default is 1): ");
            string? roleChoice = Console.ReadLine()?.Trim();

            UserRole assignedRole = roleChoice == "2" ? UserRole.Librarian : UserRole.Member;

            Member newMember = new Member(name, email, phone, pin, assignedRole);
            service.AddMember(newMember);
        }

        //Option #11: View All Members
        public static void ViewAllMembers() {
            Console.Clear();
            Console.WriteLine("---- View All Members ----");
            List<Member> members = service.GetAllMembers();
            if (members.Count <= 0) {
                Console.WriteLine("No Members Found");
                return;
            }
            foreach (Member member in members) {
                Console.WriteLine(member);
            }
        }

        //Option #12: Edit Book
        public static void EditBook() {
            Console.Clear();
            Console.WriteLine("---- Edit Book ----");
            if (service.GetAllBooks().Count() == 0) {
                Console.WriteLine("No Books Found");
                return;
            }

            ViewAllBooks();

            Console.Write("Enter Book ID to edit: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out int bookId)) {
                Console.WriteLine("Invalid Book ID.");
                return;
            }

            Book? book = service.GetBookById(bookId);
            if (book == null) {
                Console.WriteLine("Book not found.");
                return;
            }

            Console.WriteLine($"\nCurrent Details: {book}");

            Console.Write("New Title (leave blank to keep current): ");
            string? title = Console.ReadLine()?.Trim();

            Console.Write("New Author (leave blank to keep current): ");
            string? author = Console.ReadLine()?.Trim();

            Console.Write("New ISBN (leave blank to keep current): ");
            string? isbn = Console.ReadLine()?.Trim();

            Console.Write("New Genre (leave blank to keep current): ");
            string? genre = Console.ReadLine()?.Trim();

            Book updatedBook = new Book {
                Title = string.IsNullOrEmpty(title) ? book.Title : title,
                Author = string.IsNullOrEmpty(author) ? book.Author : author,
                ISBN = string.IsNullOrEmpty(isbn) ? book.ISBN : isbn,
                Genre = string.IsNullOrEmpty(genre) ? book.Genre : genre
            };

            service.UpdateBook(bookId, updatedBook);
        }

        //Option #13: Edit Member
        public static void EditMember() {
            Console.Clear();
            Console.WriteLine("---- Edit Member ----");

            if (service.GetAllMembers().Count() == 0) {
                Console.WriteLine("No Books Found");
                return;
            }

            ViewAllMembers();

            Console.Write("Enter Member ID to edit: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out int memberId)) {
                Console.WriteLine("Invalid Member ID.");
                return;
            }

            Member? member = service.GetMemberById(memberId);
            if (member == null) {
                Console.WriteLine("Member not found.");
                return;
            }

            Console.WriteLine($"Leave field blank to keep current value.");
            Console.Write($"New Name [{member.Name}]: ");
            string? name = Console.ReadLine()?.Trim();
            Console.Write($"New Email [{member.Email}]: ");
            string? email = Console.ReadLine()?.Trim();
            Console.Write($"New Phone [{member.Phone}]: ");
            string? phone = Console.ReadLine()?.Trim();
            Console.Write($"New PIN [Hidden]: ");
            string? pin = Console.ReadLine()?.Trim();
            Console.WriteLine($"Current Role: {member.Role}");
            Console.WriteLine("Select New Role (Or press Enter to keep current):");
            Console.WriteLine("1. Regular Member");
            Console.WriteLine("2. Librarian");
            Console.Write("Choice (1-2): ");
            string? roleChoice = Console.ReadLine()?.Trim();

            UserRole updatedRole = member.Role;
            if (roleChoice == "1")
                updatedRole = UserRole.Member;
            if (roleChoice == "2")
                updatedRole = UserRole.Librarian;

            string? updatedPin = string.IsNullOrEmpty(pin) ? null : pin;

            Member updatedMember = new Member {
                Name = string.IsNullOrEmpty(name) ? member.Name : name,
                Email = string.IsNullOrEmpty(email) ? member.Email : email,
                Phone = string.IsNullOrEmpty(phone) ? member.Phone : phone,
                Pin = updatedPin ?? member.Pin,
                Role = updatedRole
            };

            service.UpdateMember(memberId, updatedMember, !string.IsNullOrEmpty(pin));

            if (currentUser != null && currentUser.Id == memberId) {
                currentUser = service.GetMemberById(memberId);
                Console.WriteLine("Your active profile session has been successfully synchronized.");
            }
        }

        //Option #14: Delete Book
        public static void DeleteBook() {
            Console.Clear();
            Console.WriteLine("---- Delete Book ----");
            ViewAllBooks();

            Console.Write("Enter Book ID to delete: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out int bookId)) {
                Console.WriteLine("Invalid Book ID.");
                return;
            }

            Book? book = service.GetBookById(bookId);
            if (book == null) {
                Console.WriteLine("Book not found.");
                return;
            }

            Console.Write($"[Warning] Are you sure you want to permanently delete '{book.Title}'? (Y/N): ");
            string? confirmation = Console.ReadLine()?.Trim().ToUpper();

            if (confirmation == "Y") {
                service.DeleteBook(bookId);
            } else {
                Console.WriteLine("Deletion canceled.");
            }
        }

        //Option #15: Delete Member
        public static void DeleteMember() {
            Console.Clear();
            Console.WriteLine("---- Delete Member ----");
            ViewAllMembers();

            Console.Write("Enter Member ID to delete: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out int memberId)) {
                Console.WriteLine("Invalid Member ID.");
                return;
            }

            Member? member = service.GetMemberById(memberId);
            if (member == null) {
                Console.WriteLine("Member not found.");
                return;
            }

            Console.Write($"[Warning] Are you sure you want to permanently delete '{member.Name}'? (Y/N): ");
            string? confirmation = Console.ReadLine()?.Trim().ToUpper();

            if (confirmation == "Y") {
                service.DeleteMember(memberId);
            } else {
                Console.WriteLine("Deletion canceled.");
            }
        }

        //Option #16: Overdue Books Report
        public static void ShowOverdueBooks() {
            Console.Clear();
            List<Loan> overdueLoans = service.GetOverdueLoans();
            if (overdueLoans.Count == 0) {
                Console.WriteLine("\nNo outstanding overdue books found.");
                return;
            }

            Console.WriteLine("---- Overdue Books Report ----");
            Console.WriteLine("Loan ID | Book Title                | Borrower Name  | Due Date   | Days Overdue");
            foreach (Loan loan in overdueLoans) {
                Book? book = service.GetBookById(loan.BookId);
                Member? member = service.GetMemberById(loan.MemberId);

                int daysOverdue = (DateTime.Now - loan.DueDate).Days;

                string bookTitle = book?.Title ?? "Unknown Book";
                string memberName = member?.Name ?? "Unknown Member";

                Console.WriteLine($"{loan.Id,-7} | {bookTitle,-25} | {memberName,-14} | {loan.DueDate:yyyy-MM-dd} | {daysOverdue} days");
            }
            Console.WriteLine($"Total Overdue Books: {overdueLoans.Count}");
        }

        //Option #17: OverDue Fines Report
        public static void ShowAllFines() {
            Console.Clear();
            List<Loan> overdueLoans = service.GetOverdueLoans();

            if (overdueLoans.Count == 0) {
                Console.WriteLine("No overdue fines found.");
                return;
            }

            Console.WriteLine("---- OVERDUE FINES REPORT ----");

            decimal grandTotal = 0;
            var loansByMember = overdueLoans.GroupBy(l => l.MemberId);

            foreach (var group in loansByMember) {
                Member? member = service.GetMemberById(group.Key);
                decimal memberTotal = 0;

                Console.WriteLine($"\nMember: {member?.Name ?? "Unknown"} (ID: {group.Key})");

                foreach (Loan loan in group) {
                    Book? book = service.GetBookById(loan.BookId);
                    decimal fine = service.CalculateFine(loan);
                    memberTotal += fine;
                    grandTotal += fine;

                    Console.WriteLine($"   Loan #{loan.Id} | Book: {book?.Title ?? "Unknown"} | " +
                                      $"Days Overdue: {(DateTime.Now - loan.DueDate).Days} | " +
                                      $"Fine: {fine:C}");
                }

                Console.WriteLine($"   ** Member Total: {memberTotal:C} **");
            }

            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"GRAND TOTAL FINES OWED: {grandTotal:C}");
        }

        //Option #18: View All Loans
        public static void ViewAllLoans() {
            Console.Clear();
            Console.WriteLine("---- View All Loans ----");
            List<Loan> loans = service.GetAllLoans();
            if (loans.Count <= 0) {
                Console.WriteLine("No Loans Found");
                return;
            }
            Console.WriteLine("Sort Loans By:");
            Console.WriteLine("1. ID");
            Console.WriteLine("2. Book Id");
            Console.WriteLine("3. Member Id");
            Console.WriteLine("4. Active/Closed (default)");
            Console.Write("Choose (1-4): ");

            string? sortType = Console.ReadLine()?.Trim();

            List<Loan> sortedLoans = service.GetSortedLoans(sortType);

            Console.WriteLine($"\nSorted Loans ({sortedLoans.Count} results):\n");

            foreach (Loan loan in sortedLoans) {
                Console.WriteLine(loan);
            }

        }

        //Option #19: Statistics Dashboard
        public static void ShowStatistics() {
            Console.Clear();
            Console.WriteLine("---- LIBRARY STATISTICS DASHBOARD ----");

            List<Book> allBooks = service.GetAllBooks();
            List<Book> allAvailableBooks = service.GetAllAvailableBooks();
            List<Book> allBorrowedBooks = service.GetAllBorrowedBooks();
            List<Member> allMembers = service.GetAllMembers();
            List<Loan> activeLoans = service.GetAllActiveLoans();

            int totalBooks = allBooks.Count;
            int availableBooks = allAvailableBooks.Count;
            int borrowedBooks = allBorrowedBooks.Count;
            int totalMembers = allMembers.Count;
            int overdueCount = service.GetOverdueLoans().Count;

            Console.WriteLine($"Total Books          : {totalBooks}");
            Console.WriteLine($"Available            : {availableBooks}");
            Console.WriteLine($"Currently Borrowed   : {borrowedBooks}");
            Console.WriteLine($"Total Members        : {totalMembers}");
            Console.WriteLine($"Active Loans         : {activeLoans.Count}");
            Console.WriteLine($"Overdue Books        : {overdueCount}");

            if (totalBooks > 0) {
                double availabilityRate = (double)availableBooks / totalBooks * 100;
                Console.WriteLine($"Availability Rate    : {availabilityRate:F1}%");
            }
            Console.WriteLine($"{new string('=', 38)}\n");
        }

        //Option #20: Export Data to CSV
        public static void ExportToCsv() {
            Console.Clear();
            Console.WriteLine("---- Export Data To CSV ----");
            Console.WriteLine("What would you like to export?");
            Console.WriteLine("1. Books");
            Console.WriteLine("2. Members");
            Console.WriteLine("3. Loans");
            Console.Write("Choice (1-3): ");

            string? choice = Console.ReadLine()?.Trim();

            string? entityName = choice switch {
                "1" => "Books",
                "2" => "Members",
                "3" => "Loans",
                _ => null
            };

            if (entityName == null) {
                Console.WriteLine("Invalid choice.");
                return;
            }

            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
            string fileName = $"Library_{entityName}_Export_{timeStamp}.csv";
            string filePath = $"../../../{fileName}";

            switch (choice) {
                case "1":
                    service.ExportBooksToCsv(filePath);
                    break;
                case "2":
                    service.ExportMembersToCsv(filePath);
                    break;
                case "3":
                    service.ExportLoansToCsv(filePath);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }
            Console.WriteLine($"Successfully exported {entityName} to: {fileName}");
        }

        //Option #21: Import Data from CSV
        public static void ImportFromCsv() {
            Console.Clear();
            Console.WriteLine("---- Import from CSV ----");
            Console.WriteLine("1. Import Books");
            Console.WriteLine("2. Import Members");
            Console.WriteLine("3. Import Loans");
            Console.Write("Choice (1-3): ");

            string? choice = Console.ReadLine()?.Trim();

            if (choice != "1" && choice != "2" && choice != "3") {
                Console.WriteLine("Invalid choice.");
                return;
            }

            Console.Write("Enter CSV file path: ");
            string? filePath = Console.ReadLine()?.Trim('"');

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) {
                Console.WriteLine("File not found.");
                return;
            }

            switch (choice) {
                case "1":
                    service.ImportBooksFromCsv(filePath);
                    break;
                case "2":
                    service.ImportMembersFromCsv(filePath);
                    break;
                case "3":
                    service.ImportLoansFromCsv(filePath);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }
        }
    }
}