using ChessGame.FacadeNamespace;
using ChessGame.Global;
using ChessGame.logic;
using ChessGame.Logic.BoardNamespace;
using ChessGame.Logic.CheckerNamespace;
using ChessGame.Logic.GameNamespace;

namespace ChessGame.Logic
{
    namespace PieceNamespace
    {
        public abstract class Piece
        {
            public Statics.PieceType pieceType = Statics.PieceType.None;
            public Color pieceColor;
            protected bool capturesLikeItsMove = false;
            public abstract List<Location> GetAvailableMovesOnBoard(Location currentLocation);
        }

        internal class Pawn : Piece
        {
            public Pawn(Color color)
            {
                pieceType = Statics.PieceType.Pawn;
                pieceColor = color;
                capturesLikeItsMove = false;
            }
            public override List<Location> GetAvailableMovesOnBoard(Location currentLocation)
            {
                List<Location> pieceMovements = new();

                int rankDirection = this.pieceColor == Color.White ? 1 : -1;
                Location forwardOne = currentLocation + new Location(rankDirection * 1, 0);
                Piece? piece;

                if (Checker.IsMoveWithinBoard(forwardOne))
                {
                    piece = Board.GetBoard().matrix[forwardOne.Rank, forwardOne.File];
                    if (piece == null)
                        pieceMovements.Add(forwardOne); // Move forward
                }
                Location fowardTwo = currentLocation + new Location(rankDirection * 2, 0);
                if (Checker.IsMoveWithinBoard(fowardTwo))
                {
                    piece = Board.GetBoard().matrix[fowardTwo.Rank, fowardTwo.File];
                    if (piece == null && (rankDirection * currentLocation.Rank == 1 || currentLocation.Rank * rankDirection == -6))
                        pieceMovements.Add(fowardTwo); // Move forward (initial double move)
                }
                // Pawn can only capture diagonal if there is an opponent piece
                Location diagonalLeft = currentLocation + new Location(rankDirection * 1, -1);
                if (Checker.IsMoveWithinBoard(diagonalLeft))
                {
                    piece = Board.GetBoard().matrix[diagonalLeft.Rank, diagonalLeft.File];
                    if (piece != null && piece.pieceColor != Game.playerTurnColor)
                        pieceMovements.Add(diagonalLeft); // Capture diagonally left
                }

                Location diagonalRight = currentLocation + new Location(rankDirection * 1, 1);
                if (Checker.IsMoveWithinBoard(diagonalRight))
                {
                    piece = Board.GetBoard().matrix[diagonalRight.Rank, diagonalRight.File];
                    if (piece != null && piece.pieceColor != Game.playerTurnColor)
                        pieceMovements.Add(diagonalRight); // Capture diagonally right
                }
                Checker.RemoveInvalidMoves(pieceMovements);
                return pieceMovements;
            }
        }

        internal class Knight : Piece
        {
            public Knight(Color color)
            {
                pieceType = Statics.PieceType.Knight;
                pieceColor = color;
                capturesLikeItsMove = true;
            }
            public override List<Location> GetAvailableMovesOnBoard(Location currentLocation)
            {
                List<Location> pieceMovements = new();

                pieceMovements.Add(currentLocation + new Location(-2, -1));
                pieceMovements.Add(currentLocation + new Location(-2, 1));
                pieceMovements.Add(currentLocation + new Location(2, -1));
                pieceMovements.Add(currentLocation + new Location(2, 1));
                pieceMovements.Add(currentLocation + new Location(-1, -2));
                pieceMovements.Add(currentLocation + new Location(-1, 2));
                pieceMovements.Add(currentLocation + new Location(1, -2));
                pieceMovements.Add(currentLocation + new Location(1, 2));

                Checker.RemoveInvalidMoves(pieceMovements);
                return pieceMovements;
            }
        }

        internal class Bishop : Piece
        {
            List<Location> movementDirection = new() { new Location(-1, -1), new Location(-1, 1), new Location(1, -1), new Location(1, 1) };
            public Bishop(Color color)
            {
                pieceType = Statics.PieceType.Bishop;
                pieceColor = color;
                capturesLikeItsMove = true;
            }

            public override List<Location> GetAvailableMovesOnBoard(Location currentLocation)
            {
                List<Location> pieceMovements = new();

                foreach (Location movement in movementDirection)
                    pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, movement.Rank, movement.File));

                Checker.RemoveInvalidMoves(pieceMovements);
                return pieceMovements;
            }
        }

        internal class Rook : Piece
        {
            List<Location> movementDirection = new() { new Location(0, -1), new Location(0, 1), new Location(-1, 0), new Location(1, 0) };
            public Statics.RookSide _rookSide;

            public Rook(Color color, Statics.RookSide rookSide)
            {
                pieceType = Statics.PieceType.Rook;
                pieceColor = color;
                capturesLikeItsMove = true;
                _rookSide = rookSide;
            }

            public override List<Location> GetAvailableMovesOnBoard(Location currentLocation)
            {
                List<Location> pieceMovements = new();

                foreach (Location movement in movementDirection)
                    pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, movement.Rank, movement.File));

                Checker.RemoveInvalidMoves(pieceMovements);
                return pieceMovements;
            }

        }

        internal class Queen : Piece
        {
            List<Location> movementDirection = new() { new Location(-1, -1), new Location(-1, 1), new Location(1, -1), new Location(1, 1), new Location(0, -1), new Location(0, 1), new Location(-1, 0), new Location(1, 0) };
            public Queen(Color color)
            {
                pieceType = Statics.PieceType.Queen;
                pieceColor = color;
                capturesLikeItsMove = true;
            }
            public override List<Location> GetAvailableMovesOnBoard(Location currentLocation)
            {
                List<Location> pieceMovements = new();

                pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, -1, -1)); // Top-left
                pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, -1, 1));  // Top-right
                pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 1, -1));  // Bottom-left
                pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 1, 1));   // Bottom-right
                pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 0, -1)); // Left
                pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 0, 1));  // Right
                pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, -1, 0)); // Up
                pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 1, 0));  // Down

                Checker.RemoveInvalidMoves(pieceMovements);
                return pieceMovements;
            }
        }

        internal class King : Piece
        {
            public King(Color color)
            {
                pieceType = Statics.PieceType.King;
                pieceColor = color;
                capturesLikeItsMove = true;
            }
            public override List<Location> GetAvailableMovesOnBoard(Location currentLocation)
            {
                List<Location> pieceMovements = new();

                pieceMovements.Add(currentLocation + new Location(-1, -1));
                pieceMovements.Add(currentLocation + new Location(-1, 0));
                pieceMovements.Add(currentLocation + new Location(-1, 1));
                pieceMovements.Add(currentLocation + new Location(0, -1));
                pieceMovements.Add(currentLocation + new Location(0, 1));
                pieceMovements.Add(currentLocation + new Location(1, -1));
                pieceMovements.Add(currentLocation + new Location(1, 0));
                pieceMovements.Add(currentLocation + new Location(1, 1));

                if (!Game.check)
                {
                    // FIXME Check if a piece is blocking the castling (+1, -1) before allowing (+2, -2) using attackedLocations list
                    if (Game.playerTurnColor == Color.White)
                    {
                        if (Castle.whiteCastlingAllowedKingSide == true)
                            pieceMovements.Add(currentLocation + new Location(0, -2));
                        if (Castle.whiteCastlingAllowedQueenSide == true)
                            pieceMovements.Add(currentLocation + new Location(0, 2));
                    }
                    else if (Game.playerTurnColor == Color.Black)
                    {
                        if (Castle.blackCastlingAllowedKingSide == true)
                            pieceMovements.Add(currentLocation + new Location(0, -2));
                        if (Castle.blackCastlingAllowedQueenSide == true)
                            pieceMovements.Add(currentLocation + new Location(0, 2));
                    }
                }
                Checker.RemoveInvalidMoves(pieceMovements);
                if (Game.playerTurnColor == this.pieceColor)
                    pieceMovements.RemoveAll(location => Facade.GetAllAttackedLocations().Contains(location));
                return pieceMovements;
            }
        }
    }
}