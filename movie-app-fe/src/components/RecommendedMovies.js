import React, { useContext } from "react";
import MovieCard from "./MovieCard";
import { MovieContext } from "./MovieContext";
import "../styles/RecommendedMovies.css";
import { useParams, useNavigate } from "react-router-dom";
import "../styles/MovieDetails.css";
import UserContext from "./UserContext";

const RecommendedMovies = () => {
    const { title } = useParams();
    const {username} = useContext(UserContext);
    const navigate = useNavigate();
    const { movies, setMovies } = useContext(MovieContext); // Obținem și actualizăm lista de filme

   // console.log(title);
  //  console.log(username);
    const movie = movies && Array.isArray(movies) ? movies.find((m) => m.title === title) : null;
    const handleSurpriseClick = async () =>{
        try 
        {
            const response1 = await fetch(`https://localhost:7104/api/Recommandation/randomRecc?username=${username}`);
            if (response1.ok) {
                const data = await response1.json();
                console.log(JSON.stringify(data, null, 2));
                setMovies(data.recommendations || []);
            } else {
                console.error("Movie not found");
            }
            // Navigate to recommendations or update state
            navigate("/recommendations");
            
        } catch (error) {
            console.error("Error sending like request:", error);
            alert("Failed to communicate with the server.");
        }
        navigate("/recommendations");
    };

    return (
        <div className="recommended-movies">
            <h1>Recommended Movies</h1>
            <div className="movie-list">
                {movies.map((movie) => (
                    <MovieCard key={movie.title} movie={movie} />
                ))}
            </div>
 
            <div className="surprise-button-container">
                <button className="surprise-button" onClick={handleSurpriseClick}>
                    SURPRISE
                </button>
            </div>
        </div>
    );
};



export default RecommendedMovies;
