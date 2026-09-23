var express = require('express');
var router = express.Router();

/* GET home page. */
router.route('/').get(function(req, res, next) {
  res.render('layout', { 
    title: 'Express', 
    partials: {content: 'index'},
  nameForm: !req.session.name });
}).post((req, res, next) => {
  if (req.body.name && req.body.name.trim()) {
    req.session.name = req.body.name.trim();
  } else {
    req.session.name = null;
  }

  res.redirect('/');
});

module.exports = router;
