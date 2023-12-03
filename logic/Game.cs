namespace ChessGame.logic
{
    internal class Game
    {

        public static Location clickedLocation = new Location(-1, -1);
        public static List<Location> possibleMovements = new List<Location>();
        public static Color playerTurnColor = Color.White;
        public static List<Location> attackLocations = getAllAttackedLocations();

        public static bool check = false;
        public static Location checkingLocation = new Location(-1, -1);

        public static bool whiteCastlingAllowedKingSide = true;
        public static bool blackCastlingAllowedKingSide = true;
        public static bool whiteCastlingAllowedQueenSide = true;
        public static bool blackCastlingAllowedQueenSide = true;
        public static void movePieceFromSquareToSquare(Location currentLocation, Location newLocation)
        {
            BoardSquare currentSquare = Board.GetBoard().matrix[currentLocation.Rank, currentLocation.File];
            BoardSquare newSquare = Board.GetBoard().matrix[newLocation.Rank, newLocation.File];

            newSquare.AddPieceToSquare(currentSquare._pieceOnSquare);
            currentSquare.RemovePieceFromSquare();

            view.GUI.ChessGui().updateImageAtLocation(newLocation);
            view.GUI.ChessGui().removeImageAtLocation(currentLocation);
        }

        private static bool IsKingSideCastling(Location currentKingLocation, Location newKingLocation)
        {
            return currentKingLocation.File - newKingLocation.File == 2;
        }
        private static bool IsQueenSideCastling(Location currentKingLocation, Location newKingLocation)
        {
            return newKingLocation.File - currentKingLocation.File == 2;
        }
        private static void MoveRookBesideKing(Location kingLocation)
        {
            Game.movePieceFromSquareToSquare(statics.InitialPieceLocations.GetRookLocationFromCastlingSide(statics.CastlingSide.KingSide), new Location(kingLocation.Rank, kingLocation.File + 1));
        }
        private static void MoveRookBesideQueen(Location kingLocation)
        {
            Game.movePieceFromSquareToSquare(statics.InitialPieceLocations.GetRookLocationFromCastlingSide(statics.CastlingSide.QueenSide), new Location(kingLocation.Rank, kingLocation.File - 1));
        }
        public static void CheckAndHandleCastling(Location currentLocation, Location newLocation)
        {
            if (IsKingSideCastling(currentLocation, newLocation))
                MoveRookBesideKing(newLocation);
            else if (IsQueenSideCastling(currentLocation, newLocation))
                MoveRookBesideQueen(newLocation);
        }
        public static void UpdateCastlingCondition(BoardSquare currentSquare)
        {
            if (currentSquare._pieceOnSquare is King)
            {
                King piece = (King)currentSquare._pieceOnSquare;
                piece.hasMoved = true;
                if (playerTurnColor == Color.White)
                {
                    Game.whiteCastlingAllowedKingSide = false;
                    Game.whiteCastlingAllowedQueenSide = false;
                }
                else
                {
                    Game.blackCastlingAllowedKingSide = false;
                    Game.blackCastlingAllowedQueenSide = false;
                }

            }
            else if (currentSquare._pieceOnSquare is Rook)
            {
                Rook piece = (Rook)currentSquare._pieceOnSquare;
                piece.hasMoved = true;
                if (playerTurnColor == Color.White)
                {
                    if (piece._rookSide == statics.RookSide.KingSide)
                        Game.whiteCastlingAllowedKingSide = false;
                    else if (piece._rookSide == statics.RookSide.QueenSide)
                        Game.whiteCastlingAllowedQueenSide = false;
                }
                else
                {
                    if (piece._rookSide == statics.RookSide.KingSide)
                        Game.blackCastlingAllowedKingSide = false;
                    else if (piece._rookSide == statics.RookSide.QueenSide)
                        Game.blackCastlingAllowedQueenSide = false;
                }
            }
        }
        public static void ResetGameCheckVariables()
        {
            Game.clickedLocation = new Location(-1, -1);
            Game.checkingLocation = new Location(-1, -1);
            Game.check = false;
        }
        public static List<Location> getAllAttackedLocations()
        {
            List<Location> attackLocations = new List<Location>();
            for (int rank = 0; rank < Board.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Board.NUMBER_OF_FILES; file++)
                {
                    BoardSquare boardSquare = Board.GetBoard().matrix[rank, file];
                    if (boardSquare._pieceOnSquare != null && boardSquare._pieceOnSquare.pieceColor == playerTurnColor)
                        attackLocations.AddRange(boardSquare._pieceOnSquare.getAvailableMovesOnBoard(new Location(rank, file)));
                }
            }
            return attackLocations;
        }
    }
}
