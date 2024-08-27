'use strict';

const express = require('express');
const app = express();
const port = 3000;

app.use(express.json());

app.get('/', (req, res) => {
    res.send('Hello, World!');
});

app.get('/api/greet', (req, res) => {
    res.json({ message: 'Hello from the API!' });
});

app.listen(port, () => {
    console.log(`Server running at http://localhost:${port}`);
});
