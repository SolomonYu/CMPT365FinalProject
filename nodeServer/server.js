var app = require('express')();
var http = require('http').createServer(app);
var io = require('socket.io')(http);

var portnum = 8080

var byteCount = 0;

setInterval(function(){
  console.log("Bytes in last 5 seconds: " + byteCount);
  byteCount = 0;
}, 5000);


app.get('/', function(req, res){
  res.send('<h1>Hello World!<h1>');
});

io.on('connection', function(socket){
  console.log('a user connected');
  socket.on('disconnect', function(){
      console.log('user disconnected');
  });
  socket.on('update', function(data){
    //console.log(data.x.length);
    byteCount += data.x.length/2 * 3;
    socket.broadcast.emit("receive", data);
  });
});

http.listen(portnum, function(){
  console.log('app is listening on port: ' + portnum);
});