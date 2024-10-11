using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloAStar
{
    internal class OthelloGame
    {
        public Board Board { get; set; }
        public OthelloGame()
        {
            Board = new Board();
            //Board.SaveCurrentState(); // Save the initial state when the game starts
        }
        //public void InitGame()
        //{
        //    Board = new Board();
        //}

        //public bool IsComplete()
        //{
        //    return false;
        //}

        public Space GetMove(Piece.Type playerType)
        {
            while (true)
            {
                Console.WriteLine($"{(playerType == Piece.Type.Black ? "Black" : "White")}, enter your move (e.g., A1):");

                string input = Console.ReadLine().ToUpper(); // Read input and convert to uppercase
                if (input.Length < 2 || input.Length > 2)
                {
                    Console.WriteLine("Invalid input. Please enter a letter (A-H) followed by a number (1-8).");
                    continue;
                }

                char columnChar = input[0]; // First character is the column (A-H)
                char rowChar = input[1];     // Second character is the row (1-8)

                int col = columnChar - 'A';  // Convert letter to index (0-7)
                int row = rowChar - '1';      // Convert number to index (0-7)

                // Check if the row and column are within bounds
                if (row < 0 || row > 7 || col < 0 || col > 7)
                {
                    Console.WriteLine("Invalid move. Please enter a valid row (1-8) and column (A-H).");
                    continue;
                }

                Space selectedSpace = Board.Spaces[row, col]; // Get the selected space
                if (Board.IsValidMove(selectedSpace, playerType))
                {
                    return selectedSpace; // Valid move, return the selected space
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again.");
                }
            }
        }

    }
}
