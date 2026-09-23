import random

random_integer = random.randint(1, 100)
print(random_integer)
attempts = 0

while True:
    try:
        guess = int(input('Guess a number between 1 and 100? '))
        attempts += 1

        if guess < 1 or guess > 100:
            print('Please enter a number bewtween 1 and 100.')
            attempts -= 1
            continue

        if guess > random_integer:
            print(f'{guess} is too high, try again')
        elif guess < random_integer:
            print(f'{guess} is too low, try again')
        else:
            print(f'You are correct! The number was {random_integer}')
            print(f'It took you {attempts} attempts.')
            break
    except ValueError:
        print('Please enter a valid number')
        attempts -= 1
        continue
