import Homes from './Homes.json';
import HomeCard from './HomeCard';
import './BuyHome.css';

export default function BuyHome() {
  return (
    <>
      <h1>Buy Home</h1>
      <div className="homesContainer">
        {Homes.map((home) => (
          <HomeCard
            key={home.id}
            name={home.name}
            url={home.url}
            price={home.price}
            rooms={home.rooms}
            beds={home.beds}
            baths={home.baths}
          />
        ))}
      </div>
    </>
  );
}
