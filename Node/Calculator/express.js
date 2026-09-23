import express from 'express';
import parseNumbers from './parseNumbers.js';
import parseOperator from './parseOperator.js';

const app = express();

app.get('/add', parseNumbers, (req, res) => {
    const result = req.numA + req.numB;
    res.send(result.toString());
});

app.get('/add/:a/:b', parseNumbers, (req, res) => {
    const result = req.numA + req.numB;
    res.send(result.toString());
});

app.get('/subtract', parseNumbers, (req, res) => {
    const result = req.numA - req.numB;
    res.send(result.toString());
});

app.get('/subtract/:a/:b', parseNumbers, (req, res) => {
    const result = req.numA - req.numB;
    res.send(result.toString());
});

app.get('/operate', parseOperator, parseNumbers, (req, res) => {
    let result;
    switch (req.op) {
        case '+':
            result = req.numA + req.numB;
            break;
        case '-':
            result = req.numA - req.numB;
            break;
        case '*':
            result = req.numA * req.numB;
            break;
        case '/':
            if (req.numB === 0) {
                return res.status(400).send('Division by zero is not allowed');
            }
            result = req.numA / req.numB;
            break;
    }
    res.send(result.toString());
});

app.get('/operate/:op/:a/:b', parseOperator, parseNumbers, (req, res) => {
    let result;
    switch (req.op) {
        case '+':
            result = req.numA + req.numB;
            break;
        case '-':
            result = req.numA - req.numB;
            break;
        case '*':
            result = req.numA * req.numB;
            break;
        case '/':
            if (req.numB === 0) {
                return res.status(400).send('Division by zero is not allowed');
            }
            result = req.numA / req.numB;
            break;
    }
    res.send(result.toString());
});

app.listen(3000);