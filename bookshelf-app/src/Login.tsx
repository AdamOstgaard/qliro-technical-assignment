import React, { useState, useEffect } from "react";
import { login } from "./_services/api.service";
import { Redirect } from "react-router-dom";

export function Login() {
    const [isSending, setIsSending] = useState(false);
    const [error, setError] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [shouldContinue, setShouldContinue] = useState(false);
    const [shouldRegister, setShouldRegister] = useState(false);

    const handleSubmit = () => {
        setIsSending(true);
        login(username, password)
            .then(() => setShouldContinue(true))
            .catch(e => setError(e.message))
            .finally(() => {
                setIsSending(false);
            });
    }

    if (isSending) {
        return (<div className="lds-ring"><div></div><div></div><div></div><div></div></div>)
    }
    if (shouldContinue) {
        return (<Redirect to='/' />);
    }
    if (shouldRegister) {
        return (<Redirect to='/register' />);
    }
    return (
        <div>
            <h1>
                Sign in
            </h1>
            <h2>
                Username
            </h2>
            <input type="text" onChange={e => setUsername(e.target.value)}/>
            <h2>
                Password
            </h2>
            <input type="password" onChange={e => setPassword(e.target.value)} />
            <button onClick={handleSubmit} >Login</button>
            {error ? <p>{error}</p> : null}
            <button onClick={() => setShouldRegister(true)}>Register</button>
        </div>
    )
}