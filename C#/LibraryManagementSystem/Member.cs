namespace LibraryManagementSystem {
    /// <summary>
    /// Specifies authorization permissions and system roles for users.
    /// </summary>
    public enum UserRole {
        Member,
        Librarian
    }

    /// <summary>
    /// Represents a registered library user profile, recording credentials, contact information, and tracked book states.
    /// </summary>
    public class Member {
        public int Id {
            get; set;
        }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; } = null;
        public string Pin { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Member;

        public List<int> BorrowedBookIds { get; set; } = new List<int>();
        public List<int> ReservedBookIds { get; set; } = new List<int>();
        public List<int> ReadyForPickupBookIds { get; set; } = new List<int>();

        public Member() {
        }

        /// <summary>
        /// Creates a new library member with basic profile details.
        /// </summary>
        public Member(string name, string email, string? phone = null, string pin = "0000", UserRole role = UserRole.Member) {
            Name = name;
            Email = email;
            Phone = phone;
            Pin = pin;
            Role = role;
        }

        /// <summary>
        /// Returns a formatted string representation of the member's profile and active book counts.
        /// </summary>
        public override string ToString() {
            string paddedRole = Role.ToString().PadRight(9);
            return $"Member #{Id,-3} | {paddedRole} | {Name,-18} | {Email,-27} | Books Borrowed: {BorrowedBookIds.Count,-2} | Ready For Pickup: {ReadyForPickupBookIds.Count}";
        }
    }
}
