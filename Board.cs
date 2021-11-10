using System;
using System.Linq;
using System.Collections.Generic;

namespace tictactoe
{
    public class Board
    {
        // ENCAPSULATES STATE OF GAME BOARD

        public const int    minGridSize = 3;
        public const int    maxGridSize = 20;
        private bool        gameOver = false;
        private Player      gameWinner = Player.Undefined;
        private int         size;
        private Player[,]   board;
        
        public Board(int gridSize)
        {
            // constructor, creates default new Board

            if (gridSize < minGridSize)
            {
                gridSize = minGridSize;
            }
            else if (gridSize > maxGridSize)
            {
                gridSize = maxGridSize;
            }
            size = gridSize;
            Reset();
        }

        public Board(Board other)
        {
            // constructor, clones passed-in Board

            gameOver = other.gameOver;
            gameWinner = other.gameWinner;
            size = other.size;
            board = new Player[size,size];
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    board[r,c] = other.board[r,c];
                }
            }
        }

        public void Reset()
        {
            // reset Board to initial state

            gameOver = false;
            gameWinner = Player.Undefined;
            board = new Player[size,size];
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    board[r,c] = Player.Undefined;
                }
            }
        }

        public void SetSize(int gridSize)
        {
            // change size of board grid

            size = gridSize;
            Reset();
        }

        public int GetSize()
        {
            // get size of board grid

            return size;
        }


        public bool MakeMove(Player player, int cellNumber)
        {
            // process move (cell number) of a player (human or computer)

            // returns false, if cell already taken
            int         r = (cellNumber-1) / size;
            int         c = (cellNumber-1) % size;
            if (board[r,c] == Player.Undefined)
            {
                board[r,c] = player;
                return true;
            }
            return false;
        }

        public int[] GetAvailableMoves()
        {
            // return list of all available moves (cell numbers)

            List<int>   moves = new List<int>();
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (board[r,c] == Player.Undefined)
                    {
                        moves.Add(r * size + c + 1);
                    }
                }
            }
            return moves.ToArray();
        }

        public int GetRandomAvailableMove()
        {
            // return a random selected move (cell number) from all available

            int[]       moves = GetAvailableMoves();
            return moves[new Random().Next(moves.Length)];
        }

        public Player GetWinner()
        {
            // return winning player (if set, otherwise it will be Player.Undefined)

            // btw, a draw is when (gameOver && gameWinner == Player.Undefined)
            return gameWinner;
        }

        public bool CheckGameOver()
        {
            // determine if the game is over (a winner or draw is set)

            if (! gameOver)
            {
                Player      winner = CheckWinner();
                if (winner != Player.Undefined)
                {
                    gameWinner = winner;
                    gameOver = true;
                }
                else if (CheckNoMovesRemaining())
                {
                    gameWinner = Player.Undefined;
                    gameOver = true;
                }
            }
            return gameOver;
        }

        public Player CheckWinner()
        {
            // return the winning player, or Player.Undefined if not over or a draw

            Player      winner = CheckHorizWinner();
            if (winner == Player.Undefined)
            {
                winner = CheckVertWinner();
            }
            if (winner == Player.Undefined)
            {
                winner = CheckDiagLRWinner();
            }
            if (winner == Player.Undefined)
            {
                winner = CheckDiagRLWinner();
            }
            return winner;
        }

        private Player CheckHorizWinner()
        {
            // return winning player, if game over with horizontal win

            Player      player;
            for (int r = 0; r < size; r++)
            {
                player = board[r,0];
                if (player != Player.Undefined)
                {
                    bool        found = true;
                    for (int c = 1; c < size; c++)
                    {
                        if (board[r,c] != player)
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        return player;
                    }
                }
            }
            return Player.Undefined;
        }

        private Player CheckVertWinner()
        {
            // return winning player, if game over with vertical win
            
            Player      player;
            for (int c = 0; c < size; c++)
            {
                player = board[0,c];
                if (player != Player.Undefined)
                {
                    bool        found = true;
                    for (int r = 1; r < size; r++)
                    {
                        if (board[r,c] != player)
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        return player;
                    }
                }
            }
            return Player.Undefined;
        }

        private Player CheckDiagLRWinner()
        {
            // return winning player, if game over with diagonal (top-left to bottom-right) win
            
            Player      player = board[0,0];
            if (player != Player.Undefined)
            {
                bool        found = true;
                for (int r = 1; r < size; r++)
                {
                    if (board[r,r] != player)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return player;
                }
            }
            return Player.Undefined;
        }
        
        private Player CheckDiagRLWinner()
        {
            // return winning player, if game over with diagonal (top-right to bottom-left) win
            
            Player      player = board[0,size-1];
            if (player != Player.Undefined)
            {
                bool        found = true;
                for (int r = 1; r < size; r++)
                {
                    if (board[r,size-1-r] != player)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return player;
                }
            }
            return Player.Undefined;
        }

        public bool CheckNoMovesRemaining()
        {
            // return true, when all cells have been taken

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (board[r,c] == Player.Undefined)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Print()
        {
            // print board grid, in text characters

            const int   cellWidth = 7;
            string      topLine = new string('_', (cellWidth+1)*size-1);
            string      cellDivider = new string('_', cellWidth);
            string      rowDivider = string.Concat(Enumerable.Repeat($"|{cellDivider}", size)) + "|";
            string      cellSpacer = new string(' ', cellWidth);
            string      rowSpacer = string.Concat(Enumerable.Repeat($"|{cellSpacer}", size)) + "|";
            
            try
            {
                // when debugging, vscode is unhappy with this
                Console.Clear();
            }
            catch{}

            Console.WriteLine($"Tic Tac Toe (Player vs Computer, {size}x{size}):");
            Console.WriteLine();
            Console.WriteLine($" {topLine}");
            int cellNumber = 1;
            for (int r = 0; r < size; r++)
            {
                Console.WriteLine(rowSpacer);
                for (int c = 0; c < size; c++, cellNumber++)
                {
                    Player      cellPlayer = board[r,c];
                    string      cellLabel;
                    if (cellPlayer == Player.X)
                    {
                        cellLabel = "X";
                    }
                    else if (cellPlayer == Player.O)
                    {
                        cellLabel = "O";
                    }
                    else
                    {
                        cellLabel = cellNumber.ToString();
                    }
                    Console.Write("|"+Program.CenterString(cellLabel, cellWidth));
                }
                Console.WriteLine("|");
                Console.WriteLine(rowDivider);
            }
            Console.WriteLine();
        }
    }
}
