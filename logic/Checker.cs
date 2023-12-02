namespace ChessGame.logic
{
    internal class Checker
    {
        public bool isMoveValid(Location newLocation)
        {
            if (!isMoveWithinBoard(newLocation))
                return false;
            if (isKingChecked())
                return isMoveValidWhenKingChecked();
            if (isPiecePinned())
                return isMoveValidWhenPiecePinned();
            return false;
        }

        public static bool isMoveWithinBoard(Location newLocation)
        {
            return newLocation.Rank >= 0 && newLocation.Rank < Board.NUMBER_OF_RANKS && newLocation.File >= 0 && newLocation.File < Board.NUMBER_OF_FILES;
        }
        public bool isKingChecked()
        {
            return false;
        }
        public bool isPiecePinned()
        {
            return false;
        }
        public bool isMoveValidWhenKingChecked()
        {
            return true;
        }

        public bool isMoveValidWhenPiecePinned()
        {
            return true;
        }

        public bool isCastlingAllowedForPlayerKingside(King selectedKing)
        {
            // return !(selectedKing.moved() || playerRook.kingSide.moved()) && !isKingChecked() && piecesBlockingWay();
            return true;
        }
        public bool isCastlingAllowedForPlayerQueenside()
        {
            // return !(selectedKing.moved() || playerRook.kingSide.moved()) && !isKingChecked();
            return true;
        }

        /*public List<Location> checkAvailableMoves(Piece piece)
        {
            foreach (Location location in piece.pieceMovements)
            {

            }
        }*/
    }
}
