namespace LibraryManagementSystem {
    /// <summary>
    /// Represents a book in the library system.
    /// Contains details such as title, author, ISBN, genre, availability, and reservation queue.
    /// </summary>
    public class Book {
        public int Id {
            get; set;
        }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public DateTime? AddedDate {
            get; set;
        } = DateTime.Now;

        /// <summary>
        /// List of member IDs who have reserved this book (in order of reservation).
        /// </summary>
        public List<int> ReservedByMemberIds { get; set; } = new List<int>();

        public Book() {
        }

        /// <summary>
        /// Creates a new book with required details.
        /// </summary>
        public Book(string title, string author, string iSBN, string genre) {
            Title = title;
            Author = author;
            ISBN = iSBN;
            Genre = genre;
        }

        /// <summary>
        /// Returns a formatted string representation of the book for display in lists.
        /// </summary>
        public override string ToString() {
            string author = Author.PadCenter(20);
            string status = IsAvailable ? "Available" : "Borrowed";

            return $"{Id,3}) {Title,-25}By: {author} | {Genre,-10} | {status}";
        }
    }
}
