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

            UpdateCastlingCondition(currentSquare);
            if (currentSquare._pieceOnSquare is King)
            {
                if (currentLocation.File - newLocation.File == 2)
                    Game.movePieceFromSquareToSquare(new Location(0, 0), new Location(newLocation.Rank, newLocation.File + 1));
                else if (newLocation.File - currentLocation.File == 2)
                    Game.movePieceFromSquareToSquare(new Location(0, 7), new Location(newLocation.Rank, newLocation.File - 1));
            }
            newSquare.AddPieceToSquare(currentSquare._pieceOnSquare);
            GUI.ChessGui().updateImageAtLocation(newLocation);
            currentSquare.RemovePieceFromSquare();
            GUI.ChessGui().removeImageAtLocation(currentLocation);
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
                    if (piece._rookSide == RookSide.KingSide)
                        Game.whiteCastlingAllowedKingSide = false;
                    else if (piece._rookSide == RookSide.QueenSide)
                        Game.whiteCastlingAllowedQueenSide = false;
                }
                else
                {
                    if (piece._rookSide == RookSide.KingSide)
                        Game.blackCastlingAllowedKingSide = false;
                    else if (piece._rookSide == RookSide.QueenSide)
                        Game.blackCastlingAllowedQueenSide = false;
                }
            }
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
