using OthelloAStar.OthelloAStar;

namespace OthelloAStar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Othello!");

            OthelloGame game = new OthelloGame();
            //game.Board.SaveCurrentState(); // Save initial state
            Piece.Type currentPlayer = Piece.Type.White;

            OthelloAI ai = new OthelloAI(Piece.Type.Black); // Assuming AI is black

            while (game.Board.CheckWinner() == null)
            {
                game.Board.Print();
                
                Space aiMove = ai.GetBestMove(game.Board, Piece.Type.Black);
                if (aiMove != null)
                {
                    game.Board.MakeMove(aiMove, ai.AIColor);
                    game.Board.Print();
                    Console.WriteLine("AI played piece on row " + aiMove.Row + " and column " + aiMove.Column);
                    //Console.WriteLine("Press enter to start your turn");
                    //Console.ReadLine();
                    //game.Board.Spaces[aiMove.Row, aiMove.Column].Piece.Owner = ai.AIColor;
                }

                game.Board.Print();

                game.Board.CheckWinner();

                //Space move = ai.GetBestMove(game.Board, Piece.Type.White);
                Space move = game.GetMove(currentPlayer);

                if (game.Board.IsValidMove(move, currentPlayer))
                {
                    // Update the board and change turns
                    game.Board.MakeMove(move, currentPlayer); // Implement this method to place the piece and flip opponent's pieces
                    game.Board.Print();
                    //Console.WriteLine("Press enter to allow AI to play");
                    //Console.ReadLine();
                    //currentPlayer = (currentPlayer == Piece.Type.Black) ? Piece.Type.White : Piece.Type.Black; // Switch turns
                }
                else
                {
                    Console.WriteLine("Invalid move, try again.");
                }

                //game.Board.CheckWinner();
            }

            // print out winner
            Console.WriteLine("Winner!");
            game.Board.PrintScore();
            Console.ReadLine();

        }
    }
}
