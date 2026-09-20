using System.Text.Json;

namespace LibraryManagementSystem {
    /// <summary>
    /// Core business logic service for the Library Management System.
    /// Manages books, members, loans, reservations, fines, and JSON persistence.
    /// </summary>
    public class LibraryService {
        private static readonly string FilePath = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", LibraryConstants.FilePaths.DataFile)
        );

        private List<Book> books = new List<Book>();
        private List<Member> members = new List<Member>();
        private List<Loan> loans = new List<Loan>();

        private int nextBookId = 1;
        private int nextMemberId = 1;
        private int nextLoanId = 1;

        private static readonly object DataLock = new object();

        /// <summary>
        /// Hashes a PIN using SHA256 for secure storage.
        /// </summary>
        public string HashPin(string pin) {
            if (string.IsNullOrEmpty(pin))
                return string.Empty;

            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create()) {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(pin);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToHexString(hashBytes);
            }
        }

        /// <summary>
        /// Authenticates a member by ID and PIN.
        /// </summary>
        /// <returns>The authenticated Member object on success; otherwise, null.</returns>
        public Member? Login(int memberId, string pin) {
            Member? member = members.FirstOrDefault(m => m.Id == memberId);
            if (member == null) {
                Console.WriteLine("Invalid Member ID.");
                return null;
            }

            string hashedInput = HashPin(pin);
            if (member.Pin != hashedInput) {
                Console.WriteLine("Incorrect PIN.");
                return null;
            }
            return member;
        }

        /// <summary>
        /// Checks if a member has Librarian privileges.
        /// </summary>
        public bool IsLibrarian(Member member) {
            return member.Role == UserRole.Librarian;
        }

        /// <summary>
        /// Creates a default administrative librarian account if no administrator currently exists in the system database.
        /// </summary>
        public void CreateDefaultAdmin() {
            lock (DataLock) {
                if (!members.Any(m => m.Role == UserRole.Librarian)) {
                    string hashedAdminPin = HashPin(LibraryConstants.DefaultAdminPin);
                    Member admin = new Member(LibraryConstants.DefaultAdminName, LibraryConstants.DefaultAdminEmail, LibraryConstants.DefaultAdminPhone, hashedAdminPin, UserRole.Librarian);
                    admin.Id = nextMemberId++;
                    members.Add(admin);
                    Console.WriteLine($"Default Admin created: ID = 1, PIN = {LibraryConstants.DefaultAdminPin}");
                    Console.WriteLine("Press any key to clear and continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        // ----------------
        // Book Operations:
        // ----------------

        /// <summary>
        /// Adds a new book to the library after validating the ISBN is unique.
        /// </summary>
        public void AddBook(Book book) {
            lock (DataLock) {
                if (book == null) {
                    Console.WriteLine("Error: Cannot add a null book.");
                    return;
                }
                if (string.IsNullOrEmpty(book.ISBN.Trim())) {
                    Console.WriteLine("Error: ISBN is required.");
                    return;
                }

                if (books.Any(b => b.ISBN == book.ISBN)) {
                    Console.WriteLine($"A book with ISBN {book.ISBN} already exists.");
                    return;
                }
                book.Id = nextBookId++;
                books.Add(book);
                SaveData();
                Console.WriteLine($"Book added successfully! ID: {book.Id} - {book.Title}");
            }
        }

        public List<Book> GetAllBooks() {
            return books;
        }

        public Book? GetBookById(int id) {
            return books.FirstOrDefault(b => b.Id == id);
        }

        /// <summary>
        /// Updates selected properties of an existing book.
        /// </summary>
        public void UpdateBook(int bookId, Book updatedBook) {
            lock (DataLock) {
                Book? existing = books.FirstOrDefault(b => b.Id == bookId);
                if (existing == null) {
                    Console.WriteLine($"Error: Book with ID {bookId} not found.");
                    return;
                }

                if (!string.IsNullOrEmpty(updatedBook.Title))
                    existing.Title = updatedBook.Title;
                if (!string.IsNullOrEmpty(updatedBook.Author))
                    existing.Author = updatedBook.Author;
                if (!string.IsNullOrEmpty(updatedBook.ISBN))
                    existing.ISBN = updatedBook.ISBN;
                if (!string.IsNullOrEmpty(updatedBook.Genre))
                    existing.Genre = updatedBook.Genre;

                SaveData();
                Console.WriteLine($"Book ID {bookId} updated successfully!");
            }
        }

        /// <summary>
        /// Borrows a book for a member if it is available and the member is not blocked.
        /// </summary>
        public void BorrowBook(int bookId, int memberId) {
            lock (DataLock) {
                Book? bookToBorrow = books.FirstOrDefault(b => b.Id == bookId);
                if (bookToBorrow == null) {
                    Console.WriteLine($"Error: Book with ID {bookId} not found.");
                    return;
                }

                if (!bookToBorrow.IsAvailable) {
                    Console.WriteLine($"Error: Book '{bookToBorrow.Title}' is not available.");
                    return;
                }

                if (bookToBorrow.ReservedByMemberIds.Count > 0) {
                    Console.WriteLine($"Error: Book '{bookToBorrow.Title}' is reserved for another member.");
                    return;
                }

                Member? borrower = members.FirstOrDefault(m => m.Id == memberId);
                if (borrower == null) {
                    Console.WriteLine($"Error: Member with ID {memberId} not found.");
                    return;
                }

                if (BlockMember(memberId)) {
                    Console.WriteLine($"Error: {borrower.Name} is BLOCKED from borrowing due to fines exceeding $10.00.");
                    return;
                }

                bookToBorrow.IsAvailable = false;
                borrower.BorrowedBookIds.Add(bookId);

                Loan newLoan = new Loan(nextLoanId++, bookId, memberId) {
                    PickupDate = DateTime.Now
                };
                loans.Add(newLoan);

                SaveData();
                Console.WriteLine($"Success: Book '{bookToBorrow.Title}' has been borrowed by {borrower.Name}");
            }
        }

        public List<Book> GetAllAvailableBooks() {
            return books.Where(b => b.IsAvailable).ToList();
        }

        public List<Book> GetAllBorrowedBooks() {
            return books.Where(b => !b.IsAvailable).ToList();
        }

        public List<Book> GetAllBorrowedBooksByMember(int memberId) {
            Member? member = members.FirstOrDefault(m => m.Id == memberId);
            if (member == null || member.BorrowedBookIds.Count == 0) {
                return new List<Book>();
            }

            return books
                .Where(book => member.BorrowedBookIds.Contains(book.Id))
                .OrderBy(book => book.Title)
                .ToList();
        }

        /// <summary>
        /// Processes returned inventory items, closes active loans, calculates overdue thresholds, 
        /// and automatically updates state metrics for the next member in the reservation queue.
        /// </summary>
        public void ReturnBook(int bookId) {
            lock (DataLock) {
                Book? book = books.FirstOrDefault(b => b.Id == bookId);
                if (book == null) {
                    Console.WriteLine($"Error: Book with ID {bookId} not found.");
                    return;
                }

                if (book.IsAvailable) {
                    Console.WriteLine($"Error: Book '{book.Title}' is not currently borrowed.");
                    return;
                }

                Console.WriteLine($"Processing return for '{book.Title}' (ID: {bookId})...");

                Loan? loan = loans.FirstOrDefault(l => l.BookId == bookId && l.ReturnDate == null);
                if (loan == null) {
                    Console.WriteLine($"Warning: Active loan record not found for Book ID {bookId}. Proceeding with manual shelf recovery.");
                } else {
                    loan.ReturnDate = DateTime.Now;

                    Member? borrower = members.FirstOrDefault(m => m.Id == loan.MemberId);
                    if (borrower != null) {
                        borrower.BorrowedBookIds.Remove(bookId);
                    }
                }

                book.IsAvailable = true;

                bool assignedToReservation = false;
                int queueIndex = 0;

                while (queueIndex < book.ReservedByMemberIds.Count) {
                    int nextMemberId = book.ReservedByMemberIds[queueIndex];
                    Member? nextMember = members.FirstOrDefault(m => m.Id == nextMemberId);

                    if (nextMember == null) {
                        book.ReservedByMemberIds.RemoveAt(queueIndex);
                        continue;
                    }

                    if (BlockMember(nextMemberId)) {
                        Console.WriteLine($"Reservation skipped for now: Member '{nextMember.Name}' (ID: {nextMemberId}) is BLOCKED due to fines but retains their place in line.");
                        queueIndex++;
                        continue;
                    }

                    book.ReservedByMemberIds.RemoveAt(queueIndex);
                    nextMember.ReservedBookIds.Remove(bookId);
                    nextMember.ReadyForPickupBookIds.Add(bookId);

                    book.IsAvailable = false;
                    Loan newLoan = new Loan(nextLoanId++, bookId, nextMemberId) {
                        PickupDate = null
                    };
                    loans.Add(newLoan);

                    Console.WriteLine($"Book held! It is now ready for pickup by Member ID: {nextMemberId}.");
                    assignedToReservation = true;
                    break;
                }

                if (!assignedToReservation) {
                    Console.WriteLine($"Book '{book.Title}' is now available on the shelves.");
                }

                SaveData();
            }
        }

        /// <summary>
        /// Places a reservation on a book that is currently borrowed.
        /// </summary>
        public void ReserveBook(int bookId, int memberId) {
            lock (DataLock) {
                Book? book = books.FirstOrDefault(b => b.Id == bookId);
                if (book == null) {
                    Console.WriteLine($"Error: Book with ID {bookId} not found.");
                    return;
                }

                if (book.IsAvailable) {
                    Console.WriteLine($"Book '{book.Title}' is currently available. You can borrow it directly.");
                    return;
                }

                Member? member = members.FirstOrDefault(m => m.Id == memberId);
                if (member == null) {
                    Console.WriteLine($"Error: Member with ID {memberId} not found.");
                    return;
                }

                if (BlockMember(memberId)) {
                    Console.WriteLine($"Error: Cannot reserve books. Your account is currently blocked due to fines.");
                    return;
                }

                if (book.ReservedByMemberIds.Contains(memberId)) {
                    Console.WriteLine("You have already reserved this book.");
                    return;
                }

                book.ReservedByMemberIds.Add(memberId);
                member.ReservedBookIds.Add(bookId);

                SaveData();
                Console.WriteLine($"Book '{book.Title}' has been reserved by {member.Name}.");
            }
        }

        public List<Book>? GetAllReservedBooks() {
            return books.Where(b => b.ReservedByMemberIds.Count != 0).ToList();
        }

        public List<Book>? GetReservedBooksByMember(int memberId) {
            Member? member = members.FirstOrDefault(m => m.Id == memberId);
            if (member == null || member.ReadyForPickupBookIds.Count == 0) {
                return null;
            }

            return books
                .Where(book => member.ReadyForPickupBookIds.Contains(book.Id))
                .OrderBy(book => book.Title)
                .ToList();
        }

        /// <summary>
        /// Allows a member to claim a book they have reserved and is ready for pickup.
        /// </summary>
        public void ClaimReservedBook(int bookId, int memberId) {
            lock (DataLock) {
                Book? book = books.FirstOrDefault(b => b.Id == bookId);
                if (book == null) {
                    Console.WriteLine("Book not found.");
                    return;
                }

                Member? member = members.FirstOrDefault(m => m.Id == memberId);
                if (member == null) {
                    Console.WriteLine("Member not found.");
                    return;
                }

                if (!member.ReadyForPickupBookIds.Contains(bookId)) {
                    Console.WriteLine("This book is not ready for you to pick up.");
                    return;
                }

                Loan? loan = loans.FirstOrDefault(l => l.BookId == bookId &&
                                                       l.MemberId == memberId &&
                                                       l.ReturnDate == null);

                if (loan == null) {
                    Console.WriteLine("Error: No active reservation loan tracking record found.");
                    return;
                }

                if (loan.PickupDate != null) {
                    Console.WriteLine("Error: This reservation has already been picked up and claimed.");
                    return;
                }

                if (BlockMember(memberId)) {
                    Console.WriteLine($"Error: {member.Name} is BLOCKED from claiming reservations due to fines exceeding $10.00.");
                    return;
                }

                member.ReservedBookIds.Remove(bookId);
                member.ReadyForPickupBookIds.Remove(bookId);
                member.BorrowedBookIds.Add(bookId);
                book.IsAvailable = false;

                loan.PickupDate = DateTime.Now;

                SaveData();
                Console.WriteLine($"Success: You have successfully borrowed '{book.Title}'.");
            }
        }

        /// <summary>
        /// Deletes a book only if it is available and has no active reservations.
        /// </summary>
        public void DeleteBook(int bookId) {
            lock (DataLock) {
                Book? book = books.FirstOrDefault(b => b.Id == bookId);
                if (book == null) {
                    Console.WriteLine($"Error: Book with ID {bookId} not found.");
                    return;
                }

                if (!book.IsAvailable) {
                    Console.WriteLine($"Cannot delete '{book.Title}' - it is currently borrowed.");
                    return;
                }

                if (book.ReservedByMemberIds.Count > 0) {
                    Console.WriteLine($"Cannot delete '{book.Title}' - it has active reservations.");
                    return;
                }

                books.Remove(book);
                SaveData();
                Console.WriteLine($"Book '{book.Title}' (ID: {bookId}) has been deleted successfully.");
            }
        }

        // ------------------
        // Member Operations:
        // ------------------

        /// <summary>
        /// Registers a new member and securely hashes their PIN.
        /// </summary>
        public void AddMember(Member member) {
            lock (DataLock) {
                if (member == null) {
                    Console.WriteLine("Error: Cannot add a null member.");
                    return;
                }
                if (string.IsNullOrEmpty(member.Name.Trim())) {
                    Console.WriteLine("Error: Member name is required.");
                    return;
                }
                if (string.IsNullOrEmpty(member.Email.Trim())) {
                    Console.WriteLine("Error: Member email is required.");
                    return;
                }

                if (members.Any(m => m.Email.ToLower() == member.Email.ToLower())) {
                    Console.WriteLine($"A member with email {member.Email} already exists.");
                    return;
                }

                if (string.IsNullOrEmpty(member.Pin.Trim())) {
                    Console.WriteLine("Error: Member pin is required.");
                    return;
                }
                member.Pin = HashPin(member.Pin.Trim());

                member.Id = nextMemberId++;
                members.Add(member);
                SaveData();

                Console.WriteLine($"Member added successfully! ID: {member.Id} - {member.Name}");
            }
        }

        public List<Member> GetAllMembers() {
            return members;
        }

        public Member? GetMemberById(int id) {
            return members.FirstOrDefault(m => m.Id == id);
        }

        /// <summary>
        /// Updates member profile information.
        /// </summary>
        public void UpdateMember(int memberId, Member updatedMember, bool pinChanged) {
            lock (DataLock) {
                Member? existing = members.FirstOrDefault(m => m.Id == memberId);
                if (existing == null) {
                    Console.WriteLine($"Error: Member with ID {memberId} not found.");
                    return;
                }

                existing.Name = updatedMember.Name;
                existing.Email = updatedMember.Email;
                existing.Phone = updatedMember.Phone;
                existing.Role = updatedMember.Role;

                if (pinChanged && !string.IsNullOrEmpty(updatedMember.Pin)) {
                    existing.Pin = HashPin(updatedMember.Pin);
                }

                SaveData();
                Console.WriteLine($"Member ID {memberId} updated successfully!");
            }
        }

        /// <summary>
        /// Deletes a member only if they have no active loans, fines, or reservations.
        /// </summary>
        public void DeleteMember(int memberId) {
            lock (DataLock) {
                Member? member = members.FirstOrDefault(m => m.Id == memberId);
                if (member == null) {
                    Console.WriteLine($"Error: Member with ID {memberId} not found.");
                    return;
                }

                if (member.BorrowedBookIds.Count > 0) {
                    Console.WriteLine($"Cannot delete {member.Name} - they have borrowed books.");
                    return;
                }

                if (GetMemberTotalFines(memberId) > 0) {
                    Console.WriteLine($"Cannot delete {member.Name} - they have outstanding unpaid fines of {GetMemberTotalFines(memberId):C}.");
                    return;
                }

                if (member.ReservedBookIds.Count > 0) {
                    Console.WriteLine($"Cannot delete {member.Name} - they have active reservations.");
                    return;
                }

                members.Remove(member);
                SaveData();
                Console.WriteLine($"Member '{member.Name}' (ID: {memberId}) has been deleted successfully.");
            }
        }

        // --------------
        // Loans and Fees
        // --------------
        public List<Loan> GetAllLoans() {
            return loans;
        }

        public List<Loan> GetAllActiveLoans() {
            return loans.Where(l => l.ReturnDate == null).ToList();
        }

        public List<Loan> GetOverdueLoans() {
            return loans.Where(l => (l.ReturnDate == null) && (l.DueDate < DateTime.Now)).ToList();
        }

        /// <summary>
        /// Calculates the overdue fine for a single loan.
        /// </summary>
        public decimal CalculateFine(Loan loan) {
            DateTime calculationEnd = loan.ReturnDate ?? DateTime.Now;

            if (calculationEnd > loan.DueDate) {
                int daysOverdue = (calculationEnd - loan.DueDate).Days;
                return daysOverdue * LibraryConstants.DailyFineRate;
            }
            return 0;
        }

        public decimal GetMemberTotalFines(int memberId) {
            List<Loan> overdueLoans = loans.Where(l => l.MemberId == memberId &&
                                                       l.DueDate < DateTime.Now).ToList();
            decimal total = 0;
            foreach (Loan loan in overdueLoans) {
                total += CalculateFine(loan);
            }
            return total;
        }

        public bool BlockMember(int memberId) {
            return GetMemberTotalFines(memberId) > LibraryConstants.BlockFineThreshold;
        }

        /// <summary>
        /// Applies a payment toward a member's outstanding fines.
        /// </summary>
        public void PayFines(int memberId, decimal amountToPay) {
            lock (DataLock) {
                decimal totalFines = GetMemberTotalFines(memberId);
                if (totalFines == 0) {
                    Console.WriteLine("No outstanding fines to pay.");
                    return;
                }

                if (amountToPay <= 0) {
                    Console.WriteLine("Invalid payment amount.");
                    return;
                }

                if (amountToPay > totalFines) {
                    amountToPay = totalFines;
                }

                List<Loan> overdueLoans = loans.Where(l => l.MemberId == memberId &&
                                                           l.ReturnDate == null &&
                                                           l.DueDate < DateTime.Now)
                                                           .OrderBy(l => l.DueDate)
                                                           .ToList();
                decimal cashLeft = amountToPay;

                foreach (Loan loan in overdueLoans) {
                    decimal fineAmount = CalculateFine(loan);
                    if (fineAmount == 0)
                        continue;

                    DateTime clearTarget = loan.ReturnDate ?? DateTime.Now;

                    if (cashLeft >= fineAmount) {
                        loan.DueDate = clearTarget;
                        cashLeft -= fineAmount;
                    } else {
                        int daysPaid = (int)(cashLeft / LibraryConstants.DailyFineRate);
                        loan.DueDate = loan.DueDate.AddDays(daysPaid);
                        Console.WriteLine($"Partially paid Loan #{loan.Id}");
                        cashLeft = 0;
                        break;
                    }
                }
                SaveData();

                Console.WriteLine($"Successfully paid ${amountToPay - cashLeft:F2} in fines.");
                Console.WriteLine($"True Remaining Fines Owed: {GetMemberTotalFines(memberId):C}");
            }
        }

        // ---------
        // Sort Data
        // ---------
        public List<Book> GetSortedBooks(string? sortType) {
            return sortType switch {
                "2" => books.OrderBy(b => b.Author)
                            .ThenBy(b => b.Title)
                            .ToList(),
                "3" => books.OrderBy(b => b.Genre)
                            .ThenBy(b => b.Title)
                            .ThenBy(b => b.Author)
                            .ToList(),
                "4" => books.OrderBy(b => !b.IsAvailable)
                            .ThenBy(b => b.Title)
                            .ThenBy(b => b.Author)
                            .ToList(),
                _ => books.OrderBy(b => b.Title)
                          .ThenBy(b => b.Author)
                          .ToList()
            };
        }

        public List<Member> GetSortedMembers(string? sortType) {
            var primarySort = members.OrderByDescending(m => m.Role);

            return sortType switch {
                "1" => primarySort.ThenBy(m => m.Name).ThenBy(m => m.Email).ToList(),
                "2" => primarySort.ThenBy(m => m.Email).ThenBy(m => m.Name).ToList(),
                "3" => primarySort.ThenByDescending(m => m.BorrowedBookIds.Count).ThenBy(m => m.Name).ThenBy(m => m.Email).ToList(),
                "4" => primarySort.ThenBy(m => m.Phone ?? string.Empty).ThenBy(m => m.Name).ThenBy(m => m.Email).ToList(),
                _ => primarySort.ThenBy(m => m.Name).ThenBy(m => m.Email).ToList()
            };
        }
        public List<Loan> GetSortedLoans(string? sortType) {
            return sortType switch {
                "1" => loans.OrderBy(l => l.Id)
                            .ToList(),
                "2" => loans.OrderBy(l => l.BookId)
                            .ToList(),
                "3" => loans.OrderBy(l => l.MemberId)
                            .ThenBy(l => l.Id)
                            .ToList(),
                "4" => loans.OrderBy(l => l.ReturnDate.HasValue)
                            .ThenByDescending(l => l.ReturnDate)
                            .ToList(),
                _ => loans.OrderBy(l => l.ReturnDate.HasValue)
                          .ThenByDescending(l => l.ReturnDate)
                          .ToList()
            };
        }

        // -----------------
        // Data Persistence:
        // -----------------

        /// <summary>
        /// Saves all current data to the JSON file using an atomic write pattern.
        /// </summary>
        public void SaveData() {
            lock (DataLock) {
                try {
                    LibraryData data = new LibraryData {
                        Books = books,
                        Members = members,
                        Loans = loans,
                        NextBookId = nextBookId,
                        NextMemberId = nextMemberId,
                        NextLoanId = nextLoanId
                    };

                    string json = JsonSerializer.Serialize(data, new JsonSerializerOptions {
                        WriteIndented = true
                    });

                    string tempPath = FilePath + ".tmp";
                    File.WriteAllText(tempPath, json);
                    File.Move(tempPath, FilePath, overwrite: true);

                    Console.WriteLine("Data saved successfully.");
                } catch (Exception e) {
                    Console.WriteLine($"Error saving data: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Loads library data from the JSON file on application startup.
        /// </summary>
        public void LoadData() {
            if (!File.Exists(FilePath))
                return;

            lock (DataLock) {
                try {
                    string json = File.ReadAllText(FilePath);
                    LibraryData? data = JsonSerializer.Deserialize<LibraryData>(json);

                    if (data != null) {
                        books = data.Books ?? new List<Book>();
                        members = data.Members ?? new List<Member>();
                        loans = data.Loans ?? new List<Loan>();

                        nextBookId = data.NextBookId > 0 ? data.NextBookId : (books.Count > 0 ? books.Max(b => b.Id) + 1 : 1);
                        nextMemberId = data.NextMemberId > 0 ? data.NextMemberId : (members.Count > 0 ? members.Max(m => m.Id) + 1 : 1);
                        nextLoanId = data.NextLoanId > 0 ? data.NextLoanId : (loans.Count > 0 ? loans.Max(l => l.Id) + 1 : 1);
                    }
                } catch (Exception e) {
                    Console.WriteLine($"Error loading data: {e.Message}");
                }
            }
        }

        // -----------------------
        // CSV Export/Import Layer
        // -----------------------
        public void ExportBooksToCsv(string filePath) {
            try {
                using (StreamWriter writer = new StreamWriter(filePath)) {
                    writer.WriteLine("ID,Title,Author,ISBN,Genre,IsAvailable,AddedDate,ReservedByMemberIds");
                    foreach (Book book in books) {
                        writer.WriteLine($"{book.Id},\"{EscapeCsv(book.Title)}\",\"{EscapeCsv(book.Author)}\",{book.ISBN},\"{EscapeCsv(book.Genre)}\",{book.IsAvailable},{book.AddedDate:yyyy-MM-dd},\"{string.Join(";", book.ReservedByMemberIds)}\"");
                    }
                }
                Console.WriteLine($"Books exported successfully to: {filePath}");
            } catch (Exception e) {
                Console.WriteLine($"Error exporting books: {e.Message}");
            }
        }

        public void ExportMembersToCsv(string filePath) {
            try {
                using (StreamWriter writer = new StreamWriter(filePath)) {
                    writer.WriteLine("ID,Name,Email,Phone,HashedPin,Role,BorrowedBookIds,ReservedBookIds,ReadyForPickupBookIds");
                    foreach (Member member in members) {
                        writer.WriteLine($"{member.Id},\"{EscapeCsv(member.Name)}\",\"{EscapeCsv(member.Email)}\",\"{EscapeCsv(member.Phone ?? "")}\",{member.Pin},{member.Role},\"{string.Join(";", member.BorrowedBookIds)}\",\"{string.Join(";", member.ReservedBookIds)}\",\"{string.Join(";", member.ReadyForPickupBookIds)}\"");
                    }
                }
                Console.WriteLine($"Members exported successfully to: {filePath}");
            } catch (Exception e) {
                Console.WriteLine($"Error exporting members: {e.Message}");
            }
        }

        public void ExportLoansToCsv(string filePath) {
            try {
                using (StreamWriter writer = new StreamWriter(filePath)) {
                    writer.WriteLine("ID,BookID,MemberID,BorrowDate,DueDate,ReturnDate,PickupDate");
                    foreach (Loan loan in loans) {
                        string returnDate = loan.ReturnDate?.ToString("yyyy-MM-dd") ?? "Outstanding";
                        string pickupDate = loan.PickupDate?.ToString("yyyy-MM-dd") ?? "";
                        writer.WriteLine($"{loan.Id},{loan.BookId},{loan.MemberId},{loan.BorrowDate:yyyy-MM-dd},{loan.DueDate:yyyy-MM-dd},{returnDate},{pickupDate}");
                    }
                }
                Console.WriteLine($"Loans exported successfully to: {filePath}");
            } catch (Exception e) {
                Console.WriteLine($"Error exporting loans: {e.Message}");
            }
        }

        private string EscapeCsv(string? field) {
            if (string.IsNullOrEmpty(field))
                return "";
            return field.Replace("\"", "\"\"");
        }

        public void ImportBooksFromCsv(string csvFilePath) {
            if (!File.Exists(csvFilePath)) {
                Console.WriteLine("File not found.");
                return;
            }

            try {
                string[] lines = File.ReadAllLines(csvFilePath);
                int success = 0;
                int skipped = 0;

                lock (DataLock) {
                    for (int i = 1; i < lines.Length; i++) {
                        string line = lines[i];
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        List<string> fields = ParseCsvLine(line);
                        if (fields.Count < 5) {
                            skipped++;
                            continue;
                        }

                        try {
                            int bookId = int.Parse(fields[0].Trim());
                            string title = fields[1].Trim();
                            string author = fields[2].Trim();
                            string isbn = fields[3].Trim();
                            string genre = fields[4].Trim();

                            if (books.Any(b => b.Id == bookId || b.ISBN == isbn)) {
                                skipped++;
                                continue;
                            }

                            Book book = new Book(title, author, isbn, genre) {
                                Id = bookId,
                                IsAvailable = fields.Count <= 5 || bool.Parse(fields[5].Trim())
                            };

                            books.Add(book);

                            if (bookId >= nextBookId)
                                nextBookId = bookId + 1;

                            success++;
                        } catch {
                            skipped++;
                        }
                    }
                    SaveData();

                    Console.WriteLine($"Books Import Complete: {success} added, {skipped} skipped.");
                }
            } catch (Exception e) {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
        public void ImportMembersFromCsv(string filePath) {
            try {
                string[] lines = File.ReadAllLines(filePath);
                int success = 0, skipped = 0, duplicate = 0;

                lock (DataLock) {
                    for (int i = 1; i < lines.Length; i++) {
                        string line = lines[i].Trim();
                        if (string.IsNullOrEmpty(line))
                            continue;

                        try {
                            var fields = ParseCsvLine(line);
                            if (fields.Count < 6) {
                                skipped++;
                                continue;
                            }

                            int memberId = int.Parse(fields[0].Trim());
                            string email = fields[2].Trim();

                            if (members.Any(m => m.Email.ToLower() == email.ToLower() || m.Id == memberId)) {
                                duplicate++;
                                continue;
                            }

                            string rawPin = fields[4].Trim();

                            UserRole role = UserRole.Member;
                            if (Enum.TryParse(fields[5].Trim(), out UserRole roleResult)) {
                                role = roleResult;
                            }

                            Member member = new Member {
                                Id = memberId,
                                Name = fields[1].Trim(),
                                Email = email,
                                Phone = fields.Count > 3 && !string.IsNullOrEmpty(fields[3].Trim()) ? fields[3].Trim() : null,
                                Pin = rawPin.Length == 64 && System.Text.RegularExpressions.Regex.IsMatch(rawPin, "^[0-9A-Fa-f]+$")
                                      ? rawPin : HashPin(rawPin),
                                Role = role
                            };

                            if (fields.Count > 6) {
                                string ids = fields[6].Trim();
                                if (!string.IsNullOrEmpty(ids)) {
                                    member.BorrowedBookIds = ids.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(id => int.TryParse(id.Trim(), out int x) ? x : -1)
                                        .Where(id => id > 0).ToList();
                                }
                            }
                            if (fields.Count > 7) {
                                string ids = fields[7].Trim();
                                if (!string.IsNullOrEmpty(ids)) {
                                    member.ReservedBookIds = ids.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(id => int.TryParse(id.Trim(), out int x) ? x : -1)
                                        .Where(id => id > 0).ToList();
                                }
                            }
                            if (fields.Count > 8) {
                                string ids = fields[8].Trim();
                                if (!string.IsNullOrEmpty(ids)) {
                                    member.ReadyForPickupBookIds = ids.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(id => int.TryParse(id.Trim(), out int x) ? x : -1)
                                        .Where(id => id > 0).ToList();
                                }
                            }

                            members.Add(member);
                            if (memberId >= nextMemberId)
                                nextMemberId = memberId + 1;

                            success++;
                        } catch {
                            skipped++;
                        }
                    }
                    SaveData();

                    Console.WriteLine($"Members Import Complete: {success} added, {duplicate} duplicates skipped, {skipped} bad rows.");
                }
            } catch (Exception e) {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
        public void ImportLoansFromCsv(string filePath) {
            try {
                string[] lines = File.ReadAllLines(filePath);
                int success = 0, skipped = 0;

                lock (DataLock) {
                    for (int i = 1; i < lines.Length; i++) {
                        string line = lines[i].Trim();
                        if (string.IsNullOrEmpty(line))
                            continue;

                        try {
                            var fields = ParseCsvLine(line);
                            if (fields.Count < 5) {
                                skipped++;
                                continue;
                            }

                            int loanId = int.Parse(fields[0].Trim());
                            if (loans.Any(l => l.Id == loanId)) {
                                skipped++;
                                continue;
                            }

                            Loan loan = new Loan {
                                Id = loanId,
                                BookId = int.Parse(fields[1].Trim()),
                                MemberId = int.Parse(fields[2].Trim()),
                                BorrowDate = DateTime.Parse(fields[3].Trim()),
                                DueDate = DateTime.Parse(fields[4].Trim()),
                                ReturnDate = fields.Count > 5 && DateTime.TryParse(fields[5].Trim(), out DateTime rd) ? rd : null
                            };

                            if (fields.Count > 6 && DateTime.TryParse(fields[6].Trim(), out DateTime pd)) {
                                loan.PickupDate = pd;
                            }

                            loans.Add(loan);

                            if (loanId >= nextLoanId)
                                nextLoanId = loanId + 1;

                            success++;
                        } catch {
                            skipped++;
                        }
                    }
                    SaveData();

                    Console.WriteLine($"Loans Import Complete: {success} added, {skipped} skipped.");
                }
            } catch (Exception e) {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Custom text parsing token extraction engine targeting standard comma-separated sequences.
        /// Handles embedded quotation-mark boundary checks.
        /// </summary>
        private List<string> ParseCsvLine(string line) {
            var fields = new List<string>();
            var current = new System.Text.StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++) {
                char c = line[i];
                if (c == '"') {
                    inQuotes = !inQuotes;
                } else if (c == ',' && !inQuotes) {
                    fields.Add(current.ToString());
                    current.Clear();
                } else {
                    current.Append(c);
                }
            }
            fields.Add(current.ToString());
            return fields;
        }
    }
}
