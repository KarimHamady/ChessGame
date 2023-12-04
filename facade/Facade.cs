using ChessGame.Logic;
using ChessGame.Global;
using ChessGame.Statics;
using ChessGame.Logic.CheckerNamespace;
using ChessGame.Logic.GameNamespace;
using ChessGame.Logic.PieceNamespace;
using ChessGame.Logic.BoardNamespace;

namespace ChessGame.FacadeNamespace
{
    internal class Facade
    {
        private static Facade? instance;
        public PictureBox[,] chessboardPictureBoxes = new PictureBox[Constants.NUMBER_OF_RANKS, Constants.NUMBER_OF_FILES];
        private Facade() { }
        public static Facade GetInstance()
        {
            if (instance == null)
                instance = new Facade();
            return instance;
        }
        public void UpdateImageAtLocation(Location location)
        {
            chessboardPictureBoxes[location.Rank, location.File].Image = LoadPieceImage(Board.GetBoard().matrix[location.Rank, location.File]._pieceOnSquare);
        }
        public void RemoveImageAtLocation(Location location)
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
        public static void HandlePieceClick(Location clickLocation) {
            int rank = clickLocation.Rank;
            int file = clickLocation.File;
            // Get the piece on the BoardSquare
            Piece? clickedPiece = Board.GetBoard().matrix[rank, file]._pieceOnSquare;

            if (clickedPiece != null && clickedPiece.pieceColor == Game.playerTurnColor)
            {
                // Get the possible movements for the clicked piece
                List<Location> possibleMovements = clickedPiece.GetAvailableMovesOnBoard(new Location(rank, file));
                if (Game.check)
                {
                    LimitPiecesMovements(clickedPiece, possibleMovements);
                    // TODO: improve
                    if (possibleMovements.Count == 0)
                    {
                        GUI.AddCheckLabel();
                        return;
                    }
                }

                GUI.ResetSquareColors();
                GUI.ColorLocations(possibleMovements, Color.Green);
                Game.clickedLocation = new Location(rank, file);
                Game.possibleMovements = possibleMovements;
            }
            else if (Game.clickedLocation.Rank != -1 && Game.clickedLocation.File != -1)
            {
                Location newLocation = new(rank, file);
                if (Game.possibleMovements.Contains(newLocation))
                {
                    MovePieceFromSquareToSquare(Game.clickedLocation, new Location(rank, file));
                    Game.UpdateCastlingCondition(Board.GetBoard().matrix[Game.clickedLocation.Rank, Game.clickedLocation.File]._pieceOnSquare);
                    if (Board.GetBoard().matrix[newLocation.Rank, newLocation.File]._pieceOnSquare is King)
                        CheckAndHandleCastling(Game.clickedLocation, newLocation);

                    GUI.ResetSquareColors();
                    Game.ResetGameCheckVariables();
                    foreach (Location location in Board.GetBoard().matrix[rank, file]._pieceOnSquare!.GetAvailableMovesOnBoard(newLocation))
                    {
                        Piece? piece = Board.GetBoard().matrix[location.Rank, location.File]._pieceOnSquare;
                        if (piece != null && piece is King && piece.pieceColor != Game.playerTurnColor)
                        {
                            GetInstance().chessboardPictureBoxes[location.Rank, location.File].BackColor = Color.Red;
                            Game.checkingLocation = new Location(rank, file);
                            Game.check = true;
                        }
                    }
                    Game.playerTurnColor = Game.playerTurnColor == Color.White ? Color.Black : Color.White;
                }
            }
        }


        public static void LimitPiecesMovements(Piece clickedPiece, List<Location> possibleMovements)
        {
            if (clickedPiece is not King)
                Checker.RemoveInvalidMovesForCheck(possibleMovements);
            else
                possibleMovements.RemoveAll(location => GetAllAttackedLocations().Contains(location));
        }

        public static Color GetColor(int rank, int file)
        {
            return Board.GetBoard().matrix[rank, file]._color;
        }

        public static Image? GetImage(int rank, int file)
        {
            return LoadPieceImage(Board.GetBoard().matrix[rank, file]._pieceOnSquare);
        }


        public static void MovePieceFromSquareToSquare(Location currentLocation, Location newLocation)
        {
            BoardSquare currentSquare = Board.GetBoard().matrix[currentLocation.Rank, currentLocation.File];
            BoardSquare newSquare = Board.GetBoard().matrix[newLocation.Rank, newLocation.File];

            newSquare.AddPieceToSquare(currentSquare._pieceOnSquare!);
            currentSquare.RemovePieceFromSquare();

            GetInstance().UpdateImageAtLocation(newLocation);
            GetInstance().RemoveImageAtLocation(currentLocation);
        }
        private static void MoveRookBesideKing(Location kingLocation)
        {
            MovePieceFromSquareToSquare(Game.GetRookLocationFromCastlingSide(CastlingSide.KingSide), new Location(kingLocation.Rank, kingLocation.File + 1));
        }
        private static void MoveRookBesideQueen(Location kingLocation)
        {
            MovePieceFromSquareToSquare(Game.GetRookLocationFromCastlingSide(CastlingSide.QueenSide), new Location(kingLocation.Rank, kingLocation.File - 1));
        }
        public static void CheckAndHandleCastling(Location currentLocation, Location newLocation)
        {
            if (Game.IsKingSideCastling(currentLocation, newLocation))
                MoveRookBesideKing(newLocation);
            else if (Game.IsQueenSideCastling(currentLocation, newLocation))
                MoveRookBesideQueen(newLocation);
        }
        public static List<Location> GetAllAttackedLocations()
        {
            List<Location> attackLocations = new();
            for (int rank = 0; rank < Constants.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Constants.NUMBER_OF_FILES; file++)
                {
                    BoardSquare boardSquare = Board.GetBoard().matrix[rank, file];
                    if (boardSquare._pieceOnSquare != null && boardSquare._pieceOnSquare.pieceColor == Game.playerTurnColor)
                        attackLocations.AddRange(boardSquare._pieceOnSquare.GetAvailableMovesOnBoard(new Location(rank, file)));
                }
            }
            return attackLocations;
        }
        public static Piece? GetPiece(int rank, int file)
        {
            return Board.GetBoard().matrix[rank, file]._pieceOnSquare;
        }
    }
}
