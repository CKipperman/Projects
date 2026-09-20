namespace LibraryManagementSystem {
    /// <summary>
    /// Acts as the core data transfer object (DTO) wrapping the collections of 
    /// library assets, users, transactions, and auto-incrementing ID counters.
    /// </summary>
    public class LibraryData {
        public List<Book> Books { get; set; } = new List<Book>();
        public List<Member> Members { get; set; } = new List<Member>();
        public List<Loan> Loans { get; set; } = new List<Loan>();

        public int NextBookId { get; set; } = 1;
        public int NextMemberId { get; set; } = 1;
        public int NextLoanId { get; set; } = 1;
    }
}
