# Weather App (React)

A React application that fetches and displays current weather data for a given ZIP code using the OpenWeatherMap API.

## About the Project

When the app loads, it requests weather information for a predefined ZIP code. If the data is successfully retrieved, it shows the city name, temperature, weather description, and an icon. If the request fails, a fallback message is displayed.

## Technologies Used

- React (Class Components)
- Fetch API
- OpenWeatherMap API
- CSS

## Features Practiced

- React class components and lifecycle methods (`componentDidMount`)
- Fetching data from an external API
- Managing state with `this.setState`
- Conditional rendering
- Passing data to child components via props
- Error handling for failed API requests
- Displaying weather icons from an external source

## Components

- `App.jsx` – Main component that fetches weather data and manages state
- `Header.jsx` – App header
- `WeatherDisplay.jsx` – Displays weather information when available
- `NoWeather.jsx` – Fallback message when weather data is not available

## How to Run

1. Get a free API key from [OpenWeatherMap](https://openweathermap.org/)
2. Replace `'YOUR KEY HERE'` in `App.jsx` with your actual API key
3. Install dependencies:
   ```bash
   npm install
4. npm run dev

## Purpose

This project was created to practice React class components, API integration, and conditional rendering.

---

Created by: Chavie Kipperman  
Year: 2025-2026