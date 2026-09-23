var createError = require('http-errors');
var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');
var session = require('express-session');

var indexRouter = require('./routes/index');
var usersRouter = require('./routes/users');

var app = express();

app.use(session({
  secret: 'foo',
  resave: false,
  saveUninitialized: false
}));

// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'hjs');

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.use((req, res, next) => {
  res.locals.name = req.session.name ?? 'Guest (please enter your name)';
  next();
});

app.use('/', indexRouter);
app.use('/users', usersRouter);

app.get('/viewCount', (req, res, next) => {
  let viewCount = req.session.viewCount ?? 0;
  viewCount++;
  req.session.viewCount = viewCount;
  res.send(`Hi ${res.locals.name}! You have visited this page ${viewCount} times.
    <br>
    <a href='/'>Home</a> `)
});

app.get('/aboutUs', (req, res) => {
  res.render('layout', {
    title: 'About Us',
    partials: { content: 'aboutUs' }
  });
});

// catch 404 and forward to error handler
app.use(function(req, res, next) {
  next(createError(404));
});

// error handler
app.use(function(err, req, res, next) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  // render the error page
  res.status(err.status || 500);
  res.render('error');
});

module.exports = app;
