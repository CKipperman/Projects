import './noWeather.css'
export default function NoWeather () {
    return (
        <div className = "noWeather">
            <h2>Enter valid zip to see weather</h2>
            <h5 className="error"></h5>
        </div>
    );
}