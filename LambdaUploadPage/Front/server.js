// this package has sort of lightweight set of additions on the node of http
// make dealing with cookies a little bit easier
const express = require('express')
const { createReadStream } = require('fs') // this is the filesystem module
// those brackets is actually saying pulls createReadStream property from fs package

//this package is only to help parse the properties send from forms or to server
const bodyParser = require('body-parser')
const cookieParser = require('cookie-parser')

//creating a server
const app = express()
app.use(function(req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
});

app.use(express.static('public'));
app.use(bodyParser.urlencoded({ extended: false }))
app.use(cookieParser())

app.get('/', (req, res) => {
    createReadStream('index.html').pipe(res)
})

app.get('/test/image/resize', (req, res) => {
    createReadStream('resize-image.html').pipe(res)
})

app.listen(4000)
console.log('server is running on 4000')
