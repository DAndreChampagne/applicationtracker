import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';

export default function AppNav() {
    return (
        <Navbar expand="lg" className="bg-body-tertiary" fixed="top">
            <Container fluid>
                <Navbar.Brand href="#home">
                    <img
                        src="/vite.svg"
                        className="d-inline-block align-top"
                        alt="Navbar Brand"
                    />
                </Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="me-auto">
                        <Nav.Link href="#home">Home</Nav.Link>
                        <NavDropdown title="Admin" id="dd-admin">
                            <NavDropdown.Item href="#admin1">Section 1</NavDropdown.Item>
                            <NavDropdown.Divider />
                            <NavDropdown.Item href="#admin2">Section 2</NavDropdown.Item>
                        </NavDropdown>
                        <Nav.Link href="#home">Login</Nav.Link>
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    )
}