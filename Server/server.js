const express = require('express');
const bodyParser = require('body-parser');
const mongoose = require('mongoose');
const app = express();
const http = require('http').Server(app);
const io = require('socket.io').listen(http);

app.use(bodyParser.urlencoded({extended: true}));
app.use(bodyParser.json());

var port = process.end.PORT || 80;
var server = app.listen(port, () => {
  console.log('server is running at port: ' + port);
});

var numOfPlayers = 0; // number of players online
var playerSocketId = []; // player socket id!
playerSocketId[0] = 0; // do not use 0, it is default

function getMyTeamSocketId(numOfPlayer) {
  if (numOfPlayer === 0) return 0;
  if ((numOfPlayer%2) === 0) return playerSocketId[numOfPlayer-1];
  else return playerSocketId[numOfPlayer+1];
}

io.on('connection', function(socket){  
  // 첫번째는 대기, 두번째 들어오면 ㄱㄱ
  socket.on('newPlayer', function(){
    numOfPlayers += 1;
    playerSocketId[numOfPlayers] = socket.id;
    console.log('newPlayer socket id is: ' + socket.id);
    // matched, and play
    if ((ifWait%2) === 0) {
      socket.emit('initialSetting', numOfPlayers);
      io.socket(getMyTeamSocketId(numOfPlayers)).emit('initialSetting');
    }
    // need to wait for another player
    console.log('player needs to wait');
    return socket.emit('waitPlayer', numOfPlayers); // client should save its player num.
  });

  // make the move
  socket.on('makeMove', function(numOfPlayer, whatMove){
    io.socket(getMyTeamSocketId(numOfPlayer)).emit('makeMove');
    socket.emit('makeMove');
  });

  // if the game ends
  socket.on('winner', function(numOfPlayer){
    io.socket(getMyTeamSocketId(numOfPlayer)).emit('youLost');
    socket.emit('youWon');
  });
});

