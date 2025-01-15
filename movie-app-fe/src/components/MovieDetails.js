import React, { useEffect, useState, useContext } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { MovieContext } from "./MovieContext";
import "../styles/MovieDetails.css";
import UserContext from "./UserContext";
 
const MovieDetails = () => {
    const { title } = useParams();
    const { username } = useContext(UserContext);
    const navigate = useNavigate();
    const { movies, setMovies } = useContext(MovieContext);
 
    const movie = movies?.find((m) => m.title === title);
 
    const [progress, setProgress] = useState(0);
 
    useEffect(() => {
        if (!movie) return;
 
        const interval = setInterval(() => {
            setProgress((prev) => (prev >= 100 ? 100 : prev + 5));
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
 
    const fetchNewRecommendations = async (url) => {
        try {
            const response = await fetch(url);
            if (response.ok) {
                const data = await response.json();
                setMovies(data.recommendations || []);
                navigate("/recommendations");
            } else {
                console.error("Error fetching recommendations:", response.statusText);
                alert("Failed to fetch new recommendations.");
            }
        } catch (error) {
            console.error("Error:", error);
            alert("An error occurred while fetching recommendations.");
        }
    };
 
    const handleLike = async () => {
        try {
            const response = await fetch("https://localhost:7104/api/FavoriteMovie/addfavorite", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(userData),
            });
 
            if (response.ok) {
                alert(`You liked ${movie.title}!`);
                await fetchNewRecommendations(`https://localhost:7104/api/Recommandation/normalReccLike?username=${username}`);
            } else {
                console.error("Failed to like the movie:", response.statusText);
                alert("An error occurred while liking the movie.");
            }
        } catch (error) {
            console.error("Error liking movie:", error);
            alert("Failed to communicate with the server.");
        }
    };
 
    const handleDislike = async () => {
        try {
            const response = await fetch("https://localhost:7104/api/DislikeMovie/adddislike", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(userData),
            });
 
            if (response.ok) {
                alert(`You disliked ${movie.title}!`);
                await fetchNewRecommendations(`https://localhost:7104/api/Recommandation/normalReccDislike?username=${username}`);
            } else {
                console.error("Failed to dislike the movie:", response.statusText);
                alert("An error occurred while disliking the movie.");
            }
        } catch (error) {
            console.error("Error disliking movie:", error);
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
<button className="like-button" onClick={handleLike}>
<img src="/black-like.svg" alt="Like" className="icon" />
</button>
<button className="dislike-button" onClick={handleDislike}>
<img src="/black-dislike.svg" alt="Dislike" className="icon" />
</button>
<button className="back-button" onClick={() => navigate(-1)}>
<img src="/back.svg" alt="Back" className="icon" />
</button>
</div>
</div>
</div>
    );
};
 
export default MovieDetails;