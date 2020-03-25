import React, { useState } from "react";
import { register } from "./_services/api.service";
import { Redirect } from "react-router-dom";

export function Register() {
    const [isSending, setIsSending] = useState(false);
    const [error, setError] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [shouldContinue, setShouldContinue] = useState(false);

    const handleSubmit = () => {
        setIsSending(true);
        register(username, password)
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
        return (<Redirect to='/login' />);
    }
    return (
        <div>
            <h1>
                Register
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
            {error ? <h2>{error}</h2> : null}
        </div>
    )
}