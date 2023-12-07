using ChessGame.Global;
using ChessGame.Subsystems;
using System.Diagnostics;
using System.Text;

namespace ChessGame.StockFishAI
{
    internal class StockfishCommunicator
    {
        private Process stockfishProcess;
        private StreamWriter stockfishStreamWriter;
        private StreamReader stockfishStreamReader;

        public StockfishCommunicator(string stockfishPath)
        {
            stockfishProcess = new Process();
            stockfishProcess.StartInfo.FileName = stockfishPath;
            stockfishProcess.StartInfo.UseShellExecute = false;
            stockfishProcess.StartInfo.RedirectStandardInput = true;
            stockfishProcess.StartInfo.RedirectStandardOutput = true;
            stockfishProcess.StartInfo.CreateNoWindow = true;

            stockfishProcess.Start();

            stockfishStreamWriter = stockfishProcess.StandardInput;
            stockfishStreamReader = stockfishProcess.StandardOutput;
            string[] parameters = {
            "Write Debug Log false",
            "Contempt 0",
            "Min Split Depth 0",
            "Threads 8",
            "Ponder false",
            "Hash 16",
            "MultiPV 1",
            "Skill Level 20",
            "Move Overhead 30",
            "Minimum Thinking Time 40",
            "Slow Mover 80",
            "UCI_Chess960 false",
            "UCI_LimitStrength false",
            "UCI_Elo 2000"
        };
            foreach (var parameter in parameters)
            {
                SendCommand($"setoption name {parameter}");
            }
            string command = $"position fen {BoardToFEN()} moves {GameState.Moves.ToString()}";
            SendCommand(command);

            // end "go depth 5" command
            SendCommand("go depth 10");
            string moveString = ReadResponse();
            if (moveString.Length != 4)
            {
                throw new ArgumentException("Invalid move string format. It should be in the format 'fromSquareToSquare'.");
            }

            char fromFile = moveString[0];
            char fromRank = moveString[1];
            char toFile = moveString[2];
            char toRank = moveString[3];

            int fromRankIndex = char.ToUpper(fromRank) - '1'; // Assuming '1' is the lowest rank
            int fromFileIndex = char.ToUpper(fromFile) - 'A'; // Assuming 'A' is the leftmost file

            int toRankIndex = char.ToUpper(toRank) - '1';
            int toFileIndex = char.ToUpper(toFile) - 'A';

            Location fromLocation = new Location(fromRankIndex, fromFileIndex);
            Location toLocation = new Location(toRankIndex, toFileIndex);

            new Strategy.ShowMovesStrategy().processClick(fromLocation);
            new Strategy.MoveStrategy().processClick(toLocation);
            GameState.Moves.Append(" ");
            // GameNamespace.Game.GetInstance().MovePiece(fromLocation, toLocation);


        }

        public void SendCommand(string command)
        {
            stockfishStreamWriter.WriteLine(command);
            stockfishStreamWriter.Flush();
        }

        public string ReadResponse()
        {
            string bestMove = "";
            string line;
            while ((line = stockfishStreamReader.ReadLine()) != null)
            {

                // Check for the "bestmove" line
                if (line.StartsWith("bestmove"))
                {
                    // Extract the best move from the line
                    string[] parts = line.Split(' ');
                    if (parts.Length >= 2)
                    {
                        bestMove = parts[1];
                        //MessageBox.Show(bestMove);
                    }
                    break;
                }

            }
            return bestMove.Substring(0, 4);
        }
        public string BoardToFEN()
        {
            StringBuilder fenBuilder = new StringBuilder();
            int boardSize = 8;

            for (int rank = boardSize - 1; rank >= 0; rank--)
            {
                int emptySquareCount = 0;

                for (int file = 0; file < boardSize; file++)
                {
                    ChessGame.Global.IPiece piece = GameNamespace.Game.GetInstance().chessBoard.matrix[rank, file];

                    if (piece == null)
                    {
                        emptySquareCount++;
                    }
                    else
                    {
                        if (emptySquareCount > 0)
                        {
                            fenBuilder.Append(emptySquareCount);
                            emptySquareCount = 0;
                        }

                        fenBuilder.Append(GetFENSymbol(piece.PieceType, piece.PieceColor));
                    }
                }

                if (emptySquareCount > 0)
                {
                    fenBuilder.Append(emptySquareCount);
                }

                if (rank > 0)
                {
                    fenBuilder.Append('/');
                }
            }

            // Add other FEN components

            // Add turn (current side to move)
            fenBuilder.Append(" ");

            fenBuilder.Append("w");

            // Add castling availability
            fenBuilder.Append(" ");

            // Logic to determine castling availability based on your game state
            // Modify this part based on your specific implementation.
            string castlingAvailability = "KQkq"; // Assuming all castling is available for both sides initially.

            fenBuilder.Append(castlingAvailability);

            // Add en passant square
            fenBuilder.Append(" ");

            // Logic to determine en passant square based on your game state
            // If no en passant square, use "-".
            fenBuilder.Append("-");

            // Add halfmove clock and fullmove number
            fenBuilder.Append(" 0 1"); // Modify these values based on your specific implementation.

            return fenBuilder.ToString();
        }

        /*public string BoardToFEN()
        {
            StringBuilder fenBuilder = new StringBuilder();
            int boardSize = 8;

            for (int rank = 0; rank < boardSize; rank++)
            {
                int emptySquareCount = 0;

                for (int file = 0; file < boardSize; file++)
                {
                    ChessGame.Global.IPiece piece = GameNamespace.Game.GetInstance().chessBoard.matrix[rank, file];

                    if (piece == null)
                    {
                        emptySquareCount++;
                    }
                    else
                    {
                        if (emptySquareCount > 0)
                        {
                            fenBuilder.Append(emptySquareCount);
                            emptySquareCount = 0;
                        }

                        fenBuilder.Append(GetFENSymbol(piece.PieceType, piece.PieceColor));
                    }
                }

                if (emptySquareCount > 0)
                {
                    fenBuilder.Append(emptySquareCount);
                }

                if (rank < boardSize - 1)
                {
                    fenBuilder.Append('/');
                }
            }

            // Add other FEN components

            // Add turn (current side to move)
            fenBuilder.Append(" ");


            fenBuilder.Append("b");


            // Add castling availability
            fenBuilder.Append(" ");

            string castlingAvailability = "";

            // Add logic to determine castling availability based on your game state
            // For example, if kingside castling is available for white, and queenside castling is available for black, you would have "Kq" here.
            // If no castling is available, you can use "-".
            // Modify this part based on your specific implementation.

            fenBuilder.Append(castlingAvailability);

            // Add en passant square
            fenBuilder.Append(" ");

            // Add logic to determine en passant square based on your game state
            // If no en passant square, use "-".

            fenBuilder.Append("-");

            // Add halfmove clock and fullmove number
            fenBuilder.Append(" 0 1"); // Modify these values based on your specific implementation.

            return fenBuilder.ToString();
        }*/

        public char GetFENSymbol(PieceType pieceType, Color color)
        {
            bool IsWhite = color == Color.White;
            // Implement logic to determine the FEN symbol based on the type and color of the piece
            switch (pieceType)
            {
                case PieceType.King:
                    return IsWhite ? 'K' : 'k';
                case PieceType.Queen:
                    return IsWhite ? 'Q' : 'q';
                case PieceType.Rook:
                    return IsWhite ? 'R' : 'r';
                case PieceType.Bishop:
                    return IsWhite ? 'B' : 'b';
                case PieceType.Knight:
                    return IsWhite ? 'N' : 'n';
                case PieceType.Pawn:
                    return IsWhite ? 'P' : 'p';
                default:
                    throw new InvalidOperationException("Invalid piece type.");
            }
        }


        // Other methods...

        // Close the Stockfish process when the application is done
        public void Close()
        {
            stockfishStreamWriter.Close();
            stockfishStreamReader.Close();
            stockfishProcess.Close();
        }
    }
}

