import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/PreferencesForm.css";

const PreferencesForm = () => {
    const [genre, setGenre] = useState("");
    const [highScore, setHighScore] = useState("");
    const [language, setLanguage] = useState("");
    const navigate = useNavigate();

    const genres = ["Actiune", "Documentar", "Comedie", "Drama", "Horror"];
    const languages = ["Engleza", "Italiana", "Spaniola", "Germana"];

    const handleSubmit = (e) => {
        e.preventDefault();
        // Poti salva preferințele local sau le poți trimite către un backend
        navigate("/recommendations");
    };

    return (
        <div className="preferences-form">

            <form onSubmit={handleSubmit}>
                <h1>Seteaza-ti preferintele</h1>
                <label>
                    Genul preferat
                    <select value={genre} onChange={(e) => setGenre(e.target.value)}>
                        <option value="">Select</option>
                        {genres.map((g) => (
                            <option key={g} value={g}>
                                {g}
                            </option>
                        ))}
                    </select>
                </label>
                <label>
                    Preferi sa aiba un scor ridicat?
                    <select value={highScore} onChange={(e) => setHighScore(e.target.value)}>
                        <option value="">Select</option>
                        <option value="yes">Yes</option>
                        <option value="no">No</option>
                    </select>
                </label>
                <label>
                    Limba preferata:
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
