import Homes from './Homes.json';
import './Home.css';

export default function Home() {
  return (
    <>
      <h1>Home</h1>
      <div className="pictureCarousel">
        {Homes.map(pic => (
          <span key={pic.id}>
            <img src={pic.url} alt="" />
          </span>
        ))}
      </div>
      <h2>For more information contact: (718) Now-Home</h2>
    </>
  )
}
