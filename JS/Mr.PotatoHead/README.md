# Mr. Potato Head

An interactive drag-and-drop Mr. Potato Head style application built with vanilla JavaScript.

## About the Project

Users can drag facial features and body parts from a sidebar onto a main canvas to build their own character. The positions of the parts are saved in `localStorage` so they remain after the page is refreshed.

## Technologies Used

- HTML5
- CSS (CSS Grid)
- Vanilla JavaScript
- localStorage

## Features Practiced

- Drag and drop functionality using mouse events (`mousedown`, `mousemove`, `mouseup`)
- Dynamically positioning elements with absolute positioning
- Managing z-index so the most recently clicked part appears on top
- Saving and loading data with `localStorage`
- Using an IIFE for encapsulation
- Working with images and audio

## How to Use

1. Drag any part from the left sidebar onto the main area.
2. Arrange the parts to create your character.
3. Refresh the page — the parts will reappear in their last saved positions.

## How to Run

1. Open `mrPotatoHead.html` in any modern web browser.

No build tools or installation required.

## Purpose

This project was created to practice mouse event handling, drag-and-drop interactions, and persisting data with `localStorage` in vanilla JavaScript.

---

Created by: Chavie Kipperman  
Year: 2025-2026