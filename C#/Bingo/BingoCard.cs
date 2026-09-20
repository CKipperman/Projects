namespace Bingo {
    public class BingoCard {
        const int SIZE = 5;
        const int middle = SIZE / 2;
        const int FREE = 0, MATCH = 0;
        static Random random = new Random();
        int[,] card = new int[SIZE, SIZE];
        private int[,] Card {
            get; set;
        }
        public BingoCard() {
            Card = PopulateCard();
        }
        int[,] PopulateCard() {
            for (int i = 0; i < card.GetLength(0); i++) {
                int minValue = i * 15 + 1; // Minimum value for this row
                int maxValue = minValue + 14; // Maximum value for this row
                for (int j = 0; j < card.GetLength(1); j++) {
                    if (i == middle && j == middle) {
                        card[i, j] = FREE; // Set the center to FREE
                    } else {
                        // Generate random number within the range for this row
                        int slotValue = random.Next(minValue, maxValue + 1);
                        while (IsDuplicate(card, slotValue)) {
                            slotValue = random.Next(minValue, maxValue + 1); // Regenerate if duplicate is found
                        }
                        card[i, j] = slotValue;
                    }
                }
            }
            return card;
        }
        public bool IsDuplicate(int[,] card, int slotValue) {
            for (int i = 0; i < card.GetLength(0); i++) {
                for (int j = 0; j < card.GetLength(1); j++) {
                    if (card[i, j] == slotValue)
                        return true; // Return true if duplicate found
                }
            }
            return false; // No duplicate found
        }
        public void PrintBoard() {
            Console.WriteLine("Your Bingo Card:");
            for (int j = 0; j < card.GetLength(1); j++) {
                for (int i = 0; i < card.GetLength(0); i++) {
                    if (card[i, j] < 10)
                        Console.Write($"{card[i, j]}  ");
                    else
                        Console.Write($"{card[i, j]} ");
                }
                Console.WriteLine();
            }
        }
        public void Match(int randomNumber) {
            for (int i = 0; i < card.GetLength(0); i++) {
                for (int j = 0; j < card.GetLength(1); j++) {
                    if (card[i, j] == randomNumber) {
                        card[i, j] = MATCH;
                        Console.WriteLine("A match was drawn!!!");
                        break;
                    }
                }
            }
        }
        public bool CheckForWinner() {
            // Check for a full row:
            for (int i = 0; i < card.GetLength(0); i++) {
                bool rowWin = true;
                for (int j = 0; j < card.GetLength(1); j++) {
                    if (card[i, j] != MATCH) {
                        rowWin = false;
                        break;
                    }
                }
                if (rowWin)
                    return true;
            }
            // Check for a full column:
            for (int j = 0; j < card.GetLength(1); j++) {
                bool columnWin = true;
                for (int i = 0; i < card.GetLength(0); i++) {
                    if (card[i, j] != MATCH) {
                        columnWin = false;
                        break;
                    }
                }
                if (columnWin)
                    return true;
            }
            // Check for both diagonals:
            bool diagonalWin1 = true, diagonalWin2 = true;
            for (int i = 0; i < SIZE; i++) {
                if (card[i, i] != MATCH)
                    diagonalWin1 = false;
                if (card[i, SIZE - 1 - i] != MATCH)
                    diagonalWin2 = false;
            }
            if (diagonalWin1 || diagonalWin2)
                return true;
            return false;
        }
    }
}
