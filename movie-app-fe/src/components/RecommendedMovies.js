import React from "react";
import MovieCard from "./MovieCard";
import "../styles/RecommendedMovies.css";

const RecommendedMovies = () => {
    const movies = [
        { id: 1, title: "Inception", genre: "Sci-Fi", language: "English", score: 8.8 },
        { id: 2, title: "Parasite", genre: "Drama", language: "Korean", score: 8.6 },
        { id: 3, title: "The Dark Knight", genre: "Action", language: "English", score: 9.0 },
    ];

    return (
        <div className="recommended-movies">
            <h1>Recommended Movies</h1>
            <div className="movie-list">
                {movies.map((movie) => (
                    <MovieCard key={movie.id} movie={movie} />
                ))}
            </div>
        </div>
    );
};

export default RecommendedMovies;
