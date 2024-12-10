import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styles/RegisterForm.css";

const RegisterForm = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();

        const userData = {
            username: username,
            password: password,
        };

        try {
            const response = await fetch("https://localhost:5267/api/User/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(userData),
            });

            if (response.ok) {
                const data = await response.json();
                console.log("User registered:", data);
                navigate("/preferences");
            } else {
                // În cazul în care backend-ul returnează o eroare
                const errorData = await response.json();
                console.error("Error registering user:", errorData);
            }
        } catch (error) {
            console.error("Error:", error);
        }
    };

    return (
        <div className="register-form">

            <form onSubmit={handleRegister}>

                <h1>Register</h1>

                <input
                    type="text"
                    placeholder="Username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                <button type="submit">Register</button>
            </form>
        </div>
    );
};

export default RegisterForm;