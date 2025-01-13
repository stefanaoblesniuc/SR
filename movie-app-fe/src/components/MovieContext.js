import React, { createContext, useState, useEffect, useContext } from "react";
import UserContext from "./UserContext";

export const MovieContext = createContext();

export const MovieProvider = ({ children }) => {
    const [movies, setMovies] = useState([]);
   // const { username } = useContext(UserContext); // Obținem username-ul din contex
    const fetchMovies = async (username) => {
        try {
            const response = await fetch(`https://localhost:7104/api/Recommandation/coldStartRecc?username=${username}`);
            if (!response.ok) throw new Error("Failed to fetch recommendations");
            const data = await response.json();
            console.log(JSON.stringify(data, null, 2)); 
            setMovies(data.recommendations || []);
        } catch (error) {
            console.error("Error fetching recommendations:", error);
        }
    };
   /* useEffect(() => {
        if (username) {
        const fetchMovies = async () => {
            try {
                const response = await fetch(`https://localhost:7104/api/Recommandation/coldStartRecc?username=${username}`, {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json",
                    },
                });

                if (response.ok) {
                    const data = await response.json();
                    console.log(data);
                    setMovies(data); // Actualizăm contextul cu datele primite
                } else {
                    console.error("Failed to fetch movies:", response.status);
                }
            } catch (error) {
                console.error("Error fetching movies:", error);
            }
        };
            fetchMovies();
        }
    }, [username]); // Fetch doar când username-ul este disponibil*/

    return (
        <MovieContext.Provider value={{ movies, setMovies, fetchMovies }}>
            {children}
        </MovieContext.Provider>
    );
};