# Die

A simple Python class that simulates a die with a configurable number of sides.

## Features

- Create a die with any number of sides (default: 6)
- Roll the die to get a random result
- Change the number of sides after creation
- Basic validation (sides must be ≥ 1)
- Clean string representation

## Usage

```python
from Die import Die   # or just run the file directly

# Create a standard 6-sided die
d6 = Die()
print(d6)                # → 6-sided Die
print(d6.roll())         # → random number between 1 and 6

# Create a 12-sided die
d12 = Die(12)
print(d12)               # → 12-sided Die

# Change the number of sides
d12.set_sides(20)
print(d12.get_sides())   # → 20
```

## Example Output

When you run the included test code:
6-sided Die
Roll #1: 4
Roll #2: 1
Roll #3: 6
...
Roll #10: 3

12-sided Die
Roll #1: 9
Roll #2: 11
...
Roll #10: 5


## API

| Method              | Description                              |
|---------------------|------------------------------------------|
| `Die(sides=6)`      | Create a die with the given number of sides |
| `roll()`            | Return a random integer from 1 to sides  |
| `get_sides()`       | Return the current number of sides       |
| `set_sides(n)`      | Change the number of sides (must be ≥ 1) |
| `str(die)`          | Returns e.g. `"6-sided Die"`             |

## Requirements

- Python 3.6+
- No external dependencies (uses only the standard library)

## License

Free to use and modify.

---

Created by: Chavie Kipperman  
Year: 2026