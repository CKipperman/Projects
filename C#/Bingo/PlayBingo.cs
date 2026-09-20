namespace Bingo {
    internal class PlayBingo {
        static Random random = new Random();

        static void Main(string[] args) {
            Console.WriteLine("How many people are playing Bingo?");
            string r = Console.ReadLine();
            int numOfPlayers = Convert.ToInt32(r);
            Player[] player = new Player[numOfPlayers];
            for (int i = 0; i < player.Length; i++) {
                player[i] = new Player();
                Console.WriteLine($"please enter player {i + 1}'s name:");
                string s = Console.ReadLine();
                player[i].Name = s;
            }
            Console.Clear();
            for (int i = 0; i < player.Length; i++) {
                player[i].PrintCard();
                Console.WriteLine();
            }
            bool winner = false;
            while (!winner) {
                int rand = random.Next(1, 76);
                Console.Clear();
                for (int i = 0; i < player.Length; i++) {
                    PlayGame(player[i], rand);
                    player[i].PrintCard();
                    Console.WriteLine("");
                }
                for (int i = 0; i < player.Length; i++) {
                    if (player[i].Card.CheckForWinner()) {
                        winner = true;
                        Console.WriteLine($"Bingo!!! {player[i].Name} is the winner! ");
                        break;
                    }
                }
                if (!winner) {
                    winner = false;
                    Console.WriteLine("Press any key to pick the next number:");
                    Console.ReadLine();
                }
            }
        }
        static void PlayGame(Player player, int randomNumber) {
            Console.WriteLine($"Number picked: {randomNumber}");
            player.Card.Match(randomNumber);
        }
    }
}
