import './HomeCard.css';

export default function HomeCard({ name, url, price, rooms, beds, baths}) {
  return (
    <div className="homeCard">
        <img src={url} alt={name} />
        <h3>{name}</h3>
        <h2>{price}</h2>
        <div className="homeDetails">
            <h4>{rooms} rooms</h4>
            <h4>{beds} beds</h4>
            <h4>{baths} baths</h4>
        </div>
    </div>
  )
}
