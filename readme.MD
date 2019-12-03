Made by Solomon Yu for CMPT 365 at SFU in Fall 2019 semester.

The motivation for this project is to explore how the data being sent between a client and a server for a program, such as a game, can be reduced using compression methods. 

This project uses a Node.js server with Socket.IO to allow a Unity game to communicate with another instance of itself, and sending the position of 2 players back and forth.

To run this program:
This project was originally run from a Windows environment.
First, install node.js to open the server. https://nodejs.org/en/download/
Then, navigate to CMPT365FinalProject\nodeServer and run this command in the terminal: ‘node server.js’ and leave the terminal open. The logs will show here.
Navigate to CMPT365FinalProject\Builds and open 2 copies of CMPT365FinalProject.exe. You can customize the resolution here, and I recommend Windowed mode so you can see more windows. 

In the game, you can move with WASD and jump with space. There are also some interfaces where you can customize the options for compression. The game may have issues if the compression options are different, so they need to be set the same.

If you are opening this from the Unity editor, I am using is version 2019.3.0a5. You should be able to select this folder as the Unity folder.




