using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloAStar
{
    namespace OthelloAStar
    {
        internal class OthelloAI
        {
            private bool UseAlphaBetaPruning = true;
            private int MaxDepth = 7;
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            public Piece.Type AIColor { get; set; }

            public OthelloAI(Piece.Type color)
            {
                AIColor = color;
            }

            private int iterations = 0;

            public Space GetBestMove(Board board, Piece.Type player)
            {
                int bestScore = int.MinValue;
                Space bestMove = null;
                //Piece.Type player = board.GetCurrentPlayer();
                Console.WriteLine("AI is calculating best move for " + player + "...");
                // Iterate over all valid moves
                foreach (var move in board.GetValidMoves(player))
                {
                    // Check if the move is valid
                    if (board.IsValidMove(move, player)) // Ensure this function checks validity
                    {
                        // Make the move and flip the pieces
                        board.MakeMove(move, player); // Use the new method
                        //Console.WriteLine("Testing move in GetBestMove:");
                        //board.Print();
                        // Calculate the score of the board after the move
                        int score = Minimax(board, player, false, alpha, beta, 0);

                        // Undo the move
                        board.UndoMove(); // Call to undo the flipping and placement

                        //Console.WriteLine("Move undone, board is now");
                        //board.Print();
                        //board.Spaces[move.Row, move.Column].Piece.Owner = Piece.Type.None;

                        // If the score is better than the best found, update bestMove
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = move;
                        }
                    }
                }

                //board.MakeMove(bestMove, AIColor);
                return bestMove;
            }

            public int Minimax(Board board, Piece.Type player, bool maximizingPlayer, int alpha, int beta, int currentDepth)
            {
                iterations++;
                //if (iterations % 10_000_000 == 0 || currentDepth < 5)
                //{
                //    Console.WriteLine("Iterations: " + iterations);
                //    Console.WriteLine("Depth: " + currentDepth);
                //    Console.WriteLine("Evaluating board state: ");
                //    board.Print();
                //}
                // Check if the game is over
                if (IsGameOver(board) || currentDepth > MaxDepth)
                {
                    return EvaluateBoard(board, player);
                }

                // Get all possible moves for the current player
                var moves = board.GetValidMoves(player);

                // If no moves are available, return evaluation as it's essentially a terminal state
                if (moves.Count == 0)
                {
                    return EvaluateBoard(board, player);
                }

                if (maximizingPlayer)
                {
                    int maxEval = int.MinValue;

                    foreach (var move in moves)
                    {
                        // Apply the move
                        var clonedBoard = CloneBoard(board);
                        clonedBoard.MakeMove(move, player);

                        // Call minimax recursively
                        int eval = UseAlphaBetaPruning
                            ? Minimax(clonedBoard, player == Piece.Type.Black ? Piece.Type.White : Piece.Type.Black, false, alpha, beta, currentDepth + 1)
                            : Minimax(clonedBoard, player == Piece.Type.Black ? Piece.Type.White : Piece.Type.Black, false, int.MinValue, int.MaxValue, currentDepth + 1);

                        maxEval = Math.Max(maxEval, eval);

                        // If pruning is enabled, update alpha and check if we can prune
                        if (UseAlphaBetaPruning)
                        {
                            alpha = Math.Max(alpha, eval);
                            if (beta <= alpha)
                            {
                                //Console.WriteLine($"Pruning at depth {currentDepth}, alpha: {alpha}, beta: {beta}, eval: {eval}");
                                break; // Prune
                            }
                        }
                    }

                    return maxEval;
                }
                else
                {
                    int minEval = int.MaxValue;

                    foreach (var move in moves)
                    {
                        // Apply the move
                        var clonedBoard = CloneBoard(board);
                        clonedBoard.MakeMove(move, player);

                        // Call minimax recursively
                        int eval = UseAlphaBetaPruning
                            ? Minimax(clonedBoard, player == Piece.Type.Black ? Piece.Type.White : Piece.Type.Black, true, alpha, beta, currentDepth + 1)
                            : Minimax(clonedBoard, player == Piece.Type.Black ? Piece.Type.White : Piece.Type.Black, true, int.MinValue, int.MaxValue, currentDepth + 1);

                        minEval = Math.Min(minEval, eval);

                        // If pruning is enabled, update beta and check if we can prune
                        if (UseAlphaBetaPruning)
                        {
                            beta = Math.Min(beta, eval);
                            if (beta <= alpha)
                            {
                                //Console.WriteLine($"Pruning at depth {currentDepth}, alpha: {alpha}, beta: {beta}, eval: {eval}");
                                break; // Prune
                            }
                        }
                    }

                    return minEval;
                }
            }


            //public int Minimax(Board board, Piece.Type currentPlayer, int currentDepth)
            //{
            //    iterations++;
            //    //if (iterations % 10_000_000 == 0 || currentDepth < 49)
            //    //{
            //    //    Console.WriteLine("Iterations: " + iterations);
            //    //    Console.WriteLine("Depth: " + currentDepth);
            //    //    Console.WriteLine("Evaluating board state: ");
            //    //    board.Print();
            //    //}
            //    Piece.Type opponent = (currentPlayer == Piece.Type.Black) ? Piece.Type.White : Piece.Type.Black;

            //    // Base case: Evaluate the board if it's a terminal state (no more moves or game over)
            //    if (!board.HasValidMove(currentPlayer) || !board.HasValidMove(opponent))
            //    {
            //        //Console.WriteLine("Evaluating board state as " + board.EvaluateBoard(currentPlayer));
            //        return board.EvaluateBoard(currentPlayer); // Return evaluation for the current player
            //    }

            //    if (IsGameOver(board)) // Replace with your game-over check
            //    {
            //        Console.WriteLine("Game evaluated as complete: ");
            //        board.Print();
            //        return EvaluateBoard(board, currentPlayer);
            //    }

            //    int bestValue = (currentPlayer == Piece.Type.Black) ? int.MinValue : int.MaxValue;

            //    List<Space> validMoves = board.GetValidMoves(currentPlayer);

            //    // Generate all possible moves for the current player
            //    foreach (var move in validMoves)
            //    {
            //        // Make the move on a copy of the board
            //        Board newBoard = CloneBoard(board);
            //        newBoard.MakeMove(move, currentPlayer);

            //        //PrintCurrentDepth();
            //        //Console.WriteLine("Testing move in Minimax:");
            //        //newBoard.Print();

            //        //Console.ReadLine();
            //        // Recursively call Minimax for the opponent
            //        int value = Minimax(newBoard, opponent, currentDepth + 1);

            //        // Update best value based on the current player
            //        if (currentPlayer == Piece.Type.Black)
            //        {
            //            bestValue = Math.Max(bestValue, value); // Maximize for Black
            //        }
            //        else
            //        {
            //            bestValue = Math.Min(bestValue, value); // Minimize for White
            //        }
            //    }

            //    return bestValue;
            //}

            public Board CloneBoard(Board board)
            {
                Board newBoard = new Board();

                for (int row = 0; row < board.Spaces.GetLength(0); row++)
                {
                    for (int col = 0; col < board.Spaces.GetLength(1); col++)
                    {
                        Space originalSpace = board.Spaces[row, col];
                        Space clonedSpace = new Space
                        {
                            Row = originalSpace.Row,
                            Column = originalSpace.Column,
                            Piece = originalSpace.Piece != null ? new Piece(originalSpace.Piece.Owner) : null
                        };

                        newBoard.Spaces[row, col] = clonedSpace;
                    }
                }

                return newBoard;
            }

            // Call this method to print the current depth during each call
            //public void PrintCurrentDepth()
            //{
            //    Console.WriteLine($"Current Depth: {currentDepth}");
            //}

            public int EvaluateBoard(Board board, Piece.Type playerColor)
            {
                int score = 0;

                int playerCount = 0;
                int opponentCount = 0;
                int cornerControl = 0;

                // Count pieces and evaluate positions
                for (int row = 0; row < board.Spaces.GetLength(0); row++)
                {
                    for (int col = 0; col < board.Spaces.GetLength(1); col++)
                    {
                        if (board.Spaces[row, col].Piece.Owner == playerColor)
                        {
                            playerCount++;
                            score += 1; // Score for owning a piece
                        }
                        else if (board.Spaces[row, col].Piece.Owner != Piece.Type.None)
                        {
                            opponentCount++;
                            score -= 1; // Penalty for opponent pieces
                        }

                        // Check for corner control (corners are (0,0), (0,7), (7,0), (7,7))
                        if ((row == 0 && col == 0) || (row == 0 && col == 7) ||
                            (row == 7 && col == 0) || (row == 7 && col == 7))
                        {
                            if (board.Spaces[row, col].Piece.Owner == playerColor)
                            {
                                cornerControl += 10; // Higher score for corners
                            }
                            else if (board.Spaces[row, col].Piece.Owner != Piece.Type.None)
                            {
                                cornerControl -= 10; // Penalty for opponent owning a corner
                            }
                        }
                    }
                }

                // Add corner control score
                score += cornerControl;

                // Additional strategic considerations can be added here
                // For example, control of edges, mobility, or potential future moves

                return score;
            }



            //private int Minimax(Board board, int depth, bool isMaximizing)
            //{
            //    Piece.Type? winner = board.CheckWinner();
            //    if (winner == AIColor)
            //        return 1; // AI wins
            //    else if (winner == (AIColor == Piece.Type.Black ? Piece.Type.White : Piece.Type.Black))
            //        return -1; // Opponent wins
            //    else if (winner == null)
            //        return 0; // Tie

            //    if (isMaximizing)
            //    {
            //        int bestScore = int.MinValue;
            //        foreach (var move in board.GetValidMoves())
            //        {
            //            // Make the move
            //            board.Spaces[move.Row, move.Column].Piece.Owner = AIColor;

            //            // Recursively call Minimax
            //            int score = Minimax(board, depth + 1, false);

            //            // Undo the move
            //            board.Spaces[move.Row, move.Column].Piece.Owner = Piece.Type.None;

            //            bestScore = Math.Max(score, bestScore);
            //        }
            //        return bestScore;
            //    }
            //    else
            //    {
            //        int bestScore = int.MaxValue;
            //        foreach (var move in board.GetValidMoves())
            //        {
            //            // Make the move
            //            board.Spaces[move.Row, move.Column].Piece.Owner = (AIColor == Piece.Type.Black) ? Piece.Type.White : Piece.Type.Black;

            //            // Recursively call Minimax
            //            int score = Minimax(board, depth + 1, true);

            //            // Undo the move
            //            board.Spaces[move.Row, move.Column].Piece.Owner = Piece.Type.None;

            //            bestScore = Math.Min(score, bestScore);
            //        }
            //        return bestScore;
            //    }
            //}

            private bool IsGameOver(Board board)
            {
                // Check if the board is full
                if (IsBoardFull(board))
                {
                    return true; // Game is over because the board is full
                }

                // Check if both players have valid moves
                bool blackHasValidMove = HasValidMove(board, Piece.Type.Black);
                bool whiteHasValidMove = HasValidMove(board, Piece.Type.White);

                if (!blackHasValidMove && !whiteHasValidMove)
                {
                    return true; // Game is over because neither player has valid moves
                }

                return false; // Game is still ongoing
            }

            private bool IsBoardFull(Board board)
            {
                for (int row = 0; row < board.Spaces.GetLength(0); row++)
                {
                    for (int col = 0; col < board.Spaces.GetLength(1); col++)
                    {
                        if (board.Spaces[row, col].IsEmpty())
                        {
                            return false; // Found an empty space, the board is not full
                        }
                    }
                }
                return true; // No empty spaces found, the board is full
            }

            private bool HasValidMove(Board board, Piece.Type playerType)
            {
                foreach (var space in board.Spaces)
                {
                    if (board.IsValidMove(space, playerType))
                    {
                        return true; // Found a valid move for the player
                    }
                }
                return false; // No valid moves found for the player
            }

        }
    }

}
