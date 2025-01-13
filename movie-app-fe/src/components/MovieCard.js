import React from "react";
import { useNavigate } from "react-router-dom";
import "../styles/MovieCard.css";

const MovieCard = ({ movie }) => {
    const navigate = useNavigate();

    const handleCardClick = () => {
        navigate(`/movie/${movie.title}`); // Navighează către pagina filmului specific
    };

    return (
        <div className="recommended-movie-card" onClick={handleCardClick}>
            <h2>{movie.title}</h2>
            <p>Genre: {movie.genre}</p>
            <p>Language: {movie.language}</p>
            <p>Score: {movie.imdbScore}</p>
        </div>
    );
};

export default MovieCard;