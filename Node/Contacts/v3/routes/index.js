import express from 'express';
import nocache from 'nocache';
import pool from '../pool.js';
const router = express.Router();

router.get('/', async function (req, res, next) {
  try {
    const [results] = await pool.execute(
      'SELECT * FROM contacts'
    );

    res.render('layout', {
      title: 'Contact List',
      contacts: results,
      noContacts: !results?.length,
      partials: { content: 'index' }
    });
  } catch (err) {
    return next(err);
  }
});

router.route('/addContact')
  .get((req, res, next) => {
    res.render('layout', {
      title: 'Add Contact',
      partials: { content: 'contactForm' }
    });
  }).post(async (req, res, next) => {
    try {
      const [results] = await pool.execute(
        `INSERT INTO contacts(first, last, email, phone) VALUES (?,?,?,?)`,
        [req.body.first, req.body.last, req.body.email, req.body.phone]
      );

      res.writeHead(301, {
        location: '/'
      });

      res.end();
    } catch (err) {
      return next(err);
    }
  });

router.get('/:id', async (req, res, next) => {
  try {
    const [rows] =  await pool.execute(
      'SELECT id, first, last, email, phone FROM contacts WHERE id = ?',
      [req.params.id]
    );

    if (rows.length === 0) {
      return res.status(404).json({
        error: 'Contact not found',
        id: req.params.id
      });
    }

    res.status(200).json(rows[0]);
  } catch (err){
    return next(err);
  }
});

router.param('id', async (req, res, next, id) => {
  try {
    const [results] = await pool.execute(
      `SELECT id, first, last, email, phone FROM contacts WHERE id = ?`,
      [req.params.id]
    );

    console.log(results);

    if (!results.length) {
      //return next(new Error(`contact ${req.params.id} not found`));
      req.contact = null;
      return next();
    }

    req.contact = results[0];
  } catch (err) {
    return next(err);
  }

  next();
});

router.route('/editContact/:id')
  .get((req, res, next) => {
    res.render('layout', {
      title: 'Edit Contact',
      contact: req.contact,
      partials: { content: 'contactForm' }
    });
  }).post(async (req, res, next) => {
    try {
      const [results] = await pool.execute(
        `UPDATE contacts SET first = ?, last = ?, email = ?, phone  = ? WHERE id = ?`,
        [req.body.first, req.body.last, req.body.email, req.body.phone, req.params.id]
      );

      if (!results.changedRows) {
        next(new Error(`contact ${req.params.id} not found`))
      }

      res.writeHead(301, {
        location: '/'
      });

      res.end();
    } catch (err) {
      return next(err);
    }
  });

router.get('/deleteContact/:id', nocache(), async (req, res, next) => {
  try {
    const [results] = await pool.execute(
      `DELETE FROM contacts WHERE id = ?`,
      [req.params.id]
    );

    console.log(results);
    if (!results.affectedRows) {
      next(new Error(`contact ${req.params.id} not found`))
    }

    res.writeHead(301, {
      location: '/'
    });

    res.end();
  } catch (err) {
    return next(err);
  }
});


export default router;
