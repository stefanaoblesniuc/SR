import React, { useEffect, useState, useContext } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { MovieContext } from "./MovieContext";
import "../styles/MovieDetails.css";
import UserContext from "./UserContext";

const MovieDetails = () => {
    const { title } = useParams();
    const {username} = useContext(UserContext);
    const navigate = useNavigate();
    const { movies, setMovies } = useContext(MovieContext); // Obținem și actualizăm lista de filme

   // console.log(title);
  //  console.log(username);
    const movie = movies && Array.isArray(movies) ? movies.find((m) => m.title === title) : null;

    const [progress, setProgress] = useState(0);


/*    useEffect(() => {
        const fetchMovie = async () => {
            try {
                const response = await fetch(`https://localhost:7104/api/Recommandation/normalRecc?username=${username}`);
                if (response.ok) {
                    const data = await response.json();
                    setMovies(data);
                } else {
                    console.error("Movie not found");
                }
            } catch (error) {
                console.error("Error fetching movie details:", error);
            }
        };

        fetchMovie();
    }, [title]); */

    useEffect(() => {
        if (!movie) return;

        const interval = setInterval(() => {
            setProgress((prev) => {
                if (prev >= 100) {
                    clearInterval(interval);
                    return 100;
                }
                return prev + 5;
            });
        }, 300);

        return () => clearInterval(interval);
    }, [movie]);

    if (!movie) {
        return <h1>Loading...</h1>;
    }
    const userData = {
        username: username,
        movieTitle: movie.title,
    };

    const handleLike = async () => {
        try {
            console.log(userData);
            const response = await fetch("https://localhost:7104/api/FavoriteMovie/addfavorite", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(
                    userData
                )
            });
    
            if (response.ok) {
                const result = await response.json();
                alert(`You liked ${movie.title}!`);
                console.log("Response from server:", result);
                const response1 = await fetch(`https://localhost:7104/api/Recommandation/normalRecc?username=${username}`);
                if (response1.ok) {
                    const data = await response1.json();
                    console.log(JSON.stringify(data, null, 2));
                    setMovies(data.recommendations || []);
                } else {
                    console.error("Movie not found");
                }
                // Navigate to recommendations or update state
                navigate("/recommendations");
            } else {
                console.error("Failed to like the movie:", response.statusText);
                alert("An error occurred while liking the movie.");
            }
        } catch (error) {
            console.error("Error sending like request:", error);
            alert("Failed to communicate with the server.");
        }
    };

    return (
        <div className="movie-details">
            <div className="movie-card">
                <h1>{movie.title}</h1>
                <p><strong>Genre:</strong> {movie.genre}</p>
                <p><strong>Language:</strong> {movie.language}</p>
                <p><strong>Score:</strong> {movie.imdbScore}</p>
                <div className="progress-section">
                    <p>Movie Viewing Progress:</p>
                    <div className="progress-bar-container">
                        <div className="progress-bar" style={{ width: `${progress}%` }} />
                    </div>
                </div>
                <div className="buttons">
                    <button className="like-button" onClick={handleLike}>Like</button>
                    <button className="back-button" onClick={() => navigate(-1)}>Go Back</button>
                </div>
            </div>
        </div>
    );
};


export default MovieDetails;

