using ChessGame.Global;
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
        private void PerformStrategy(Location clickLocation)
        {
            ClickStrategy? clickStrategy = GetStrategy(clickLocation);
            if (clickStrategy != null)
                clickStrategy.processClick(clickLocation);
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
            GUI.ColorLocations(possibleMovements, Color.Gold);
        }
        public void HandleCheck()
        {
            soundPlayer.PlayCheckSound();
        }
        public void HandleCheckmate()
        {
            soundPlayer.PlayCheckmateSound();
            GUI.ShowCheckmateDialog();
        }
        public void HandlePromotionRequest()
        {
            GUI.ShowPromoteDialog();
        }
    }
}
