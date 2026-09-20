namespace TodoListApp {
    public enum Priority {
        Low,
        Medium,
        High
    }
    /// <summary>
    /// Represents a single task in the Todo List application.
    /// </summary>
    public class TodoItem {
        public int Id {
            get; set;
        }
        public string Title { get; set; } = string.Empty;
        public string? Description {
            get; set;
        }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? DueDate {
            get; set;
        }
        public Priority Priority { get; set; } = Priority.Medium;

        /// <summary>
        /// Controls how the Todo item is displayed when printed to console
        /// </summary>
        public override string ToString() {
            string status = IsCompleted ? "[✓]" : "[ ]";

            // Color codes for priority
            string priorityColor = Priority switch {
                Priority.High => "\u001b[31m",   // Red
                Priority.Medium => "\u001b[33m",   // Yellow
                Priority.Low => "\u001b[32m",   // Green
                _ => "\u001b[37m"    // White
            };

            // Reset color back to default
            string resetColor = "\u001b[0m";

            string priorityStr = Priority.ToString();

            // Overdue warning (if not completed)
            string overdue = "";
            if (!IsCompleted && DueDate.HasValue && (DueDate.Value.Date < DateTime.Now.Date)) {
                overdue = " \u001b[31m(OVERDUE!)\u001b[0m";
            }

            string dueInfo = DueDate.HasValue ? $" | Due: {DueDate.Value:MM-dd-yyyy}" : "";

            string descriptionInfo = !string.IsNullOrEmpty(Description) ? $"\n    Description: {Description}" : "";

            return $"{status} {Id}. {Title} | {priorityColor}{priorityStr}{resetColor}{dueInfo}{overdue}{descriptionInfo}";
        }
    }
}
