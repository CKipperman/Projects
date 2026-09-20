namespace Bingo {
    internal class Player {
        private string name;
        public BingoCard Card {
            get; set;
        }

        public string Name {
            get {
                if (name == null) {
                    return "John Doe";
                }
                return name;
            }

            set {
                string n = value;
                if (n != null && n.Trim().Length != 0) {
                    name = n.Trim();
                }
            }
        }

        public Player() {
            name = Name;
            Card = new BingoCard();
        }

        public void PrintCard() {
            Console.WriteLine($"Player name: {Name}\n");
            Card.PrintBoard();
        }
    }
}
