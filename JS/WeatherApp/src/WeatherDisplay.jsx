import './weatherDisplay.css'
export default function WeatherDisplay( {weatherData} )  {
    const icon = `https://openweathermap.org/img/w/${weatherData.weather[0].icon}.png`;
    return (
        <div className="haveWeather">
            <div>The weather in {weatherData.name}</div>
            <img src={icon} alt="weather_icon" />
            <div>{weatherData.main.temp} and {weatherData.weather[0].description}</div>
        </div>
    );
}