# CSS Dice Faces

A simple project that displays all six faces of a die using only HTML and CSS.

## About the Project

This project creates visual representations of dice faces (1 through 6) by positioning black dots on white squares. It was built as practice for CSS layout techniques, especially **CSS Grid**.

## Technologies Used

- HTML5
- CSS3 (CSS Grid + Flexbox)

## Features Practiced

- CSS Grid with `grid-template-areas`
- Positioning elements using named grid areas
- Flexbox for centering and wrapping the dice
- Styling circular dots with `border-radius`
- Organizing multiple similar components with classes

## How It Works

Each die is a grid container. The dots (pips) are placed into specific grid areas depending on the face number:

- Face 1 → center
- Face 2 → top-left + bottom-right
- Face 3 → top-left + center + bottom-right
- Face 4 → four corners
- Face 5 → four corners + center
- Face 6 → two columns of three dots

## Project Structure
Dice/
├── dice.html
└── dice.css


## How to View

1. Open `dice.html` in any modern web browser.
2. You will see all six dice faces displayed.

No JavaScript or external libraries required.

## Purpose

This project was created to practice and demonstrate CSS Grid layout skills by recreating the classic patterns found on dice.

---

Created by: Chavie Kipperman  
Year: 2025