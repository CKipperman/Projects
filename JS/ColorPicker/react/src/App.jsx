import ColorPicker from './colorPicker'
import './App.css'

export default function App() {
  return (
    <>
      <ColorPicker content={
        <div>
          <h1>My Color Picker:</h1>
          <p>This entire box changes background and text color based on your choices below.</p>
          <p>Lorem, ipsum dolor sit amet consectetur adipisicing elit. Eveniet, beatae dicta autem, nisi rem laborum deleniti nihil ipsam dolorum blanditiis velit possimus nesciunt aperiam a voluptas? Voluptates praesentium perferendis illo, similique, ea maxime at illum fugit labore voluptatem quis eius necessitatibus ipsam neque, porro possimus mollitia? Ipsum quae doloribus officia.</p>
        </div>
      } />
    </>
  )
}
