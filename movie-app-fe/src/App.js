import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { MovieProvider } from "./components/MovieContext";
import { UserProvider } from "./components/UserContext";
import RegisterForm from "./components/RegisterForm";
import PreferencesForm from "./components/PreferencesForm";
import RecommendedMovies from "./components/RecommendedMovies";
import MovieDetails from "./components/MovieDetails";

const App = () => {
    return (
        <UserProvider>
            <MovieProvider>
                <Router>
                    <Routes>
                        <Route path="/" element={<RegisterForm />} />
                        <Route path="/preferences" element={<PreferencesForm />} />
                        <Route path="/recommendations" element={<RecommendedMovies />} />
                        <Route path="/movie/:title" element={<MovieDetails />} />
                    </Routes>
                </Router>
            </MovieProvider>
        </UserProvider>
    );
};

export default App;

