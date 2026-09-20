namespace LibraryManagementSystem {
    /// <summary>
    /// Provides global, system-wide constant values for library rules, financial limits, and default administrative credentials.
    /// </summary>
    public class LibraryConstants {
        public const decimal DailyFineRate = 0.50m;
        public const int LoanDays = 14;
        public const decimal BlockFineThreshold = 10.00m;
        public const string DefaultAdminPin = "1234";
        public const string DefaultAdminName = "Admin";
        public const string DefaultAdminEmail = "admin@library.com";
        public const string DefaultAdminPhone = "000-000-0000";

        /// <summary>
        /// Holds configuration file paths used by the system storage layer.
        /// </summary>
        public static class FilePaths {
            public const string DataFile = "librarydata.json";
        }
    }
}
