import { Link } from "react-router";
import './Header.css'

export default function Header() {
  return (
    <header>
      <div className="logo">
        <Link to='/'><img src="/realEstateLogo.png" alt="" /></Link>
      </div>
      <nav className="navLinks">
        <Link to='/'>Home</Link>
        <Link to='/buyHome'>Buy</Link>
        <Link to='/sellHome'>Sell</Link>
      </nav>
    </header>
  )
}
