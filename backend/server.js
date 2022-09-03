const express = require('express')
const bodyParser = require('body-parser')

const app = express();
app.use(bodyParser.json())
const port = 8080;
app.get("/Hello",(req,res)=>{
    res.sendFile("hello.html",{root:'.'})
})

app.post('/timeOfLevels', (req,res) => {
    console.log(req.body)
    const timeCost = req.body['timeOfLevels'];
    console.log(timeCost);
})
app.listen(port, () => {
    console.log(`App running on PORT ${port}`);
});
