using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloAStar
{
    internal class Board
    {
        // Board properties
        // Spaces (8x8)

        public Space[,] Spaces;

        private Stack<Board> history; // Stack to keep track of board states

        public Board()
        {
            Spaces = new Space[8, 8];
            history = new Stack<Board>();
            InitBoard();
            //SaveCurrentState();
        }

        public Piece.Type GetNextPlayer()
        {
            return GetCurrentPlayer() == Piece.Type.Black ? Piece.Type.White : Piece.Type.Black;
        }

        public Piece.Type GetCurrentPlayer()
        {
            int blackCount = 0;
            int whiteCount = 0;

            // Count pieces for each player
            for (int row = 0; row < Spaces.GetLength(0); row++)
            {
                for (int col = 0; col < Spaces.GetLength(1); col++)
                {
                    if (Spaces[row, col].Piece != null)
                    {
                        if (Spaces[row, col].Piece.Owner == Piece.Type.Black)
                        {
                            blackCount++;
                        }
                        else if (Spaces[row, col].Piece.Owner == Piece.Type.White)
                        {
                            whiteCount++;
                        }
                    }
                }
            }

            // Determine whose turn it is based on piece counts
            // You can adjust this logic to fit your game's rules
            if (blackCount > whiteCount)
            {
                return Piece.Type.White; // If Black has more pieces, it's White's turn
            }
            else
            {
                return Piece.Type.Black; // If White has more pieces or they are equal, it's Black's turn
            }
        }


        //public List<Space> GetValidMoves()
        //{
        //    List<Space> validMoves = new List<Space>();

        //    for (int row = 0; row < Spaces.GetLength(0); row++)
        //    {
        //        for (int col = 0; col < Spaces.GetLength(1); col++)
        //        {
        //            if (Spaces[row, col].IsEmpty()) // Assuming there's an IsEmpty() method in Space
        //            {
        //                validMoves.Add(Spaces[row, col]);
        //            }
        //        }
        //    }
        //    return validMoves;
        //}

        public List<Space> GetValidMoves(Piece.Type playerType)
        {
            List<Space> validMoves = new List<Space>();

            for (int row = 0; row < Spaces.GetLength(0); row++)
            {
                for (int col = 0; col < Spaces.GetLength(1); col++)
                {
                    Space currentSpace = Spaces[row, col];
                    if (currentSpace.IsEmpty() && IsValidMove(currentSpace, playerType))
                    {
                        validMoves.Add(currentSpace);
                    }
                }
            }
            return validMoves;
        }



        private void InitBoard()
        {
            // Fill the board with empty spaces initially
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Spaces[row, col] = new Space();
                    Spaces[row, col].Row = row;
                    Spaces[row, col].Column = col;
                }
            }

            // Set up the initial pieces in the center of the board
            Spaces[3, 3].Piece = new Piece(Piece.Type.White);
            Spaces[3, 4].Piece = new Piece(Piece.Type.Black);
            Spaces[4, 3].Piece = new Piece(Piece.Type.Black);
            Spaces[4, 4].Piece = new Piece(Piece.Type.White);
        }

        public int EvaluateBoard(Piece.Type playerColor)
        {
            int score = 0;

            for (int row = 0; row < Spaces.GetLength(0); row++)
            {
                for (int col = 0; col < Spaces.GetLength(1); col++)
                {
                    if (Spaces[row, col].Piece != null)
                    {
                        if (Spaces[row, col].Piece.Owner == playerColor)
                        {
                            score += 1; // Add points for player's pieces
                        }
                        else
                        {
                            score -= 1; // Subtract points for opponent's pieces
                        }
                    }
                }
            }

            return score;
        }

        public Piece.Type? CheckWinner()
        {
            int blackCount = 0;
            int whiteCount = 0;

            // Count pieces for each player
            for (int row = 0; row < Spaces.GetLength(0); row++)
            {
                for (int col = 0; col < Spaces.GetLength(1); col++)
                {
                    if (Spaces[row, col].Piece != null)
                    {
                        if (Spaces[row, col].Piece.Owner == Piece.Type.Black)
                        {
                            blackCount++;
                        }
                        else if (Spaces[row, col].Piece.Owner == Piece.Type.White)
                        {
                            whiteCount++;
                        }
                    }
                }
            }

            // Check if the board is full or no valid moves are available for both players
            if (IsBoardFull() || !HasValidMove(Piece.Type.Black) && !HasValidMove(Piece.Type.White))
            {
                if (blackCount > whiteCount)
                {
                    return Piece.Type.Black;
                }
                else if (whiteCount > blackCount)
                {
                    return Piece.Type.White;
                }
                else
                {
                    return null; // Null indicates a tie
                }
            }

            // No winner yet
            return null;
        }

        public bool HasValidMove(Piece.Type playerType)
        {
            // Placeholder logic: implement the actual check for valid moves for the player
            foreach (var space in Spaces)
            {
                // Replace with logic to check if placing a piece for 'playerType' is valid
                if (IsValidMove(space, playerType))
                {
                    return true;
                }
            }
            return false;
        }

        //public bool IsValidMove(Space space, Piece.Type playerType)
        //{
        //    if (!space.IsEmpty())
        //    {
        //        return false; // The space must be empty
        //    }

        //    int row = space.Row; // Assuming Space has properties for Row and Column
        //    int col = space.Column;

        //    Piece.Type opponentType = (playerType == Piece.Type.Black) ? Piece.Type.White : Piece.Type.Black;

        //    bool byOpponentPiece = CheckDirection(row, col, 1, 0, playerType, opponentType) || // Right
        //           CheckDirection(row, col, -1, 0, playerType, opponentType) || // Left
        //           CheckDirection(row, col, 0, 1, playerType, opponentType) || // Down
        //           CheckDirection(row, col, 0, -1, playerType, opponentType) || // Up
        //           CheckDirection(row, col, 1, 1, playerType, opponentType) || // Down-Right
        //           CheckDirection(row, col, -1, -1, playerType, opponentType) || // Up-Left
        //           CheckDirection(row, col, 1, -1, playerType, opponentType) || // Down-Left
        //           CheckDirection(row, col, -1, 1, playerType, opponentType); // Up-Right

        //    // Check all eight directions for valid moves
        //    return byOpponentPiece;
        //}

        public bool IsValidMove(Space? space, Piece.Type playerType)
        {
            if (space is null || !space.IsEmpty())
            {
                return false; // The space must be empty
            }

            int row = space.Row; // Assuming Space has properties for Row and Column
            int col = space.Column;

            Piece.Type opponentType = (playerType == Piece.Type.Black) ? Piece.Type.White : Piece.Type.Black;

            bool byOpponentPiece = CheckDirection(row, col, 1, 0, playerType, opponentType) || // Right
                   CheckDirection(row, col, -1, 0, playerType, opponentType) || // Left
                   CheckDirection(row, col, 0, 1, playerType, opponentType) || // Down
                   CheckDirection(row, col, 0, -1, playerType, opponentType) || // Up
                   CheckDirection(row, col, 1, 1, playerType, opponentType) || // Down-Right
                   CheckDirection(row, col, -1, -1, playerType, opponentType) || // Up-Left
                   CheckDirection(row, col, 1, -1, playerType, opponentType) || // Down-Left
                   CheckDirection(row, col, -1, 1, playerType, opponentType); // Up-Right

            // Check all eight directions for valid moves
            return byOpponentPiece;
        }


        private bool CheckDirection(int row, int col, int rowIncrement, int colIncrement, Piece.Type playerType, Piece.Type opponentType)
        {
            int currentRow = row + rowIncrement;
            int currentCol = col + colIncrement;
            bool hasOpponentPieces = false;

            while (IsInBounds(currentRow, currentCol))
            {
                // Check if the space is empty
                if (Spaces[currentRow, currentCol].IsEmpty())
                {
                    return false; // Encountered an empty space, not a valid direction
                }

                // Check if it's an opponent piece
                if (Spaces[currentRow, currentCol].Piece.Owner == opponentType)
                {
                    hasOpponentPieces = true; // Found an opponent piece
                }
                // Check if it's the player's piece
                else if (Spaces[currentRow, currentCol].Piece.Owner == playerType)
                {
                    return hasOpponentPieces; // Only valid if we've encountered opponent pieces before this
                }
                else
                {
                    return false; // If it's neither the opponent's piece nor the player's, something is wrong
                }

                // Move to the next space in the direction
                currentRow += rowIncrement;
                currentCol += colIncrement;
            }

            return false; // Out of bounds or didn't find a valid sequence ending
        }


        //private bool CheckDirection(int row, int col, int rowIncrement, int colIncrement, Piece.Type playerType, Piece.Type opponentType)
        //{
        //    int currentRow = row + rowIncrement;
        //    int currentCol = col + colIncrement;
        //    bool hasOpponentPieces = false;

        //    while (IsInBounds(currentRow, currentCol))
        //    {
        //        if (Spaces[currentRow, currentCol].IsEmpty())
        //        {
        //            return false; // Encountered an empty space
        //        }

        //        if (Spaces[currentRow, currentCol].Piece.Owner == opponentType)
        //        {
        //            hasOpponentPieces = true; // Found an opponent piece
        //        }
        //        else if (Spaces[currentRow, currentCol].Piece.Owner == playerType)
        //        {
        //            return hasOpponentPieces; // We found a friendly piece after opponent pieces
        //        }

        //        currentRow += rowIncrement;
        //        currentCol += colIncrement;
        //    }

        //    return false; // Out of bounds or didn't find a valid ending
        //}

        private bool IsInBounds(int row, int col)
        {
            return row >= 0 && row < 8 && col >= 0 && col < 8; // Assuming an 8x8 board
        }


        private bool IsBoardFull()
        {
            for (int row = 0; row < Spaces.GetLength(0); row++)
            {
                for (int col = 0; col < Spaces.GetLength(1); col++)
                {
                    if (Spaces[row, col].IsEmpty())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void MakeMove(Space space, Piece.Type playerType)
        {
            if (!IsValidMove(space, playerType))
            {
                Console.WriteLine(playerType + " playing move with row " + space.Row + " and col " + space.Column + " on board:");
                Print();
                Console.WriteLine("Invalid");
                Console.ReadLine();
                return; // Validate move before proceeding
            }

            SaveCurrentState();

            // Place the piece
            this.Spaces[space.Row, space.Column].Piece = new Piece(playerType);
            //MakeMove(space, playerType);

            // Flip opponent pieces in all directions
            int row = space.Row;
            int col = space.Column;

            Piece.Type opponentType = (playerType == Piece.Type.Black) ? Piece.Type.White : Piece.Type.Black;

            // Check all eight directions for flipping pieces
            FlipDirection(row, col, 1, 0, playerType, opponentType); // Right
            FlipDirection(row, col, -1, 0, playerType, opponentType); // Left
            FlipDirection(row, col, 0, 1, playerType, opponentType); // Down
            FlipDirection(row, col, 0, -1, playerType, opponentType); // Up
            FlipDirection(row, col, 1, 1, playerType, opponentType); // Down-Right
            FlipDirection(row, col, -1, -1, playerType, opponentType); // Up-Left
            FlipDirection(row, col, 1, -1, playerType, opponentType); // Down-Left
            FlipDirection(row, col, -1, 1, playerType, opponentType); // Up-Right
        }

        private void FlipDirection(int row, int col, int rowIncrement, int colIncrement, Piece.Type playerType, Piece.Type opponentType)
        {
            int currentRow = row + rowIncrement;
            int currentCol = col + colIncrement;
            List<Space> toFlip = new List<Space>();

            while (IsInBounds(currentRow, currentCol))
            {
                if (Spaces[currentRow, currentCol].IsEmpty())
                {
                    return; // Encountered an empty space, stop
                }
                if (Spaces[currentRow, currentCol].Piece.Owner == opponentType)
                {
                    toFlip.Add(Spaces[currentRow, currentCol]); // Mark opponent pieces for flipping
                }
                else if (Spaces[currentRow, currentCol].Piece.Owner == playerType)
                {
                    // Flip all marked pieces if we found a friendly piece
                    foreach (var space in toFlip)
                    {
                        space.Piece.Owner = playerType; // Change the owner to the current player
                        space.Piece = new Piece(playerType);
                    }
                    return;
                }

                currentRow += rowIncrement;
                currentCol += colIncrement;
            }
        }

        public void SaveCurrentState()
        {
            // Save a deep copy of the current board
            Board currentState = new Board();

            // Copy the pieces from the current board
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    currentState.Spaces[row, col].Piece = new Piece(Spaces[row, col].Piece.Owner);
                    currentState.Spaces[row, col].Row = row;
                    currentState.Spaces[row, col].Column = col;
                }
            }

            //Console.WriteLine("Saving current state:");
            //currentState.Print();

            history.Push(currentState);
        }

        //public void UndoMove()
        //{
        //    if (history.Count == 0)
        //    {
        //        throw new InvalidOperationException("No moves to undo.");
        //    }
        //    Console.WriteLine("Restoring state:");
        //    Print();
        //    Console.WriteLine(" to state:");
        //    // Restore the last board state
        //    Board lastState = history.Pop();
        //    lastState.Print();
        //    //Console.ReadLine();
        //    Spaces = lastState.Spaces; // Assuming deep copy was done
        //}

        public void UndoMove()
        {
            if (history.Count > 0)
            {
                // Restore the previous state from the history stack
                Board previousBoardState = history.Pop();

                // Restore each space to the previous state
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        // Restore the owner of the piece or set to None if it's empty
                        if (previousBoardState.Spaces[row, col].Piece != null)
                        {
                            Spaces[row, col].Piece.Owner = previousBoardState.Spaces[row, col].Piece.Owner;
                        }
                        else
                        {
                            Spaces[row, col].Piece.Owner = Piece.Type.None; // Set to None if it was empty
                        }
                    }
                }

                // Optionally, you might want to print or log the board state after undoing
                //Console.WriteLine("Move undone. Restored previous state.");
                //Print(); // Print the board state after undoing
            }
            else
            {
                Console.WriteLine("No moves to undo.");
            }
        }

        public void Print()
        {
            //Console.Clear();
            Console.WriteLine("  A B C D E F G H");

            for (int row = 0; row < 8; row++)
            {
                Console.Write(row + 1 + " ");
                for (int col = 0; col < 8; col++)
                {
                    char displayChar = Spaces[row, col].IsEmpty() ? '.' :
                                       Spaces[row, col].Piece.Owner == Piece.Type.Black ? 'B' : 'W';
                    Console.Write(displayChar + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            //Console.ReadLine();
            //Thread.Sleep(3000); // Delay for 3 seconds
        }

        internal void PrintScore()
        {
            int blackPieceCount = 0;
            int whitePieceCount = 0;
            for(int row = 0;row < 8; row++)
            {
                for (int col = 0;col < 8; col++)
                {
                    if (Spaces[row, col].Piece.Owner == Piece.Type.Black)
                    {
                        blackPieceCount++;
                    }
                    if (Spaces[row, col].Piece.Owner == Piece.Type.White)
                    {
                        whitePieceCount++;
                    }
                }
            }
            Console.WriteLine("Black piece count: " + blackPieceCount);
            Console.WriteLine("White piece count: " + whitePieceCount);
        }
    }
}
