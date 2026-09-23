var createError = require('http-errors');
var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');

var indexRouter = require('./routes/index');
var usersRouter = require('./routes/users');

var app = express();

// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'hjs');

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser('foo'));
app.use(express.static(path.join(__dirname, 'public')));

app.use((req, res, next) => {
  let visits = Number(req.signedCookies.visits) || 0;
  const username = req.signedCookies.username || null;

  if (req.path === '/') {
    visits++;
  }

  res.locals.visits = visits;
  res.locals.username = username;
  res.locals.noUsername = !username;

  res.cookie('visits', visits.toString(), {
    httpOnly: true,
    secure: true,
    maxAge: 60000 * 60 * 24, // 1 day
    signed: true
  });

  if (req.query.username) {
    res.cookie('username', req.query.username.trim(), {
      httpOnly: true,
      secure: true,
      maxAge: 60000 * 60 * 24 * 30, // 30 days
      signed: true
    });
    res.locals.username = req.query.username.trim();
    res.locals.noUsername = false;
  }

  next();
});

app.use('/', indexRouter);
app.use('/users', usersRouter);

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
