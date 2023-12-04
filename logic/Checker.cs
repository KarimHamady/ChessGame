using ChessGame.Global;
using ChessGame.Logic.PieceNamespace;
using ChessGame.Statics;
using ChessGame.Logic.BoardNamespace;

namespace ChessGame.Logic
{
    namespace CheckerNamespace { 
        internal class Checker
        {
            public bool IsMoveValid(Location newLocation)
            {
                if (!IsMoveWithinBoard(newLocation))
                    return false;
                if (IsKingChecked())
                    return IsMoveValidWhenKingChecked();
                if (IsPiecePinned())
                    return IsMoveValidWhenPiecePinned();
                return false;
            }

            public static bool IsMoveWithinBoard(Location newLocation)
            {
                return newLocation.Rank >= 0 && newLocation.Rank < Constants.NUMBER_OF_RANKS && newLocation.File >= 0 && newLocation.File < Constants.NUMBER_OF_FILES;
            }
            public static bool IsKingChecked()
            {
                return false;
            }
            public static bool IsPiecePinned()
            {
                return false;
            }
            public static bool IsMoveValidWhenKingChecked()
            {
                return true;
            }

            public static bool IsMoveValidWhenPiecePinned()
            {
                return true;
            }

            public static bool IsCastlingAllowedForPlayerKingside(King selectedKing)
            {
                // return !(selectedKing.moved() || playerRook.kingSide.moved()) && !isKingChecked() && piecesBlockingWay();
                return true;
            }
            public static bool IsCastlingAllowedForPlayerQueenside()
            {
                // return !(selectedKing.moved() || playerRook.kingSide.moved()) && !isKingChecked();
                return true;
            }

            /*public List<Location> checkAvailableMoves(PieceNamespace piece)
            {
                foreach (Location location in piece.pieceMovements)
                {

                }
            }*/

            public static void RemoveInvalidMoves(List<Location> pieceMovements)
            {
                pieceMovements.RemoveAll(location => !IsMoveWithinBoard(location));
                pieceMovements.RemoveAll(location => Board.GetBoard().matrix[location.Rank, location.File]._pieceOnSquare != null && Board.GetBoard().matrix[location.Rank, location.File]._pieceOnSquare!.pieceColor == GameNamespace.Game.playerTurnColor);
            }
            public static void RemoveInvalidMovesForCheck(List<Location> pieceMovements)
            {
                Location checkingPieceLocation = GameNamespace.Game.checkingLocation;
                Piece? checkingPiece = Board.GetBoard().matrix[checkingPieceLocation.Rank, checkingPieceLocation.File]._pieceOnSquare;
                Location king = GameNamespace.Game.playerTurnColor == Color.White ? new Location(0, 3) : new Location(7, 3); // Fixme
                List<Location> allowedLocations = GetAvailableMovesInDirection(checkingPieceLocation, MapDirection(king.Rank - checkingPieceLocation.Rank), MapDirection(king.File - checkingPieceLocation.File));
                pieceMovements.RemoveAll(location => location != GameNamespace.Game.checkingLocation && !allowedLocations.Contains(location));
            }

            public static int MapDirection(int x)
            {
                return x > 0 ? +1 : x < 0 ? -1 : 0;
            }

            public static List<Location> GetAvailableMovesInDirection(Location currentLocation, int rowDirection, int colDirection)
            {
                List<Location> pieceMovements = new();

                for (int movement = 1; movement < 8; movement++)
                {
                    Location newLocation = new(currentLocation.Rank + (movement * rowDirection), currentLocation.File + (movement * colDirection));

                    if (!IsMoveWithinBoard(newLocation))
                        break; // Stop if the new location is outside the board

                    if (Board.GetBoard().matrix[newLocation.Rank, newLocation.File]._pieceOnSquare != null)
                    {
                        if (Board.GetBoard().matrix[newLocation.Rank, newLocation.File]._pieceOnSquare!.pieceColor != GameNamespace.Game.playerTurnColor)
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

            public static List<Location> BishopAvailableMoves(Location currentLocation)
            {
                List<Location> pieceMovements = new();

                // Check all four diagonals
                pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, -1, -1)); // Top-left
                pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, -1, 1));  // Top-right
                pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 1, -1));  // Bottom-left
                pieceMovements.AddRange(GetAvailableMovesInDirection(currentLocation, 1, 1));   // Bottom-right

                return pieceMovements;
            }
        }
    }
}
