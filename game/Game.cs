using ChessGame.Global;
using ChessGame.StockFishAI;
using ChessGame.Strategy;
using ChessGame.Subsystems;

namespace ChessGame.GameNamespace
{
    internal class Game
    {
        private static Game? instance;
        public Board chessBoard;
        public GameState gameState;
        public Castling castling;
        public Sound soundPlayer;
        public static PictureBox[,] chessboardPictureBoxes = new PictureBox[Static.NUMBER_OF_RANKS, Static.NUMBER_OF_FILES];
        private Game()
        {
            chessBoard = new Board();
            gameState = new GameState();
            castling = new Castling();
            soundPlayer = new Sound();
        }
        public static Game GetInstance()
        {
            instance ??= new Game();
            return instance;
        }
        public void UpdateImageAtLocation(Location location)
        {
            chessboardPictureBoxes[location.Rank, location.File].Image = Static.LoadPieceImage(chessBoard.matrix[location.Rank, location.File]);
        }
        public void HandlePieceClick(Location clickLocation)
        {
            GUI.ResetSquareColors();
            PerformStrategy(clickLocation);
        }
        public void PerformStrategy(Location clickLocation)
        {
            ClickStrategy? clickStrategy = GetStrategy(clickLocation);
            bool AIenabled = true;
            if (clickStrategy != null)
            {
                if (clickStrategy is ShowMovesStrategy && AIenabled == true && gameState.playerTurnColor == Color.White)
                {
                    string stockfishPath = "../../../../stockfish/stockfish-windows-x86-64-avx2.exe";

                    StockfishCommunicator communicator = new StockfishCommunicator(stockfishPath);
                }
                else
                    clickStrategy.processClick(clickLocation);
            }

        }

        private string ExtractBestMove(string response)
        {
            // Split the response into lines
            string[] lines = response.Split('\n');

            // Search for the line starting with "bestmove"
            foreach (string line in lines)
            {
                if (line.StartsWith("bestmove"))
                {
                    // Extract the best move from the line
                    string[] parts = line.Split(' ');
                    if (parts.Length >= 2)
                    {
                        // Return the second part, which is the best move
                        return parts[1];
                    }
                }
            }

            // If no "bestmove" line is found, return an empty string or handle it as needed
            return string.Empty;
        }
        private ClickStrategy? GetStrategy(Location clickLocation)
        {
            IPiece? clickedPiece = chessBoard.GetPieceAt(clickLocation);

            if (clickedPiece != null && clickedPiece.PieceColor == gameState.playerTurnColor)
                return new ShowMovesStrategy();
            else if (gameState.clickedPieceLocation.Rank != -1 && gameState.clickedPieceLocation.File != -1)
                return new MoveStrategy();
            return null;
        }
        private void MoveRookBesideKing(Location kingLocation)
        {
            MovePiece(castling.GetRookLocationFromCastlingSide(gameState.playerTurnColor, CastlingSide.KingSide), new Location(kingLocation.Rank, kingLocation.File + 1));
        }
        private void MoveRookBesideQueen(Location kingLocation)
        {
            MovePiece(castling.GetRookLocationFromCastlingSide(gameState.playerTurnColor, CastlingSide.QueenSide), new Location(kingLocation.Rank, kingLocation.File - 1));
        }
        public void CheckAndHandleCastling(Location previouslyClickedPieceLocation, Location clickedLocation)
        {
            if (castling.IsKingSideCastling(previouslyClickedPieceLocation, clickedLocation))
                MoveRookBesideKing(clickedLocation);
            else if (castling.IsQueenSideCastling(previouslyClickedPieceLocation, clickedLocation))
                MoveRookBesideQueen(clickedLocation);
        }
        public void CheckAndHandlePawnPromotion(Location location)
        {

            if (location.Rank == 0 || location.Rank == Static.NUMBER_OF_RANKS - 1)
            {
                PieceType pieceType = GUI.ShowPromoteDialog();

                chessBoard.matrix[location.Rank, location.File] = PiecePromoter.CreatePiece(pieceType, location.Rank == 0 ? Color.Black : Color.White);
                UpdateImageAtLocation(location);
            }
        }
        public void LimitPiecesMovements(IPiece clickedPiece, List<Location> possibleMovements)
        {
            if (clickedPiece is not King)
                chessBoard.RemoveInvalidMovesForCheck(possibleMovements, gameState.checkingLocation, gameState.playerTurnColor == Color.White ? gameState.whiteKingLocation : gameState.blackKingLocation, gameState.playerTurnColor);
            else
                possibleMovements.RemoveAll(location => GetAllAttackedLocations().Contains(location));
        }

        public Image? GetPieceImageAt(int rank, int file)
        {
            IPiece? piece = chessBoard.GetPieceAt(new Location(rank, file));
            return piece != null ? Static.LoadPieceImage(piece) : null;
        }
        public void MovePiece(Location currentLocation, Location newLocation)
        {
            soundPlayer.PlayMoveSound();
            IPiece? piece = chessBoard.GetPieceAt(currentLocation);

            if (piece != null)
            {
                chessBoard.RemovePieceAt(currentLocation);
                chessBoard.AddPieceAt(piece, newLocation);

                UpdateImageAtLocation(currentLocation);
                UpdateImageAtLocation(newLocation);
            }
            if (gameState.playerTurnColor == Color.White)
            {
                GameState.Moves.Append(GetAlgebraicNotationFromLocation(currentLocation.Rank, currentLocation.File));
                GameState.Moves.Append(GetAlgebraicNotationFromLocation(newLocation.Rank, newLocation.File));

            }
        }
        string GetAlgebraicNotationFromLocation(int rank, int file)
        {
            // Assuming rank and file are 0-indexed
            char fileChar = (char)('a' + file);
            int rankNumber = rank + 1; // Reverse the rank to match standard chess notation
            return $"{fileChar}{rankNumber}";
        }
        public List<Location> GetAllAttackedLocations()
        {
            List<Location> attackLocations = new();
            for (int rank = 0; rank < Static.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Static.NUMBER_OF_FILES; file++)
                {
                    IPiece? piece = chessBoard.matrix[rank, file]!;
                    if (piece != null && piece.PieceColor != gameState.playerTurnColor)
                    {
                        if (piece is Pawn)
                            attackLocations.AddRange(((Pawn)piece).GetPawnAttackLocations(new Location(rank, file)));
                        else
                            attackLocations.AddRange(piece.GetAvailableMovesOnBoard(new Location(rank, file)));

                        attackLocations.RemoveAll(location => !Game.GetInstance().chessBoard.IsMoveWithinBoard(location));
                    }
                }
            }
            return attackLocations;
        }

        public Color GetPlayerColor()
        {
            return gameState.playerTurnColor;
        }
        public void ShowAvailableMoves(List<Location> possibleMovements)
        {
            GUI.ColorLocations(possibleMovements, Color.FromArgb(255, 144, 238, 144)); //173, 216, 230    144, 238, 144     255, 255, 153
        }
        public void HandleCheck()
        {
            soundPlayer.PlayCheckSound();
        }
        public void HandleCheckmate()
        {
            soundPlayer.PlayCheckmateSound();
            GUI.ShowCheckmateDialog();
            Application.Restart();
            Environment.Exit(0);
        }
        public void HandlePromotionRequest()
        {
            GUI.ShowPromoteDialog();
        }
    }
}
