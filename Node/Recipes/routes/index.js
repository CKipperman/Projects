import express from 'express';
import pool from '../pool.js';

const router = express.Router();

router.route('/')
  .get(async (req, res, next) => {
    try {
      const [results] = await pool.execute(
        'SELECT id, name FROM recipes'
      );

      res.json(results);
    } catch (error) {
      return next(error);
    }
  })
  .post(async (req, res, next) => {
    try {
      const [results] = await pool.execute(
        'INSERT INTO recipes(name) VALUES(?)',
        [req.body.name]
      );

      req.body.id = results.insertId;

      res.status(201)
        .location(`/recipes-api/${req.body.id}`)
        .json(req.body);
    } catch (error) {
      return next(error);
    }
  });

router.route('/:id')
  .get(async (req, res, next) => {
    try {
      const [results] = await pool.execute(
        'SELECT id, name FROM recipes WHERE id = ?',
        [req.params.id]
      );

      if (!results.length) {
        res.statusCode = 404;
        return res.send(`Can't find recipe ${req.params.id}`);
      }

      res.json(results);
    } catch (error) {
      return next(error);
    }
  })
  .put(async (req, res, next) => {
    try {
      const [results] = await pool.execute(
        'UPDATE recipes SET name = ? WHERE id = ?',
        [req.body.name, req.params.id]
      );

      if (!results.affectedRows) {
        res.statusCode = 404;
        return res.send(`Can't find recipe ${req.params.id}`);
      }

      res.statusCode = 204;
      res.end();
    } catch (error) {
      return next(error);
    }
  })
  .delete(async (req, res, next) => {
    try {
      const [results] = await pool.execute(
        'DELETE FROM recipes WHERE id = ?',
        [req.params.id]
      );

      if (!results.affectedRows) {
        res.statusCode = 404;
        return res.send(`Can't find recipe ${req.params.id}`);
      }

      res.statusCode = 204;
      res.end();
    } catch (error) {
      return next(error);
    }
  });

export default router;
