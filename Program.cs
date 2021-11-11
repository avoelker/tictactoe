using System;

namespace tictactoe
{
    public enum ProgramState
    {
        Running,
        Quitting,
        ChangingBoardSize
    }

    public enum Player{
        Undefined,
        X,
        O
    }
    
    public class Program
    {
        static void Main(string[] args)
        {
            // RUN GAME LOOP

            try
            {
                new Game().GameLoop();
            }
            catch (Exception e)
            {
                if (e.Source != null)
                {
                    Console.WriteLine("Exception Thrown:");
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        public static string CenterString(string s, int width)
        {
            // UTILITY FUNCTION : center align a string

            if (s.Length >= width)
            {
                return s;
            }
            int padLeft = (width - s.Length) / 2;
            int padRight = width - s.Length - padLeft;
            return new string(' ', padLeft) + s + new string(' ', padRight);
        }
    }

    public partial class Game
    {
        // ENCAPSULATES PROCESSING THE GAME

        // compile constants
        static bool     useAI = true;                       // false = computer player selects random move
        static bool     humanStartsFirstInitially = true;   // false = computer player starts first initially
        static bool     loserStartsFirstNextGame = false;   // false = don't change player who starts first

        ProgramState    programState = ProgramState.Running;
        Board           board;                              // current state of game
        Player          activePlayer = Player.Undefined;
        Player          humanPlayer = humanStartsFirstInitially ? Player.X : Player.O;
        Player          computerPlayer = humanStartsFirstInitially ? Player.O : Player.X;
        int             priorHumanMove = 0;                 // prior move (cell number) of human player
        int             priorComputerMove = 0;              // prior move (cell numbrer) of computer player
        
        public void GameLoop()
        {
            StartNewGame();

            while (true)
            {
                if (programState == ProgramState.Quitting)
                {
                    break;      // exit the loop
                }
                else if (programState == ProgramState.ChangingBoardSize)
                {
                    ChangeBoardSize();
                }
                else    // if (programState == ProgramState.Running)
                {
                    // print the state of the game
                    board.Print();
                    if (humanPlayer == Player.X)
                    {
                        Console.WriteLine($"You are Player X. The Computer is Player O.");
                    }
                    else
                    {
                        Console.WriteLine($"The Computer is Player X. You are Player O.");
                    }
                    Console.WriteLine();

                    if (board.CheckGameOver())
                    {
                        Player      gameWinner = board.GetWinner();
                        if (gameWinner == Player.Undefined)
                        {
                            Console.WriteLine("The game is a draw!");
                            Console.WriteLine();
                            if (loserStartsFirstNextGame)
                            {
                                // next game, computer goes first
                                computerPlayer = Player.X;
                                humanPlayer = Player.O;
                            }
                        }
                        else if (gameWinner == humanPlayer)
                        {
                            Console.WriteLine("You won!");
                            Console.WriteLine();
                            if (loserStartsFirstNextGame)
                            {
                                // next game, human goes first
                                humanPlayer = Player.X;
                                computerPlayer = Player.O;
                            }
                        }
                        else
                        {
                            Console.WriteLine("The Computer won!");
                            Console.WriteLine();
                        }
                        Console.Write("Press Enter to play again.");
                        Console.ReadLine();

                        // start a new game
                        StartNewGame();
                    }
                    else if (activePlayer == humanPlayer)
                    {
                        // process human input/move
                        if (HumanMove())
                        {
                            // next move by computer player
                            activePlayer = computerPlayer;
                        }
                    }
                    else
                    {
                        // process computer move
                        if (ComputerMove())
                        {
                            // next move by human player
                            activePlayer = humanPlayer;
                        }
                    }
                }
            }
        }

        void ChangeBoardSize()
        {
            // process user changing size of board

            // prompt for user input
            int         minSize = Board.minGridSize;
            int         maxSize = Board.maxGridSize;
            int         size = board.GetSize();

            Console.WriteLine();
            Console.WriteLine($"The board size is currently: {size}x{size}.");
            Console.Write($"Enter new board size ({minSize}-{maxSize}): ");
            
            // get user input, check for valid board size
            string  input = Console.ReadLine().Trim();
            int     newSize;
            if (int.TryParse(input, out newSize))
            {
                if (newSize >= minSize && newSize <= maxSize)
                {
                    // update the board size, and start a new game
                    if (size != newSize)
                    {
                        size = newSize;
                        board.SetSize(size);
                        StartNewGame();
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Board size not changed.");
                        Console.Write("Press Enter to resume game.");
                        Console.ReadLine();
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid board size.");
                    Console.Write("Press Enter to resume game.");
                    Console.ReadLine();
                }
            }

            // resume running game loop
            programState = ProgramState.Running;
        }

        void StartNewGame()
        {
            // reset to a new game
            board = new Board(board is not null ? board.GetSize() : Board.minGridSize);
            activePlayer = Player.X;        // X always goes first
            priorHumanMove = 0;             // human has not acted
            priorComputerMove = 0;          // computer has not acted
        }
    }
}
