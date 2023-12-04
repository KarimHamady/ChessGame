using ChessGame.Global;
using ChessGame.logic;
using ChessGame.Logic.BoardNamespace;
using ChessGame.Logic.CheckerNamespace;
using ChessGame.Logic.GameNamespace;
using ChessGame.Logic.PieceNamespace;
using ChessGame.Statics;

namespace ChessGame.FacadeNamespace
{
    internal class Facade
    {
        private static Facade? instance;
        public static PictureBox[,] chessboardPictureBoxes = new PictureBox[Constants.NUMBER_OF_RANKS, Constants.NUMBER_OF_FILES];
        private Facade() { }
        public static Facade GetInstance()
        {
            if (instance == null)
                instance = new Facade();
            return instance;
        }
        public static void UpdateImageAtLocation(Location location)
        {
            chessboardPictureBoxes[location.Rank, location.File].Image = LoadPieceImage(Board.GetBoard().matrix[location.Rank, location.File]);
        }
        public static void RemoveImageAtLocation(Location location)
        {
            chessboardPictureBoxes[location.Rank, location.File].Image = null;
        }
        public static Image? LoadPieceImage(Piece? piece)
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
        public static void HandlePieceClick(Location clickLocation)
        {
            Piece? clickedPiece = Board.GetPieceAtLocation(clickLocation);

            if (clickedPiece != null && clickedPiece.pieceColor == Game.playerTurnColor)
            {
                // Get the possible movements for the clicked piece
                List<Location> possibleMovements = clickedPiece.GetAvailableMovesOnBoard(clickLocation);
                if (Game.check)
                {
                    LimitPiecesMovements(clickedPiece, possibleMovements);
                    // TODO: improve
                    /*if (possibleMovements.Count == 0)
                    {
                        GUI.AddCheckLabel();
                        return;
                    }*/
                }

                GUI.ResetSquareColors();
                GUI.ColorLocations(possibleMovements, Color.Gold);
                Game.clickedLocation = clickLocation;
                Game.possibleMovements = possibleMovements;
            }
            else if (Game.clickedLocation.Rank != -1 && Game.clickedLocation.File != -1)
            {
                if (Game.possibleMovements.Contains(clickLocation))
                {
                    MovePieceFromSquareToSquare(Game.clickedLocation, clickLocation);
                    Castle.UpdateCastlingCondition(Game.playerTurnColor, Board.GetBoard().matrix[Game.clickedLocation.Rank, Game.clickedLocation.File]);
                    if (Board.GetBoard().matrix[clickLocation.Rank, clickLocation.File] is King)
                        CheckAndHandleCastling(Game.clickedLocation, clickLocation);

                    GUI.ResetSquareColors();
                    Game.ResetGameCheckVariables();
                    foreach (Location location in Board.GetPieceAtLocation(clickLocation).GetAvailableMovesOnBoard(clickLocation))
                    {
                        Piece? piece = Board.GetBoard().matrix[location.Rank, location.File];
                        if (piece != null && piece is King && piece.pieceColor != Game.playerTurnColor)
                        {
                            chessboardPictureBoxes[location.Rank, location.File].BackColor = Color.Red;
                            Game.checkingLocation = clickLocation;
                            Game.check = true;
                        }
                    }
                    Game.playerTurnColor = Game.playerTurnColor == Color.White ? Color.Black : Color.White;
                }
            }
        }

        private static void MoveRookBesideKing(Location kingLocation)
        {
            MovePieceFromSquareToSquare(Castle.GetRookLocationFromCastlingSide(Game.playerTurnColor, CastlingSide.KingSide), new Location(kingLocation.Rank, kingLocation.File + 1));
        }
        private static void MoveRookBesideQueen(Location kingLocation)
        {
            MovePieceFromSquareToSquare(Castle.GetRookLocationFromCastlingSide(Game.playerTurnColor, CastlingSide.QueenSide), new Location(kingLocation.Rank, kingLocation.File - 1));
        }
        public static void CheckAndHandleCastling(Location currentLocation, Location newLocation)
        {
            if (Castle.IsKingSideCastling(currentLocation, newLocation))
                MoveRookBesideKing(newLocation);
            else if (Castle.IsQueenSideCastling(currentLocation, newLocation))
                MoveRookBesideQueen(newLocation);
        }

        public static void LimitPiecesMovements(Piece clickedPiece, List<Location> possibleMovements)
        {
            if (clickedPiece is not King)
                Checker.RemoveInvalidMovesForCheck(possibleMovements, Game.checkingLocation, Game.playerTurnColor == Color.White ? Game.whiteKingLocation : Game.blackKingLocation);
            else
                possibleMovements.RemoveAll(location => GetAllAttackedLocations().Contains(location));
        }

        public static Image? GetImage(int rank, int file)
        {
            Piece piece = Board.GetPieceAtLocation(new Location(rank, file));
            return piece != null ? LoadPieceImage(piece) : null;
        }


        public static void MovePieceFromSquareToSquare(Location currentLocation, Location newLocation)
        {
            Piece piece = Board.GetBoard().matrix[currentLocation.Rank, currentLocation.File]!;

            Board.AddPieceAtLocation(piece, newLocation);
            Board.RemovePieceAtLocation(currentLocation);

            UpdateImageAtLocation(newLocation);
            RemoveImageAtLocation(currentLocation);
        }

        public static List<Location> GetAllAttackedLocations()
        {
            List<Location> attackLocations = new();
            for (int rank = 0; rank < Constants.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Constants.NUMBER_OF_FILES; file++)
                {
                    Piece piece = Board.GetBoard().matrix[rank, file]!;
                    if (piece != null && piece.pieceColor != Game.playerTurnColor)
                        attackLocations.AddRange(piece.GetAvailableMovesOnBoard(new Location(rank, file)));
                }
            }
            return attackLocations;
        }
    }
}
