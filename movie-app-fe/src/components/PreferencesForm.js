import React, { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/PreferencesForm.css";
import UserContext from "./UserContext";
import {MovieContext} from "./MovieContext";

const PreferencesForm = () => {
    const { fetchMovies } = useContext(MovieContext);
    const [genres, setGenre] = useState("");
    const [imdbScore, setHighScore] = useState("");
    const [language, setLanguage] = useState("");
    const navigate = useNavigate();

    const { username } = useContext(UserContext);

    const genress = ["Action", "Documentary", "Comedy", "Drama", "Horror"];
    const languages = ["English", "Italian", "Spanish", "German"];

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Create an object for the data
        const preferences = {
            genres,
            imdbScore,
            language,
            username
        };

        console.log(preferences);

        try {
            // Send the data to the API
            const response = await fetch("https://localhost:7104/api/PreferenceForm", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(preferences),
            });

            if (!response.ok) {
                throw new Error("Failed to save preferences.");
            }
            fetchMovies(username);
            // Navigate to the recommendations page on success
            navigate("/recommendations");
        } catch (error) {
            console.error("Error submitting preferences:", error);
            alert("An error occurred while saving your preferences. Please try again.");
        }
    };

    return (
        <div className="preferences-form">
            <form onSubmit={handleSubmit}>
                <h1>Set your preferences!</h1>
                <label>
                    Favorite genre
                    <select value={genres} onChange={(e) => setGenre(e.target.value)}>
                        <option value="">Select</option>
                        {genress.map((g) => (
                            <option key={g} value={g}>
                                {g}
                            </option>
                        ))}
                    </select>
                </label>
                <label>
                    High score on IMDB?
                    <select value={imdbScore} onChange={(e) => setHighScore(e.target.value)}>
                        <option value="">Select</option>
                        <option value="yes">Yes</option>
                        <option value="no">No</option>
                    </select>
                </label>
                <label>
                    Language:
                    <select value={language} onChange={(e) => setLanguage(e.target.value)}>
                        <option value="">Select</option>
                        {languages.map((lang) => (
                            <option key={lang} value={lang}>
                                {lang}
                            </option>
                        ))}
                    </select>
                </label>
                <button type="submit">Send</button>
            </form>
        </div>
    );
};

export default PreferencesForm;