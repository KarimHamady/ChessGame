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
        public static void movePieceFromSquareToSquare(Location currentLocation, Location newLocation)
        {
            BoardSquare currentSquare = Board.GetBoard().matrix[currentLocation.Rank, currentLocation.File];
            BoardSquare newSquare = Board.GetBoard().matrix[newLocation.Rank, newLocation.File];

            newSquare.AddPieceToSquare(currentSquare._pieceOnSquare);
            currentSquare.RemovePieceFromSquare();
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
