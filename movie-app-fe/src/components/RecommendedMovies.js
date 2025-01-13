import React, { useContext } from "react";
import MovieCard from "./MovieCard";
import { MovieContext } from "./MovieContext";
import "../styles/RecommendedMovies.css";

const RecommendedMovies = () => {
    const { movies } = useContext(MovieContext); // Obținem lista de filme din context


    return (
        <div className="recommended-movies">
            <h1>Recommended Movies</h1>
            <div className="movie-list">
                {movies.map((movie) => (
                    <MovieCard key={movie.title} movie={movie} />
                ))}
            </div>
        </div>
    );
};



export default RecommendedMovies;
