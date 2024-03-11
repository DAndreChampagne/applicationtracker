
import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

import Card from './components/Card'
import danHeadshot from './assets/dan-headshot.jpg'

import 'bootstrap/dist/css/bootstrap.min.css'
// import 'bootstrap/dist/js/bootstrap.bundle.min.js'

import { Button, Stack, Badge } from 'react-bootstrap'

import RedAlert from './components/Alert';
import AppNav from './AppNav';


function App() {
  const [count, setCount] = useState(0);

  return (
    <>
      <AppNav />
      <div>
        <a href="https://vitejs.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>

      <h1>Vite + React</h1>
      
      <div className="">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.jsx</code> and save to test HMR
        </p>
      </div>
      
      {/* <div>
        <Card src={danHeadshot} title="Dan Champagne" />
        <Card src={danHeadshot} title="Dan Champagne" />
        <Card src={danHeadshot} title="Dan Champagne" />
        <Card src={danHeadshot} title="Dan Champagne" />
      </div> */}


      <div>
        {/* <RedAlert/> */}

        <Stack direction="horizontal" gap={2}>
          <Button variant="primary" as="a" href="http://www.google.com">Button 1</Button>
          <Button variant="success" as="a" >Button 2</Button>

          <Badge bg="secondary" as={Button}>Test</Badge>

        </Stack>
      </div>


      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>

    </>
  )
}

export default App
