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

        public static List<Location> GetAvailableMovesForPiece(Location currentLocation, Piece piece)
        {
            List<Location> pieceMovements = new List<Location>();

            switch (piece.pieceType)
            {
                case PieceType.Bishop:
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, -1, -1)); // Top-left
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, -1, 1));  // Top-right
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 1, -1));  // Bottom-left
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 1, 1));   // Bottom-right
                    break;

                case PieceType.Rook:
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 0, -1)); // Left
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 0, 1));  // Right
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, -1, 0)); // Up
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 1, 0));  // Down
                    break;

                case PieceType.Queen:
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, -1, -1)); // Top-left
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, -1, 1));  // Top-right
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 1, -1));  // Bottom-left
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 1, 1));   // Bottom-right
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 0, -1)); // Left
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 0, 1));  // Right
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, -1, 0)); // Up
                    pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 1, 0));  // Down
                    break;

                case PieceType.King:
                    pieceMovements.Add(currentLocation + new Location(-1, -1));
                    pieceMovements.Add(currentLocation + new Location(-1, 0));
                    pieceMovements.Add(currentLocation + new Location(-1, 1));
                    pieceMovements.Add(currentLocation + new Location(0, -1));
                    pieceMovements.Add(currentLocation + new Location(0, 1));
                    pieceMovements.Add(currentLocation + new Location(1, -1));
                    pieceMovements.Add(currentLocation + new Location(1, 0));
                    pieceMovements.Add(currentLocation + new Location(1, 1));
                    break;

                case PieceType.Pawn:
                    int rankDirection = piece.pieceColor == Color.White ? 1 : -1;
                    pieceMovements.Add(currentLocation + new Location(rankDirection * 1, 0)); // Move forward
                    pieceMovements.Add(currentLocation + new Location(rankDirection * 2, 0)); // Move forward (initial double move)
                    pieceMovements.Add(currentLocation + new Location(rankDirection * 1, -1)); // Capture diagonally left
                    pieceMovements.Add(currentLocation + new Location(rankDirection * 1, 1)); // Capture diagonally right
                    break;

                case PieceType.Knight:
                    pieceMovements.Add(currentLocation + new Location(-2, -1));
                    pieceMovements.Add(currentLocation + new Location(-2, 1));
                    pieceMovements.Add(currentLocation + new Location(2, -1));
                    pieceMovements.Add(currentLocation + new Location(2, 1));
                    pieceMovements.Add(currentLocation + new Location(-1, -2));
                    pieceMovements.Add(currentLocation + new Location(-1, 2));
                    pieceMovements.Add(currentLocation + new Location(1, -2));
                    pieceMovements.Add(currentLocation + new Location(1, 2));
                    break;

                default:
                    // Handle unsupported piece types
                    break;
            }
            pieceMovements.RemoveAll(location => !Checker.isMoveWithinBoard(location));
            pieceMovements.RemoveAll(location => Board.GetBoard().matrix[location.Rank, location.File]._pieceOnSquare != null ? Board.GetBoard().matrix[location.Rank, location.File]._pieceOnSquare.pieceColor == Game.playerTurnColor : false);
            return pieceMovements;
        }


        public static List<Location> GetAvailableMovesInDirection(Location currentLocation, int rowDirection, int colDirection)
        {
            List<Location> pieceMovements = new List<Location>();

            for (int movement = 1; movement < 8; movement++)
            {
                Location newLocation = new Location(currentLocation.Rank + (movement * rowDirection), currentLocation.File + (movement * colDirection));

                if (!Checker.isMoveWithinBoard(newLocation))
                    break; // Stop if the new location is outside the board

                if (Board.GetBoard().matrix[newLocation.Rank, newLocation.File]._pieceOnSquare != null)
                    break; // Stop if there is a piece in the way

                pieceMovements.Add(newLocation);
            }

            return pieceMovements;
        }

        public List<Location> BishopAvailableMoves(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            // Check all four diagonals
            pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, -1, -1)); // Top-left
            pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, -1, 1));  // Top-right
            pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 1, -1));  // Bottom-left
            pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 1, 1));   // Bottom-right

            return pieceMovements;
        }
    }
}
