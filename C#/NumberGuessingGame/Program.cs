/*
 * Project:     Number Guessing Game
 * Author:      Chava Kipperman
 * Description: A console-based number guessing game where the computer randomly selectsa number between 1 and 100. 
 *              The player tries to guess the number with hints ("Too high" / "Too low"). 
 *              Includes play again functionality and input validation.
 */
namespace NumberGuessingGame {
    /// <summary>
    /// A simple number guessing game where the the computer picks a random number between 1 and 100, and the player tries to guess it.
    /// </summary>
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Number Guessing Game!");

            Random random = new Random();
            bool playagain = true;

            while (playagain) {
                int secretNumber = random.Next(1, 101);
                int attempts = 0;
                int guess;
                bool guessedCorrectly = false;

                Console.WriteLine("\nI'm thinking of a number between 1 and 100.");

                // Game loop: continues until the player guesses correctly
                while (!guessedCorrectly) {
                    guess = GetValidGuess();
                    attempts++;
                    if (guess < secretNumber) {
                        Console.WriteLine("Too low!");
                    } else if (guess > secretNumber) {
                        Console.WriteLine("Too high!");
                    } else {
                        Console.WriteLine($"Correct! You got it in {attempts} attempts!");
                        guessedCorrectly = true;
                    }
                }

                // Asks the player if they would like to play again
                Console.WriteLine("Would you like to play again? (Y/N)");
                string? response = Console.ReadLine()?.Trim().ToUpper();
                if (response != "Y") {
                    playagain = false;
                }
            }
            Console.WriteLine("\nThank you for playing! Come back soon! :)");
            Console.ReadKey();
        }

        /// <summary>
        /// Continuously prompts the player for input until a valid number between 1 and 100 is entered.
        /// </summary>
        /// <returns> A valid integer between 1 and 100</returns>
        static int GetValidGuess() {
            while (true) {
                Console.Write("Enter your guess: ");
                string? input = Console.ReadLine();

                // TryParse safely converts strint to int and validates range.
                if (int.TryParse(input, out int number) && number > 0 && number <= 100) {
                    return number;
                }
                Console.WriteLine("Please enter a valid number.");
            }
        }
    }
}
