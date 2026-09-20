/*
 * Project:     Todo List Application
 * Author:      Chava Kipperman
 * Date:        2026
 * Description: 
 *              A console-based Todo List application that allows users to:
 *              - Create, edit, complete, and delete tasks
 *              - Store task data using JSON persistence
 *              - Assign priorities and due dates
 *              - Detect and display overdue tasks
 */

using System.Text.Json;

namespace TodoListApp {
    internal class Program {
        private static List<TodoItem> todos = new List<TodoItem>();
        private static int nextId = 1;
        private static readonly string FilePath = "../../../todos.json";

        static void Main(string[] args) {
            LoadTodos();

            Console.WriteLine("---- My Todo List ----\n");

            bool running = true;

            while (running) {
                ShowMenu();
                string? choice = Console.ReadLine()?.Trim();

                switch (choice) {
                    case "1":
                        AddTodo();
                        break;
                    case "2":
                        ViewTodos();
                        break;
                    case "3":
                        CompleteTodo();
                        break;
                    case "4":
                        DeleteTodo();
                        break;
                    case "5":
                        EditTodo();
                        break;
                    case "6":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Please try again.\n");
                        break;
                }
            }

            SaveTodos();
            Console.WriteLine("\nThank you for using Todo List! Goodbye!");
            Console.ReadKey();
        }

        // ====================== PERSISTENCE ======================

        /// <summary>
        /// Saves all todo items to a JSON file.
        /// </summary>
        static void SaveTodos() {
            try {
                JsonSerializerOptions options = new JsonSerializerOptions {
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(todos, options);
                File.WriteAllText(FilePath, json);
            } catch (Exception ex) {
                Console.WriteLine($"Warning: Could not save data. {ex.Message}");
            }
        }

        /// <summary>
        /// Loads saved todo items from the JSON file if it exists.
        /// </summary>
        static void LoadTodos() {
            if (!File.Exists(FilePath))
                return;

            try {
                string json = File.ReadAllText(FilePath);
                List<TodoItem>? loadedTodos = JsonSerializer.Deserialize<List<TodoItem>>(json);

                if (loadedTodos != null) {
                    todos = loadedTodos;
                    if (todos.Count > 0) {
                        nextId = todos.Max(t => t.Id) + 1;
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Warning: Could not load previous data. {ex.Message}");
            }
        }

        // ====================== MENU ======================

        /// <summary>
        /// Displays the main menu to the user.
        /// </summary>
        static void ShowMenu() {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("\t1. Add new task");
            Console.WriteLine("\t2. View all tasks");
            Console.WriteLine("\t3. Mark task as complete");
            Console.WriteLine("\t4. Delete task");
            Console.WriteLine("\t5. Edit task");
            Console.WriteLine("\t6. Exit");
            Console.Write("\nEnter your choice (1-6): ");
        }

        // ====================== HELPER METHODS ======================

        /// <summary>
        /// Prompts the user to select a priority level.
        /// </summary>
        /// <returns>The selected Priority enum value.</returns>
        static Priority SetPriority() {
            while (true) {
                Console.WriteLine("\nSelect Priority:");
                Console.WriteLine("\t1. Low");
                Console.WriteLine("\t2. Medium");
                Console.WriteLine("\t3. High");
                Console.Write("\nEnter your choice (1-3): ");

                if (int.TryParse(Console.ReadLine()?.Trim(), out int choice)) {
                    return choice switch {
                        1 => Priority.Low,
                        2 => Priority.Medium,
                        3 => Priority.High,
                        _ => Priority.Medium
                    };
                }
                Console.WriteLine("Invalid choice! Please try again.\n");
            }
        }

        /// <summary>
        /// Prompts the user to enter a due date.
        /// </summary>
        /// <returns>
        /// A valid DateTime if entered correctly; otherwise null.
        /// </returns>
        static DateTime? SetDueDate() {
            Console.Write("Enter due date (MM-dd-yyyy) or press Enter to skip: ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
                return null;

            if (DateTime.TryParse(input, out DateTime dueDate))
                return dueDate;

            Console.WriteLine("Invalid date format. Due date not set.");
            return null;
        }

        // ====================== CORE FEATURES ======================

        /// <summary>
        /// Adds a new task to the list.
        /// </summary>
        static void AddTodo() {
            Console.Write("\nEnter task title: ");
            string? title = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(title)) {
                Console.WriteLine("Title cannot be empty!\n");
                return;
            }

            Console.Write("Enter description (optional): ");
            string? description = Console.ReadLine()?.Trim();

            Priority priority = SetPriority();
            DateTime? dueDate = SetDueDate();

            TodoItem newTodo = new TodoItem {
                Id = nextId++,
                Title = title,
                Description = description,
                Priority = priority,
                DueDate = dueDate
            };

            todos.Add(newTodo);
            Console.WriteLine("Task added successfully!\n");
            SaveTodos();
        }

        /// <summary>
        /// Displays all tasks in the list.
        /// </summary>
        static void ViewTodos() {
            Console.WriteLine("\n---- Your Tasks ----");

            if (todos.Count == 0) {
                Console.WriteLine("No tasks yet!\n");
                return;
            }

            foreach (TodoItem todo in todos) {
                Console.WriteLine(todo);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Marks a task as completed.
        /// </summary>
        static void CompleteTodo() {
            ViewTodos();
            if (todos.Count == 0)
                return;

            Console.Write("Enter task ID to mark as complete: ");
            if (int.TryParse(Console.ReadLine()?.Trim(), out int id)) {
                TodoItem? todo = todos.FirstOrDefault(t => t.Id == id);
                if (todo == null) {
                    Console.WriteLine("Task not found!\n");
                    return;
                }

                if (!todo.IsCompleted) {
                    todo.IsCompleted = true;
                    Console.WriteLine($"Task #{id} marked as complete!\n");
                    SaveTodos();
                } else {
                    Console.WriteLine("Task is already completed.\n");
                }
            } else {
                Console.WriteLine("Invalid ID!\n");
            }
        }

        /// <summary>
        /// Allows editing of an existing task.
        /// </summary>
        static void EditTodo() {
            ViewTodos();
            if (todos.Count == 0)
                return;

            Console.Write("Enter task ID to edit: ");
            if (!int.TryParse(Console.ReadLine()?.Trim(), out int id)) {
                Console.WriteLine("Invalid ID!\n");
                return;
            }

            TodoItem? todo = todos.FirstOrDefault(t => t.Id == id);
            if (todo == null) {
                Console.WriteLine("Task not found!\n");
                return;
            }

            Console.WriteLine($"\nEditing Task #{id}: {todo.Title}");
            Console.WriteLine("Press Enter to keep current value.\n");

            // Edit Title
            Console.Write($"New Title (current: {todo.Title}): ");
            string? newTitle = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(newTitle))
                todo.Title = newTitle;

            // Edit Description
            string currentDesc = string.IsNullOrEmpty(todo.Description) ? "None" : todo.Description;
            Console.Write($"New Description (current: {currentDesc}) to clear write \"clear\": ");
            string? newDesc = Console.ReadLine()?.Trim();

            if (newDesc?.ToLower() == "clear")
                todo.Description = null;
            else if (!string.IsNullOrEmpty(newDesc))
                todo.Description = newDesc;

            // Edit Priority
            Console.Write("Change Priority? (Y/N): ");
            if (Console.ReadLine()?.Trim().ToUpper() == "Y")
                todo.Priority = SetPriority();

            // Edit Due Date
            Console.Write("Change Due Date? (Y/N): ");
            if (Console.ReadLine()?.Trim().ToUpper() == "Y")
                todo.DueDate = SetDueDate();

            Console.WriteLine($"Task #{id} updated successfully!\n");
            SaveTodos();
        }

        /// <summary>
        /// Deletes a task from the list.
        /// </summary>
        static void DeleteTodo() {
            ViewTodos();
            if (todos.Count == 0)
                return;

            Console.Write("Enter task ID to delete: ");
            if (int.TryParse(Console.ReadLine()?.Trim(), out int id)) {
                TodoItem? todo = todos.FirstOrDefault(t => t.Id == id);
                if (todo != null) {
                    todos.Remove(todo);
                    Console.WriteLine($"Task #{id} deleted successfully!\n");
                    SaveTodos();
                } else {
                    Console.WriteLine("Task not found!\n");
                }
            } else {
                Console.WriteLine("Invalid ID!\n");
            }
        }
    }
}