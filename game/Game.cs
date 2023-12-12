using ChessGame.Global;
using ChessGame.StockFishAI;
using ChessGame.Strategy;
using ChessGame.Subsystems;

namespace ChessGame.GameNamespace
{
    internal class Game
    {
        private static Game? Instance { get; set; }
        public Board ChessBoard { get; set; }
        public GameState GameState { get; set; }
        public Castling Castling { get; set; }
        public Sound SoundPlayer { get; set; }
        public static PictureBox[,]? ChessboardPictureBoxes { get; set; }
        public static StockfishCommunicator? Communicator { get; set; }
        private Game()
        {
            ChessBoard = new Board();
            GameState = new GameState();
            Castling = new Castling();
            SoundPlayer = new Sound();
            ChessboardPictureBoxes = new PictureBox[Static.NUMBER_OF_RANKS, Static.NUMBER_OF_FILES];
            Communicator = new StockfishCommunicator();
        }
        public static Game GetInstance()
        {
            Instance ??= new Game();
            return Instance;
        }
        public void UpdateImageAtLocation(Location location)
        {
            ChessboardPictureBoxes[location.Rank, location.File].Image = Static.LoadPieceImage(ChessBoard.Matrix[location.Rank, location.File]);
        }
        public void HandlePieceClick(Location clickLocation)
        {
            GUI.ResetSquareColors();
            PerformStrategy(clickLocation);
        }
        public void PerformStrategy(Location clickLocation)
        {
            if (GameState.GameStarted)
            {
                ClickStrategy? clickStrategy = GetStrategy(clickLocation);
                if (clickStrategy != null)
                    clickStrategy.ProcessClick(clickLocation);
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
            IPiece? clickedPiece = ChessBoard.GetPieceAt(clickLocation);

            if (clickedPiece != null && clickedPiece.PieceColor == GameState.PlayerTurnColor)
                return new ShowMovesStrategy();
            else if (GameState.ClickedPieceLocation.Rank != -1 && GameState.ClickedPieceLocation.File != -1)
                return new MoveStrategy();
            return null;
        }
        private void MoveRookBesideKing(Location kingLocation)
        {
            MovePiece(Castling.GetRookLocationFromCastlingSide(GameState.PlayerTurnColor, CastlingSide.KingSide), new Location(kingLocation.Rank, kingLocation.File + 1));
        }
        private void MoveRookBesideQueen(Location kingLocation)
        {
            MovePiece(Castling.GetRookLocationFromCastlingSide(GameState.PlayerTurnColor, CastlingSide.QueenSide), new Location(kingLocation.Rank, kingLocation.File - 1));
        }
        public void CheckAndHandleCastling(Location previouslyClickedPieceLocation, Location clickedLocation)
        {
            if (Castling.IsKingSideCastling(previouslyClickedPieceLocation, clickedLocation))
                MoveRookBesideKing(clickedLocation);
            else if (Castling.IsQueenSideCastling(previouslyClickedPieceLocation, clickedLocation))
                MoveRookBesideQueen(clickedLocation);
            SoundPlayer.PlayCastlingSound();
        }
        public void CheckAndHandlePawnPromotion(Location location)
        {

            if (location.Rank == 0 || location.Rank == Static.NUMBER_OF_RANKS - 1)
            {
                PieceType pieceType = GUI.ShowPromoteDialog();

                ChessBoard.Matrix[location.Rank, location.File] = PiecePromoter.CreatePiece(pieceType, location.Rank == 0 ? Color.Black : Color.White);
                UpdateImageAtLocation(location);
            }
        }
        public void LimitPiecesMovements(IPiece clickedPiece, List<Location> possibleMovements)
        {
            if (clickedPiece is not King)
                ChessBoard.RemoveInvalidMovesForCheck(possibleMovements, GameState.CheckingLocation, GameState.PlayerTurnColor == Color.White ? GameState.WhiteKingLocation : GameState.BlackKingLocation, GameState.PlayerTurnColor);
            else
                possibleMovements.RemoveAll(location => GetAllAttackedLocations().Contains(location));
        }

        public Image? GetPieceImageAt(int rank, int file)
        {
            IPiece? piece = ChessBoard.GetPieceAt(new Location(rank, file));
            return piece != null ? Static.LoadPieceImage(piece) : null;
        }
        public void MovePiece(Location currentLocation, Location newLocation)
        {
            IPiece? piece = ChessBoard.GetPieceAt(currentLocation);
            IPiece? piece1 = ChessBoard.GetPieceAt(newLocation);
            if (piece != null)
            {
                ChessBoard.RemovePieceAt(currentLocation);
                ChessBoard.AddPieceAt(piece, newLocation);

                UpdateImageAtLocation(currentLocation);
                UpdateImageAtLocation(newLocation);
            }
            if (GameState.PlayerTurnColor == Color.White)
            {
                GameState.Moves.Append(GetAlgebraicNotationFromLocation(currentLocation.Rank, currentLocation.File));
                GameState.Moves.Append(GetAlgebraicNotationFromLocation(newLocation.Rank, newLocation.File));
            }
            if (piece1 != null)
                SoundPlayer.PlayCaptureSound();
            else
                SoundPlayer.PlayMoveSound();

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
                    IPiece? piece = ChessBoard.Matrix[rank, file]!;
                    if (piece != null && piece.PieceColor != GameState.PlayerTurnColor)
                    {
                        if (piece is Pawn)
                            attackLocations.AddRange(((Pawn)piece).GetPawnAttackLocations(new Location(rank, file)));
                        else
                            attackLocations.AddRange(piece.GetAvailableMovesOnBoard(new Location(rank, file)));

                        attackLocations.RemoveAll(location => !Game.GetInstance().ChessBoard.IsMoveWithinBoard(location));
                    }
                }
            }
            return attackLocations;
        }

        public Color GetPlayerColor()
        {
            return GameState.PlayerTurnColor;
        }
        public void ShowAvailableMoves(List<Location> possibleMovements)
        {
            GUI.ColorLocations(possibleMovements, Color.FromArgb(255, 144, 238, 144)); //173, 216, 230    144, 238, 144     255, 255, 153
        }
        public void HandleCheck()
        {
            SoundPlayer.PlayCheckSound();
        }
        public void HandleCheckmate()
        {
            SoundPlayer.PlayCheckmateSound();
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
