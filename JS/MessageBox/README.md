# Custom Message Box

A reusable JavaScript message box (modal/dialog) component created with vanilla JavaScript.

## About the Project

This project implements a custom popup message box that can display a message, optional custom buttons, and an optional modal overlay. Multiple message boxes can appear at the same time and are stacked with increasing z-index.

## Technologies Used

- HTML5
- Vanilla JavaScript
- DOM manipulation

## Features Practiced

- Creating reusable functions with an IIFE (module pattern)
- Dynamically creating and styling DOM elements
- Handling modal vs non-modal dialogs
- Supporting custom buttons and callback functions
- Managing z-index for overlapping boxes
- Positioning elements in the center of the screen
- Event handling for buttons and clicks

## How to Use

```javascript
// Simple message
pcsMessageBox('This is a test!!!');

// With custom buttons and callback
pcsMessageBox('Choose an option', ['Yes', 'No'], (button) => {
  console.log('You clicked:', button);
}, true); // true = modal
```

## How to Use
 
1. Open messageBox.html in any modern web browser.

No build tools or installation required.

## Purpose

This project was created to practice building a reusable UI component from scratch using vanilla JavaScript, focusing on the module pattern, DOM creation, and modal behavior.

Created by: Chavie Kipperman
Year: 2025-2026