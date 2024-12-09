import React from "react";
import "../styles/MovieCard.css";

const MovieCard = ({ movie }) => {
    return (
        <div className="movie-card">
            <h2>{movie.title}</h2>
            <p>Genre: {movie.genre}</p>
            <p>Language: {movie.language}</p>
            <p>Score: {movie.score}</p>
        </div>
    );
};

export default MovieCard;
