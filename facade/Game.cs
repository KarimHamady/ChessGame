using ChessGame.Global;
using ChessGame.Logic.BoardNamespace;
using ChessGame.Logic.CastleNamespace;
using ChessGame.Logic.GameStateNamespace;
using ChessGame.Logic.PieceNamespace;
using ChessGame.Statics;
using ChessGame.strategy;

namespace ChessGame.GameNamespace
{
    internal class Game
    {
        private static Game? instance;
        public Board chessBoard;
        public GameState gameState;
        public Castling castling;
        public static PictureBox[,] chessboardPictureBoxes = new PictureBox[Static.NUMBER_OF_RANKS, Static.NUMBER_OF_FILES];
        private Game()
        {
            chessBoard = new Board();
            gameState = new GameState();
            castling = new Castling();
        }
        public static Game GetInstance()
        {
            if (instance == null)
                instance = new Game();
            return instance;
        }
        public void UpdateImageAtLocation(Location location)
        {
            chessboardPictureBoxes[location.Rank, location.File].Image = LoadPieceImage(chessBoard.matrix[location.Rank, location.File]);
        }
        public void RemoveImageAtLocation(Location location)
        {
            chessboardPictureBoxes[location.Rank, location.File].Image = null;
        }
        public Image? LoadPieceImage(Piece? piece)
        {
            if (piece == null)
                return null;

            char colorPrefix = piece.pieceColor == Color.White ? 'w' : 'b';
            String? name = Enum.GetName(typeof(Statics.PieceType), piece.pieceType);
            string imagePath = $"../../../data/{(name != null ? name.ToLower() : "")}_{colorPrefix}.png";

            return File.Exists(imagePath) ? Image.FromFile(imagePath) : null;
        }
        public static /*Label*/ void HandleCheckMate()
        {
            // TODO: what?
        }
        public void HandlePieceClick(Location clickLocation)
        {
            GUI.ResetSquareColors();
            ClickStrategy clickStrategy = GetStrategy(clickLocation);
            if (clickStrategy != null)
                clickStrategy.processClick(clickLocation);
        }

        private ClickStrategy? GetStrategy(Location clickLocation)
        {
            Piece? clickedPiece = chessBoard.GetPieceAt(clickLocation);

            if (clickedPiece != null && clickedPiece.pieceColor == gameState.playerTurnColor)
                return new ClickBeforeMoveStrategy();
            else if (gameState.clickedLocation.Rank != -1 && gameState.clickedLocation.File != -1)
                return new ClickAfterMoveStrategy();
            return null;
        }
        private void MoveRookBesideKing(Location kingLocation)
        {
            MovePieceFromSquareToSquare(castling.GetRookLocationFromCastlingSide(gameState.playerTurnColor, CastlingSide.KingSide), new Location(kingLocation.Rank, kingLocation.File + 1));
        }
        private void MoveRookBesideQueen(Location kingLocation)
        {
            MovePieceFromSquareToSquare(castling.GetRookLocationFromCastlingSide(gameState.playerTurnColor, CastlingSide.QueenSide), new Location(kingLocation.Rank, kingLocation.File - 1));
        }
        public void CheckAndHandleCastling(Location currentLocation, Location newLocation)
        {
            if (castling.IsKingSideCastling(currentLocation, newLocation))
                MoveRookBesideKing(newLocation);
            else if (castling.IsQueenSideCastling(currentLocation, newLocation))
                MoveRookBesideQueen(newLocation);
        }
        public void CheckAndHandlePawnPromotion(Location location)
        {

            if (location.Rank == 0 || location.Rank == Static.NUMBER_OF_RANKS - 1)
            {
                PieceType pieceType = GUI.ShowCustomPieceTypeDialog();

                chessBoard.matrix[location.Rank, location.File] = PieceFactory.CreatePiece(pieceType, location.Rank == 0 ? Color.Black : Color.White);
                UpdateImageAtLocation(location);
            }
        }


        public void LimitPiecesMovements(Piece clickedPiece, List<Location> possibleMovements)
        {
            if (clickedPiece is not King)
                chessBoard.RemoveInvalidMovesForCheck(possibleMovements, gameState.checkingLocation, gameState.playerTurnColor == Color.White ? gameState.whiteKingLocation : gameState.blackKingLocation, gameState.playerTurnColor);
            else
                possibleMovements.RemoveAll(location => GetAllAttackedLocations().Contains(location));
        }

        public Image? GetImage(int rank, int file)
        {
            Piece piece = chessBoard.GetPieceAt(new Location(rank, file));
            return piece != null ? LoadPieceImage(piece) : null;
        }


        public void MovePieceFromSquareToSquare(Location currentLocation, Location newLocation)
        {
            Piece piece = chessBoard.GetPieceAt(currentLocation);

            chessBoard.AddPieceAt(piece, newLocation);
            chessBoard.RemovePieceAt(currentLocation);

            UpdateImageAtLocation(newLocation);
            RemoveImageAtLocation(currentLocation);
        }

        public List<Location> GetAllAttackedLocations()
        {
            List<Location> attackLocations = new();
            for (int rank = 0; rank < Static.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Static.NUMBER_OF_FILES; file++)
                {
                    Piece piece = chessBoard.matrix[rank, file]!;
                    if (piece != null && piece.pieceColor != gameState.playerTurnColor)
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
    }
}
