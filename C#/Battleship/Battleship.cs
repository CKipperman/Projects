/*
 * Project:     Battleship Game
 * Author:      Chava Kipperman
 * Description: A console-based Battleship game where players target coordinates
 *              on a grid to locate and sink randomly placed hidden ships.
 * 
 * Key Features:
 *   • Random placement of ships with horizontal and vertical orientations
 *   • 10x10 game board grid with collision detection to prevent overlapping ships
 *   • Interactive coordinate selection with input validation
 *   • Visual grid rendering indicating hits, misses, and untargeted water
 *   • Game statistics tracking hit counts, miss counts, and victory condition detection
 * 
 * Technologies: C#, .NET, Console Application
 */
namespace Battleship {
    /// <summary>
    /// Main class for the Battleship console application.
    /// Handles game board initialization, ship placement, board rendering, and main gameplay loop.
    /// </summary>
    internal class Battleship {
        /// <summary>
        /// Stores the lengths of the boats to be placed on the game board.
        /// </summary>
        static int[] boats = { 2, 3, 3, 4, 5 };
        /// <summary>
        /// The size dimension (width and height) of the square game board grid.
        /// </summary>
        const int SIZE = 10;
        static Random random = new Random();

        /// <summary>
        /// Main entry point for the Battleship game. Initializes the board and starts the gameplay.
        /// </summary>
        static void Main(string[] args) {
            int[,] boatBoard = new int[SIZE, SIZE]; // Table for the board (10x10)
            PopulateBoard(boatBoard);
            PlayGame(boatBoard);
        }

        /// <summary>
        /// Displays the current visual representation of the game board in the console.
        /// </summary>
        /// <param name="boatBoard">The 2D array representing the game board state.</param>
        static void PrintBoard(int[,] boatBoard) {
            for (int i = 0; i < boatBoard.GetLength(0); i++) { // Rows
                for (int j = 0; j < boatBoard.GetLength(1); j++) { // Columns
                    if (boatBoard[i, j] == -1)
                        Console.Write("X ");
                    else if (boatBoard[i, j] == -2)
                        Console.Write("0 ");
                    else
                        Console.Write("~ ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Randomly places all predefined boats onto the game board horizontally or vertically,
        /// ensuring no boats overlap or exceed board boundaries.
        /// </summary>
        static void PopulateBoard(int[,] boatBoard) {
            foreach (int boat in boats) {
                bool placed = false;
                while (!placed) {
                    int direction = random.Next(2); // 0 = horizontal, 1 = vertical
                    int maxColumn = direction == 0 ? SIZE - boat : SIZE;
                    int maxRow = direction == 1 ? SIZE - boat : SIZE;
                    int column = random.Next(maxColumn); // Random starting position
                    int row = random.Next(maxRow);
                    // Check if the spot is already taken by another boat:
                    if (direction == 0) {
                        bool spotTaken = false;
                        for (int i = 0; i < boat; i++) {
                            if (boatBoard[row, column + i] != 0) {
                                spotTaken = true;
                                break;
                            }
                        }
                        if (spotTaken) {
                            continue;
                        }
                        for (int j = 0; j < boat; j++) {
                            boatBoard[row, column + j] = boat;
                        }
                        placed = true;
                    } else {
                        bool verticalSpotTaken = false;
                        for (int k = 0; k < boat; k++) {
                            if (boatBoard[row + k, column] != 0) {
                                verticalSpotTaken = true;
                                break;
                            }
                        }
                        if (verticalSpotTaken) {
                            continue;
                        }
                        for (int j = 0; j < boat; j++) {
                            boatBoard[row + j, column] = boat;
                        }
                        placed = true;
                    }
                }
            }
        }

        /// <summary>
        /// Controls the primary gameplay loop, prompting player input for targeting coordinates,
        /// validating input, updating the board state, and checking for win conditions.
        /// </summary>
        static void PlayGame(int[,] boatBoard) {
            int totalBoatCells = 0;
            int hitCount = 0;
            int missCount = 0;
            int inputRow, inputColumn;

            // Calculate the total number of boat cells:
            for (int i = 0; i < boatBoard.GetLength(0); i++) {
                for (int j = 0; j < boatBoard.GetLength(1); j++) {
                    if (boatBoard[i, j] > 0 && boatBoard[i, j] <= 5) { // Boats are numbered 2-5
                        totalBoatCells++;
                    }
                }
            }
            Console.WriteLine($"Choose the coordinates that you would like to target within a {SIZE} x {SIZE} grid.");
            while (hitCount < totalBoatCells) {
                // Row input validation:
                while (true) {
                    Console.Write("Row: ");
                    string? iRow = Console.ReadLine();
                    if (string.IsNullOrEmpty(iRow)) {
                        Console.WriteLine("Row input cannot be empty. Please enter a valid number.");
                        continue;
                    }
                    if (Int32.TryParse(iRow, out inputRow) && inputRow > 0 && inputRow <= SIZE) {
                        break; // Input is valid
                    } else {
                        Console.WriteLine($"Invalid row input. Please enter a number between 1 and {SIZE}.");
                    }
                }
                // Column input validation:
                while (true) {
                    Console.Write("Column: ");
                    string? iColumn = Console.ReadLine();
                    if (string.IsNullOrEmpty(iColumn)) {
                        Console.WriteLine("Column input cannot be empty. Please enter a valid number.");
                        continue;
                    }
                    if (Int32.TryParse(iColumn, out inputColumn) && inputColumn > 0 && inputColumn <= SIZE) {
                        break; // Input is valid
                    } else {
                        Console.WriteLine($"Invalid column input. Please enter a number between 1 and {SIZE}.");
                    }
                }
                Console.Clear();
                Console.WriteLine($"You selected Row: {inputRow}, Column: {inputColumn}.");

                inputColumn--;
                inputRow--;
                for (int i = 0; i < boatBoard.GetLength(0); i++) {
                    for (int j = 0; j < boatBoard.GetLength(1); j++) {
                        if ((inputRow == i) && (inputColumn == j)) {
                            if (boatBoard[i, j] > 0) {
                                Console.WriteLine("HIT!");
                                boatBoard[i, j] = -1;
                                hitCount++;
                            } else if (boatBoard[i, j] == 0) {
                                Console.WriteLine("MISS!");
                                boatBoard[i, j] = -2;
                                missCount++;
                            } else
                                Console.WriteLine("You already targeted this location. Try again.");
                        }
                    }
                }
                PrintBoard(boatBoard);
            }
            // Check if all boats are sunk:
            if (hitCount == totalBoatCells) {
                Console.WriteLine("Congratulations! All boats are sunk!");
                Console.WriteLine($"You missed {missCount} times.");
            }
        }
    }
}
