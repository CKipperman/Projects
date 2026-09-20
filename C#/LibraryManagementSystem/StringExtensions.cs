namespace LibraryManagementSystem {
    /// <summary>
    /// Provides custom utility methods extending the built-in <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions {
        /// <summary>
        /// Centers text within a designated width by applying equal padding to both sides.
        /// </summary>
        /// <param name="text">The source string to center.</param>
        /// <param name="totalWidth">The total desired width of the resulting string.</param>
        /// <param name="paddingChar">The padding character to use (defaults to space).</param>
        /// <returns>The padded, centered string, or the original string if it is already wider than totalWidth.</returns>
        public static string PadCenter(this string? text, int totalWidth, char paddingChar = ' ') {
            if (string.IsNullOrEmpty(text) || totalWidth <= text.Length)
                return text ?? string.Empty;

            int totalPadding = totalWidth - text.Length;
            int leftPadding = totalPadding / 2;

            return text.PadLeft(text.Length + leftPadding, paddingChar)
                       .PadRight(totalWidth, paddingChar);
        }
    }
}
