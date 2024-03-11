
import PropTypes from 'prop-types';
import './Card.css';

function Card(props) {
    return (
        <div className="card">
            <img className="card-image" alt="card image" src={props.src} />
            <h2 className="card-title">{props.title}</h2>
            <p className="card-text">{props.body}</p>
        </div>
    );
}

Card.propTypes = {
    src: PropTypes.string,
    title: PropTypes.string,
    body: PropTypes.string,
}

Card.defaultProps = {
    src: "https://via.placeholder.com/150",
    title: "",
    body: "",
}

export default Card