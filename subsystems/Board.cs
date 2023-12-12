using ChessGame.Global;

namespace ChessGame.Subsystems
{
    internal class Board
    {
        public IPiece?[,] Matrix { get; set; }

        public Board()
        {
            Matrix = new IPiece[Static.NUMBER_OF_RANKS, Static.NUMBER_OF_FILES];
            AddPiecesToBoard();
        }

        public void AddPieceAt(IPiece piece, Location location)
        {
            Matrix[location.Rank, location.File] = piece;
        }

        public void RemovePieceAt(Location location)
        {
            Matrix[location.Rank, location.File] = null;
        }

        public IPiece? GetPieceAt(Location location)
        {
            return Matrix[location.Rank, location.File];
        }
        private void AddPiecesToBoard()
        {
            // Add white pieces on the first rank
            Color pieceColor = Color.White;
            Matrix[0, 0] = new Rook(pieceColor, RookSide.KingSide);
            Matrix[0, 1] = new Knight(pieceColor);
            Matrix[0, 2] = new Bishop(pieceColor);
            Matrix[0, 3] = new King(pieceColor);
            Matrix[0, 4] = new Queen(pieceColor);
            Matrix[0, 5] = new Bishop(pieceColor);
            Matrix[0, 6] = new Knight(pieceColor);
            Matrix[0, 7] = new Rook(pieceColor, RookSide.QueenSide);

            // Add white pawns on the second rank
            for (int file = 0; file < Static.NUMBER_OF_FILES; file++)
            {
                Matrix[1, file] = new Pawn(pieceColor);
            }

            // Add black pawns on the seventh rank
            pieceColor = Color.Black;
            for (int file = 0; file < Static.NUMBER_OF_FILES; file++)
            {
                Matrix[6, file] = new Pawn(pieceColor);
            }

            // Add black pieces on the eighth rank
            Matrix[7, 0] = new Rook(pieceColor, RookSide.KingSide);
            Matrix[7, 1] = new Knight(pieceColor);
            Matrix[7, 2] = new Bishop(pieceColor);
            Matrix[7, 3] = new King(pieceColor);
            Matrix[7, 4] = new Queen(pieceColor);
            Matrix[7, 5] = new Bishop(pieceColor);
            Matrix[7, 6] = new Knight(pieceColor);
            Matrix[7, 7] = new Rook(pieceColor, RookSide.QueenSide);
        }

        public bool IsMoveWithinBoard(Location newLocation)
        {
            return newLocation.Rank >= 0 && newLocation.Rank < Static.NUMBER_OF_RANKS && newLocation.File >= 0 && newLocation.File < Static.NUMBER_OF_FILES;
        }

        public void RemoveInvalidMoves(List<Location> pieceMovements, Color pieceColor)
        {
            pieceMovements.RemoveAll(location => !IsMoveWithinBoard(location));
            pieceMovements.RemoveAll(location => Matrix[location.Rank, location.File] != null && Matrix[location.Rank, location.File]!.PieceColor == pieceColor);
        }
        public void RemoveInvalidMovesForCheck(List<Location> pieceMovements, Location checkingPieceLocation, Location kingLocation, Color playerColor)
        {
            List<Location> allowedLocations = GetAvailableMovesInDirection(checkingPieceLocation, MapDirection(kingLocation.Rank - checkingPieceLocation.Rank), MapDirection(kingLocation.File - checkingPieceLocation.File), playerColor);
            pieceMovements.RemoveAll(location => location != checkingPieceLocation && !allowedLocations.Contains(location));
        }

        public int MapDirection(int x)
        {
            return x > 0 ? +1 : x < 0 ? -1 : 0;
        }

        public List<Location> GetAvailableMovesInDirection(Location currentLocation, int rowDirection, int colDirection, Color pieceColor)
        {
            List<Location> pieceMovements = new();

            for (int movement = 1; movement < 8; movement++)
            {
                Location newLocation = new(currentLocation.Rank + (movement * rowDirection), currentLocation.File + (movement * colDirection));

                if (!IsMoveWithinBoard(newLocation))
                    break; // Stop if the new location is outside the board

                if (Matrix[newLocation.Rank, newLocation.File] != null)
                {
                    if (Matrix[newLocation.Rank, newLocation.File]!.PieceColor != pieceColor)
                    {
                        pieceMovements.Add(newLocation);
                        // Stop if there is a piece in the way unless it's opposite color
                    }
                    break;
                }
                pieceMovements.Add(newLocation);
            }

            return pieceMovements;
        }
    }
}
