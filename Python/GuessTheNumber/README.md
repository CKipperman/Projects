# Number Guessing Game

A simple command-line number guessing game written in Python.

The computer picks a random number between 1 and 100, and you try to guess it.  
The game tells you whether your guess is too high or too low, and counts how many attempts you need.

## How to Play

1. Run the program:
   ```bash
   python app.py
2. Enter a number between 1 and 100 when prompted.
3. Keep guessing until you find the correct number.
4. The game will tell you how many attempts it took.

## Example Output

42
Guess a number between 1 and 100? 50
50 is too high, try again
Guess a number between 1 and 100? 25
25 is too low, try again
Guess a number between 1 and 100? 42
You are correct! The number was 42
It took you 3 attempts.

*(Note: The first number printed is the secret number — useful for testing. You can remove the `print(random_integer)` line if you want it hidden.)*

## Features

- Random number between 1 and 100
- Input validation (only accepts integers)
- Range checking (rejects numbers outside 1–100)
- Attempt counter
- Clear feedback (“too high” / “too low”)

## Requirements

- Python 3.x
- No external libraries needed

## License

Free to use and modify.

---

Created by: Chavie Kipperman  
Year: 2026