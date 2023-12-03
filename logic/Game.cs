namespace ChessGame.logic
{
    internal class Game
    {
        public static Location clickedLocation = new Location(-1, -1);
        public static List<Location> possibleMovements = new List<Location>();
        public static Color playerTurnColor = Color.White;
        public static void movePieceFromSquareToSquare(Location currentLocation, Location newLocation)
        {
            BoardSquare currentSquare = Board.GetBoard().matrix[currentLocation.Rank, currentLocation.File];
            BoardSquare newSquare = Board.GetBoard().matrix[newLocation.Rank, newLocation.File];

            newSquare.AddPieceToSquare(currentSquare._pieceOnSquare);
            currentSquare.RemovePieceFromSquare();

            playerTurnColor = playerTurnColor == Color.White ? Color.Black : Color.White;
        }
    }
}
