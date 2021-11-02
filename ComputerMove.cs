using System;
using System.Collections.Generic;

// How to PLay Tic-Tac-Toe:
// https://www.thesprucecrafts.com/tic-tac-toe-game-rules-412170

namespace tictactoe
{
    public class MoveScore
    {
        public const int    MinScore = -1000;
        public const int    NeutralScore = 0;
        public const int    MaxScore = 1000;
        
        private int         cellNumber;
        private int         score;

        public MoveScore(int cellNumber, int moveScore)
        {
            this.cellNumber = cellNumber;
            this.score = moveScore;
        }

        public int GetCellNumber()
        {
            return this.cellNumber;
        }

        public int GetScore()
        {
            return this.score;
        }
    }

    public partial class Game
    {
        bool ComputerMove()
        {
            // returns true if valid move made
            bool        validMove = false;

            int         cellNumber = GetNextMove(false);
            validMove = board.MakeMove(computerPlayer, cellNumber);
            if (validMove)
            {
                priorComputerMove = cellNumber;
            }
            else
            {
                Console.WriteLine($"Invalid computer move: {cellNumber}.");
                Console.Write("Press Enter to continue.");
                Console.ReadLine();
            }

            return validMove;
        }

        int GetNextMove(bool useAI)
        {
            // returns best cellNumber to move to next
            if (useAI)
            {
                // Minimax calculated move, from remaining squares
                return Minimax(board, computerPlayer).GetCellNumber();
            }
            else
            {
                // Randomly calculated move, from remaining squares
                return board.GetRandomAvailableMove();
            }
        }

        public static int   TEMP_Minimax_count = 0;

        public MoveScore Minimax(Board curBoard, Player curPlayer)
        {
            // returns best move available
            TEMP_Minimax_count++;
            System.Console.WriteLine($"Minimax calls: {TEMP_Minimax_count}");
            
            // return score, if at end of game
            Player      winner = curBoard.CheckWinner();
            if (winner == computerPlayer)
            {
                return new MoveScore(0, MoveScore.MaxScore);
            }
            else if (winner == humanPlayer)
            {
                return new MoveScore(0, MoveScore.MinScore);
            }
            else if (curBoard.CheckNoMovesRemaining())
            {
                return new MoveScore(0, MoveScore.NeutralScore);
            }

            int[]               possibleMoves = curBoard.GetAvailableMoves();
            List<MoveScore>     moveScoresList = new List<MoveScore>();
            for (int i = 0; i < possibleMoves.Length; i++)
            {
                int         cellNumber = possibleMoves[i];
                Board       moveBoard = new Board(curBoard);        // clone board
                moveBoard.MakeMove(curPlayer, cellNumber);

                if (curPlayer == computerPlayer)
                {
                    // recurse follow-on human moves
                    moveScoresList.Add(new MoveScore(cellNumber, Minimax(moveBoard, humanPlayer).GetScore()));
                }
                else // if (curPlayer == humanPlayer)
                {
                    // recurse follow-on computer moves
                    moveScoresList.Add(new MoveScore(cellNumber, Minimax(moveBoard, computerPlayer).GetScore()));
                }
            }
            MoveScore[]     moveScores = moveScoresList.ToArray();

            // return the move tried with the highest score
            MoveScore       bestMove = moveScores[0];
            if (curPlayer == computerPlayer)
            {
                // the best move of the computer results in the highest score
                int         highestScore = MoveScore.MinScore;
                for (int i = 0; i < moveScores.Length; i++)
                {
                    MoveScore       moveScore = moveScores[i];
                    if (highestScore < moveScore.GetScore())
                    {
                        highestScore = moveScore.GetScore();
                        bestMove = moveScore;
                    }
                }
            }
            else // if curPlayer == humanPlayer)
            {
                // the best move of the human results in the lowest score
                int         lowestScore = MoveScore.MaxScore;
                for (int i = 0; i < moveScores.Length; i++)
                {
                    MoveScore       moveScore = moveScores[i];
                    if (lowestScore > moveScore.GetScore())
                    {
                        lowestScore = moveScore.GetScore();
                        bestMove = moveScore;
                    }
                }
            }
            return bestMove;
        }
    }
}
