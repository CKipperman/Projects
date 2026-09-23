export default (req, res, next) => {
    let opStr = req.params.op || req.query.op;

    if (!opStr) {
        return res.status(400).send('Missing operator');
    }

    if (!['+', '-', '*', '/'].includes(opStr)) {
        return res.status(400).send('Invalid operator');
    }

    req.op = opStr;
    next();
}
