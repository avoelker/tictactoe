using System;

namespace tictactoe
{
    public enum GameState
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
            new Game().GameLoop();
        }

        public static string CenterString(string s, int width)
        {
            // center align a string
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
        GameState   gameState = GameState.Running;
        Player      activePlayer = Player.X;
        Player      humanPlayer = Player.X;     // default human goes first, in the first game
        Player      computerPlayer = Player.O;
        bool        newGame = true;
        int         priorComputerMove = 0;      // last move by computer player
        int         size = 3;                   // default is 3x3 grid
        Board       board;
        
        public void GameLoop()
        {
            StartNewGame();

            while (true)
            {
                if (gameState == GameState.Quitting)
                {
                    break;      // exit the loop
                }
                else if (gameState == GameState.ChangingBoardSize)
                {
                    ChangeBoardSize();
                }
                else    // if (gameState == GameState.Running)
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
                        }
                        else if (gameWinner == humanPlayer)
                        {
                            Console.WriteLine("You won!");
                            Console.WriteLine();
                            computerPlayer = Player.X;      // next game, loser goes first
                            humanPlayer = Player.O;
                        }
                        else
                        {
                            Console.WriteLine("The Computer won!");
                            Console.WriteLine();
                            humanPlayer = Player.X;         // next game, loser goes first
                            computerPlayer = Player.O;
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
                            activePlayer = computerPlayer;      // next move by computer
                        }
                    }
                    else
                    {
                        // process computer move
                        if (ComputerMove())
                        {
                            activePlayer = humanPlayer;         // next move by human
                        }
                    }
                }
            }
        }

        void ChangeBoardSize()
        {
            // prompt for user input
            int         minSize = Board.minGridSize;
            int         maxSize = Board.maxGridSize;
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
                    size = newSize;
                    StartNewGame();
                }
            }

            // resume running game loop
            gameState = GameState.Running;
        }

        void StartNewGame()
        {
            board = new Board(size);
            newGame = true;
        }
    }
}
