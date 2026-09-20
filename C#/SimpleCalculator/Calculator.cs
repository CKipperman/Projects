/*
 * Project:     Simple Calculator
 * Author:      Chava Kipperman
 * Description: A console-based calculator that supports addition, subtraction, multiplication, and division with proper input validation.
 */
namespace SimpleCalculator {
    /// <summary>
    /// Program for simple console calculator.
    /// </summary>
    internal class Calculator {
        static void Main(string[] args) {
            Console.WriteLine("Simple Calculator\n");

            bool running = true;

            while (running) {
                // Get input numbers
                Console.Write("Enter first number: ");
                double num1 = GetValidNumber();

                Console.Write("Enter second number: ");
                double num2 = GetValidNumber();

                // Show operation menu
                Console.WriteLine("\nChoose an operation: ");
                Console.WriteLine("\t+ (Addition)");
                Console.WriteLine("\t- (Subtraction)");
                Console.WriteLine("\t* (Multiplication)");
                Console.WriteLine("\t/ (Division)");
                Console.Write("\nEnter your choice ( + - * / ): ");
                string? operation = Console.ReadLine()?.Trim();


                // Perform calculation
                double result = Calculate(num1, num2, operation);

                Console.WriteLine($"\nResult: {num1} {operation} {num2} = {result}\n");

                // Ask user if they want to continue
                Console.WriteLine("Would you like to do another calculation? (Y/N)");
                string? response = Console.ReadLine()?.Trim().ToUpper();
                if (response != "Y") {
                    running = false;
                }
            }
            Console.WriteLine("\nThank you for using my calculator! Come back soon!");
            Console.ReadKey();
        }

        /// <summary>
        /// Continuously prompts the user until a valid number is entered.
        /// </summary>
        /// <returns> A valid double number</returns>
        static double GetValidNumber() {
            while (true) {
                string? input = Console.ReadLine();
                if (double.TryParse(input, out double number)) {
                    return number;
                }
                Console.WriteLine("Invalid input. Please enter a valid number:");
            }
        }

        /// <summary>
        /// Performs the chosen mathematical operation on two numbers.
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <param name="operation">The operator (+, -, *, /)</param>
        /// <returns>The result of the calculation</returns>
        static double Calculate(double a, double b, string? operation) {
            switch (operation) {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    if (b == 0) {
                        Console.WriteLine("Error: Can not divide by zero!");
                        return 0;
                    }
                    return a / b;
                default:
                    Console.WriteLine("Error: Invalid operation!");
                    return 0;
            }
        }
    }
}
