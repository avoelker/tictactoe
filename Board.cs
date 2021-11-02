using System;
using System.Linq;
using System.Collections.Generic;

namespace tictactoe
{
    public class Board
    {
        public const int    minGridSize = 3;
        public const int    maxGridSize = 20;
        private bool        gameOver = false;
        private Player      gameWinner = Player.Undefined;
        private int         size;
        private Player[,]   board;
        
        public Board(int gridSize)
        {
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
            size = gridSize;
            Reset();
        }

        public int GetSize()
        {
            return size;
        }


        public bool MakeMove(Player player, int cellNumber)
        {
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
            // returns list of all available moves
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
            // returns cell number of randomly selected available cell (1 to size*size)
            int[]       moves = GetAvailableMoves();
            return moves[new Random().Next(moves.Length)];
        }

        public Player GetWinner()
        {
            // btw, a draw is when (gameOver && gameWinner == Player.Undefined)
            return gameWinner;
        }

        public bool CheckGameOver()
        {
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
            // returns winner or Player.Undefined for no winner yet
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
            // return horizontal winner
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
            // return vertical winner
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
            // return diagonal (top-left to bottom-right) winner
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
            // return diagonal (top-right to bottom-left)  winner
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
            // return true if no moves are remaining 
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
            const int   cellWidth = 7;
            string      topLine = new string('_', (cellWidth+1)*size-1);
            string      cellDivider = new string('_', cellWidth);
            string      rowDivider = string.Concat(Enumerable.Repeat($"|{cellDivider}", size)) + "|";
            string      cellSpacer = new string(' ', cellWidth);
            string      rowSpacer = string.Concat(Enumerable.Repeat($"|{cellSpacer}", size)) + "|";
            
            try
            {
                Console.Clear();
            }
            catch{}
            Console.WriteLine($"Tic Tac Toe (Player vs Computer, {size}x{size}):\n");
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
