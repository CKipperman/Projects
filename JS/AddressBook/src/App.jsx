import './App.css'
import Address from './Address.jsx'

function App() {
  return (
    <>
      <h1>Famous Places Address Book:</h1>
      <table>
        <thead>
          <tr>
            <th>Location Name:</th>
            <th>Street:</th>
            <th>City, State & Zip:</th>
          </tr>
        </thead>
        <tbody>
          <Address locName="The White House" street="1600 Pennsylvania Avenue NW" city="Washington" state="DC" zip="20500" />
          <Address locName="U.S. Capitol" street="First Street NE & SE" city="Washington" state="DC" zip="20510" />
          <Address locName="The Pentagon" street="1400 Defense Pentagon" city="Arlington" state="VA" zip="20301" />
          <Address locName="Supreme Court" street="1 First Street NE" city="Washington" state="DC" zip="20543" />
          <Address locName="FBI Headquarters" street="935 Pennsylvania Avenue NW" city="Washington" state="DC" zip="20535" />
          <Address locName="United Nations" street="405 East 42nd Street" city="New York" state="NY" zip="10017" />
        </tbody>
      </table>
    </>
  )
}

export default App
