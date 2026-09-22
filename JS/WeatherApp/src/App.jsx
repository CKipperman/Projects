import { Component } from 'react'
import './App.css'
import Header from './Header';
import WeatherDisplay from './WeatherDisplay';
import NoWeather from './NoWeather';

class App extends Component {
  state = {
    weather: null
  };

  zipCode = '11234';

  async componentDidMount() {
    const key = 'YOUR KEY HERE';
    try {
      const r = await fetch(`https://api.openweathermap.org/data/2.5/weather?zip=${this.zipCode}&appid=${key}&units=imperial&lang=he`);
      const weatherData = await r.json();
      if (!r.ok) {
        throw new Error(`${r.status} - ${r.statusText} - ${weatherData.message}`);
      }
      console.log(weatherData);

      this.setState({
        weather: weatherData
      });
    } catch (e) {
      console.error(e);
      this.setState({
        weather: null
      });
    }
  }

  render() {
    const { weather } = this.state;
    return (
      <div className='weatherContainer'>
        <Header />

        <p>
          Showing weather for ZIP: {this.zipCode}
        </p>

        {weather ? <WeatherDisplay weatherData={weather} /> : <NoWeather />}

      </div>
    );
  }
}

export default App
