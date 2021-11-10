using System;

namespace tictactoe
{
    public partial class Game
    {
        // ENCAPSULATES HUMAN MOVE LOGIC
        
        bool HumanMove()
        {
            // process human player move

            // returns true if valid move made
            bool        validMove = false;

            if (priorHumanMove == 0)
            {
                // inform user a new game has started
                if (humanPlayer == Player.X)
                {
                    Console.WriteLine("This is new game. You go will first.");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"This is a new game. The Computer went first, taking square: {priorComputerMove}.");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"The Computer took square: {priorComputerMove}.");
                Console.WriteLine();
            }

            // prompt for user input
            Console.WriteLine("Enter C to change board size.");
            Console.WriteLine("Enter Q to quit.");
            
            // get user input
            while (! validMove)
            {
                int       boardSize = board.GetSize();
                int       cellCount = boardSize * boardSize;
                Console.Write($"Enter 1-{cellCount} to make a move: ");
                
                string  input = Console.ReadLine().Trim();
                if (input.Equals("Q", StringComparison.OrdinalIgnoreCase))
                {
                    programState = ProgramState.Quitting;
                    break;
                }
                else if (input.Equals("C", StringComparison.OrdinalIgnoreCase))
                {
                    programState = ProgramState.ChangingBoardSize;
                    break;
                }
                else
                {
                    // check for valid move
                    int     cellNumber;
                    validMove = int.TryParse(input, out cellNumber);
                    if (validMove && cellNumber >= 1 && cellNumber <= cellCount)
                    {
                        validMove = board.MakeMove(humanPlayer, cellNumber);
                        if (! validMove)
                        {
                            Console.WriteLine($"That square is already taken.");
                        }
                        else
                        {
                            priorHumanMove = cellNumber;
                        }
                    }
                }
            }

            return validMove;
        }
    }
}
