# tictactoe

Description:
* This is a text version of the classic game.

To Run:
* run tictoc.exe, copied to top directory for convenience
* or build and run (in vscode, or "dotnet build" and "dotnet run")

Features:
* The grid starts as 3x3, but you can change this up to 20x20.
* The player mode is human vs computer.
* If a player wins, he shall go second the next game.

Requirements:
* This is a .NET 5.0 game. It requires the dotnet runtime installed.
* This uses the StdIn and StdOut console for user interface.
* This has been tested on Windows 10 Pro with .NET 5.0 SDK installed.

Bugs:
* The computer A.I. Minimax algorithm has a recursion problem, so the computer does a simple random move until fixed.
