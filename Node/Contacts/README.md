# PCS Contacts – Contact Management App

A contact management application built with Node.js and Express. This repository contains multiple versions showing the progression from in-memory storage to a real MySQL database.

## Versions

### Version 3 (Recommended) – MySQL + ES Modules
The most complete version. Contacts are stored in a MySQL database using a connection pool. Uses modern ES Modules.

**Key features:**
- Full CRUD with MySQL
- Parameterized queries (SQL injection safe)
- Connection pooling
- Environment variables for database credentials
- Server-side rendering with Hogan.js

### Version 1 & 2 – In-Memory Storage
Earlier practice versions that store contacts in a JavaScript array (data is lost when the server restarts). Useful for learning basic Express routing and form handling.

## Purpose

These projects were created to practice building a full CRUD application with Express, progressing from simple in-memory storage to a real database-backed application.

---

Created by: Chavie Kipperman  
Year: 2026