import { FC, useState } from 'react';

import styles from './login.module.css';

const Login: FC<{ onLogin: () => void }> = ({ onLogin }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleLogin = () => {
        if (username && password) {
            onLogin();
        }
    };

    return (
        <div className={styles.loginContainer}>
            <h2>Login</h2>
            <input
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className={styles.inputField}
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className={styles.inputField}
            />
            <button onClick={handleLogin} className={styles.loginButton} disabled={!username || !password}>
                Login
            </button>
        </div>
    );
};

export default Login;
