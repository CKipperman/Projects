import { Link } from 'react-router';
import './Header.css';
import Logo from './assets/blogSiteLogo.png';

export default function Header() {
  return (
      <header>
          <img className="logo" src={Logo} alt="logo" />
          <div>
              <h1>Connect. Share. Inspire.</h1>
              <h4>Join the community and share your ideas!</h4>
          </div>
          <Link to='/' className="homeBtn">Home</Link>
      </header>
  )
}
