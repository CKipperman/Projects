export default (req, res, next) => {
    let aStr = req.params.a || req.query.a;
    let bStr = req.params.b || req.query.b;

    if (!aStr || !bStr) {
        return res.status(400).send('Missing numbers');
    }

    const numA = parseFloat(aStr);
    const numB = parseFloat(bStr);

    if (isNaN(numA) || isNaN(numB)) {
        return res.status(400).send('The number passed was invalid');
    }

    req.numA = numA;
    req.numB = numB;
    next();
}