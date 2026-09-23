var express = require('express');
var router = express.Router();

let contacts = [
  {
    id: 1,
    first: 'Donald',
    last: 'Trump',
    phone: '1234567890',
    email: 'dtrump@whitehouse.gov'
  },
  {
    id: 2,
    first: 'JD',
    last: 'Vance',
    phone: '9876543210',
    email: 'jd@whitehouse.gov'
  }
];

/* GET home page. */
router.get('/', function (req, res, next) {
  res.render('layout', {
    title: 'Contact List',
    contacts,
    noContacts: !contacts?.length,
    partials: { content: 'index.hjs' }
  });
});

router.get('/contacts', (req, res) => {
  res.json(contacts);
});

router.get('/contacts/:id', (req, res) => {
  const id = Number(req.params.id);
  const contact = contacts.find(c => c.id === id);

  if (!contact) {
    return res.status(404).json({
      error: "Contact not found",
      message: `No contact with ID ${id}`
    });
  }

  res.json(contact);
});

router.get('/addContact', (req, res, next) => {
  res.render('layout', {
    title: 'Add Contact',
    partials: { content: 'contactForm.hjs' }
  });
});

router.post('/addContact', (req, res, next) => {
  const newId = contacts.length > 0 
    ? Math.max(... contacts.map(c => c.id)) + 1
    : 1;

  const newContact = {
    id: newId,
    first: req.body.first,
    last: req.body.last,
    email: req.body.email,
    phone: req.body.phone
  };

  contacts.push(newContact);

  res.writeHead(301, {
    location: '/'
  });

  res.end();
});

router.post('/deleteContact/:id', (req, res, next) => {
  contacts = contacts.filter(c => c.id !== Number(req.params.id));

  res.writeHead(301, {
    location: '/'
  });

  res.end();
});

router.get('/editContact/:id', (req, res, next) => {
  const contact = contacts.find(c => c.id === Number(req.params.id));

    res.render('layout', {
      title: 'Edit Contact',
      contact: contact,
      partials: { content: 'contactForm.hjs'}
    });
});

router.post('/editContact/:id', (req, res, next) => {
  const contact = contacts.find(c => c.id === Number(req.params.id));

  contact.first = req.body.first;
  contact.last = req.body.last;
  contact.email = req.body.email;
  contact.phone = req.body.phone;

  res.writeHead(301, {
    location: '/'
  });

  res.end();
});

module.exports = router;
