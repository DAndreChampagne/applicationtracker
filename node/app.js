
const express = require('express');
const app = express();
const path = require('path');
const bodyParser = require('body-parser');

// middleware

app.use(express.static('public'));
app.use(bodyParser.urlencoded({ extended: false, }));
app.use(bodyParser.json());


// other routes

app.use('/api', require('./route.api'));

// main routes

app.get('/', (req,res) => {
    res.send('API running');
});


// start up

const server = app.listen('3001', () => {
    console.log('API running at http://localhost:3001');
});

