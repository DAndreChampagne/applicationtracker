
const express = require('express');
const router = express.Router();


router.use((req,res,next) => {
    console.log(req.url);
    next();
});



router.get('/:controller/', (req,res) => {
    const controller = req.params.controller;
    res.send(DATA[controller]);
});

router.get('/:controller/:id', (req,res) => {
    const controller = req.params.controller;
    const id = req.params.id;

    res.send(DATA[controller].filter(x => x.Id == id));
});

router.put('/:controller/:id', (req,res) => {
    const controller = req.params.controller;
    const id = req.params.id;
    const item = req.body;

    if (id != item.Id) {
        res.statusMessage = `Id value from query parameter (${id}) does not match Id of object (${item.Id})`;
        res.send(400).end();
        return;
    }

    let index = DATA[controller].map(x => x.Id.toString()).indexOf(id);
    if (index === -1) {
        res.statusMessage = `Can't find object with Id of ${id}`;
        res.send(404).end();
        return;
    }

    DATA[controller][index] = item;
    res.jsonp(item);
});

router.post('/:controller/', (req,res) => {
    const controller = req.params.controller;
    const item = req.body;

    let newId = DATA[controller].map(x => x.Id).reduce((p, x) => x > p ? x : p);
    ++newId;

    item.Id = newId;
    DATA[controller].push(item);

    res.jsonp(item);
});

router.delete('/:controller/:id', (req,res) => {
    const controller = req.params.controller;
    const id = req.params.id;
    
    let index = DATA[controller].map(x => x.Id.toString()).indexOf(id);
    if (index === -1) {
        res.statusMessage = `Can't find object with Id of ${id}`;
        res.send(404).end();
        return;
    }

    let item = DATA[controller][index];
    DATA[controller].splice(index, 1);

    res.jsonp(DATA[controller]);
});


module.exports = router;




/*
    Test data until I set up a mongo and mysql server on my homelab.
*/

const JobType = {
    Unknown: 0,
    FullTime: 1,
    Contract: 2,
};

const ApplicationStatus = {
    Considering: 0,
    Applied: 1,
    Pending: 2,
    Declined: 3,
    Accepted: 4,
    RejectedWithoutInterview: 5,
    RejectedAfterInterview: 6,
};


const DATA = {
    "companies": [
        { Id: 1, Name: "GitHub" },
        { Id: 2, Name: "Healthcare Company" },
        { Id: 3, Name: "State Of The-State-Where-I-Currently-Live" },
    ],
    "contacts": [
        {
            Id: 1,
            Name: "Brian Recruiter",
            Email: "brian@company.com",
            Phone: "555-555-1234"
        }
    ],
    "applications": [
        {
            Id: 1,
            Link: "https://www.github.careers/jobs/2433?lang=en-us",
            CompanyId: 1,
            Company: null,
            ContactId: 1,
            Contact: null,
            Title: "Senior Software Engineer",
            Type: JobType.FullTime,
            Location: "Remote",
            MatchPercent: 60,
            SalaryMin: 104400,
            SalaryMax: 277000,
            DateApplied: new Date(2024, 1, 15),
            FollowUps: [
                new Date(2024, 1, 18),
                new Date(2024, 1, 20),
            ],
            ApplicationStatus: ApplicationStatus.RejectedWithoutInterview,
            ApplicationStatusReason: "None given",
            Notes: [
                "email rejection without reason given"
            ]
        }
    ]
};