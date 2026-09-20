namespace LibraryManagementSystem {
    /// <summary>
    /// Represents a lending transaction tracking when a book is requested, picked up, due, and returned.
    /// </summary>
    public class Loan {
        public int Id {
            get; set;
        }
        public int BookId {
            get; set;
        }
        public int MemberId {
            get; set;
        }
        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime? PickupDate { get; set; } = null;
        private DateTime? _dueDate = null;

        /// <summary>
        /// Gets or sets the return deadline. Dynamically calculates based on standard loan days 
        /// from the PickupDate (or BorrowDate if not yet picked up) unless explicitly overridden.
        /// </summary>
        public DateTime DueDate {
            get {
                if (_dueDate.HasValue)
                    return _dueDate.Value;

                DateTime startDate = PickupDate ?? BorrowDate;
                return startDate.AddDays(LibraryConstants.LoanDays);
            }
            set {
                _dueDate = value;
            }
        }
        public DateTime? ReturnDate { get; set; } = null;

        public Loan() {
        }

        /// <summary>
        /// Creates a loan transaction record linking a book and a member.
        /// </summary>
        public Loan(int id, int bookId, int memberId, DateTime? returnDate = null) {
            Id = id;
            BookId = bookId;
            MemberId = memberId;
            ReturnDate = returnDate;
        }

        /// <summary>
        /// Returns a string summary of the loan information, transaction dates and statuses.
        /// </summary>
        public override string ToString() {
            string returnDate = ReturnDate != null ? $"{ReturnDate:MM/dd/yyyy}" : "Outstanding";
            return $"Loan Id: {Id}, Book Id: {BookId}, Member Id: {MemberId}\nBorrow Date: {BorrowDate:MM/dd/yyyy}, Due Date: {DueDate:MM/dd/yyyy}, Return Date: {returnDate}";
        }
    }
}
