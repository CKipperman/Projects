from random import randint

class Die:
    def __init__(self, sides = 6):
        self._sides = sides
    
    def __str__(self):
        return f'{self._sides}-sided Die'
    def get_sides(self):
        return self._sides
    
    def set_sides(self, new_sides):
        if new_sides < 1:
            raise ValueError(f'{new_sides} is not a valid number of sides')
        self._sides = new_sides

    def roll(self):
        random_roll = randint(1, self._sides)
        return random_roll

normalDie = Die(6)

print(normalDie)
for i in range(1, 11):
    result = normalDie.roll()
    print(f'Roll #{i}: {result}')

print()
twelveSided = Die(12)

print(twelveSided)
for i in range(1, 11):
    result = twelveSided.roll()
    print(f'Roll #{i}: {result}')