using ChessGame.GameNamespace;
using ChessGame.Global;

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

                if (Game.GetInstance().chessBoard.IsMoveWithinBoard(forwardOne))
                {
                    piece = Game.GetInstance().chessBoard.matrix[forwardOne.Rank, forwardOne.File];
                    if (piece == null)
                        pieceMovements.Add(forwardOne); // Move forward
                }
                Location fowardTwo = currentLocation + new Location(rankDirection * 2, 0);
                if (Game.GetInstance().chessBoard.IsMoveWithinBoard(fowardTwo))
                {
                    piece = Game.GetInstance().chessBoard.matrix[fowardTwo.Rank, fowardTwo.File];
                    if (piece == null && (rankDirection * currentLocation.Rank == 1 || currentLocation.Rank * rankDirection == -6))
                        pieceMovements.Add(fowardTwo); // Move forward (initial double move)
                }
                // Pawn can only capture diagonal if there is an opponent piece
                Location diagonalLeft = currentLocation + new Location(rankDirection * 1, -1);
                if (Game.GetInstance().chessBoard.IsMoveWithinBoard(diagonalLeft))
                {
                    piece = Game.GetInstance().chessBoard.matrix[diagonalLeft.Rank, diagonalLeft.File];
                    if (piece != null && piece.pieceColor != Game.GetInstance().gameState.playerTurnColor)
                        pieceMovements.Add(diagonalLeft); // Capture diagonally left
                }

                Location diagonalRight = currentLocation + new Location(rankDirection * 1, 1);
                if (Game.GetInstance().chessBoard.IsMoveWithinBoard(diagonalRight))
                {
                    piece = Game.GetInstance().chessBoard.matrix[diagonalRight.Rank, diagonalRight.File];
                    if (piece != null && piece.pieceColor != Game.GetInstance().gameState.playerTurnColor)
                        pieceMovements.Add(diagonalRight); // Capture diagonally right
                }
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
                List<Location> pieceMovements = new()
                {
                    currentLocation + new Location(-2, -1),
                    currentLocation + new Location(-2, 1),
                    currentLocation + new Location(2, -1),
                    currentLocation + new Location(2, 1),
                    currentLocation + new Location(-1, -2),
                    currentLocation + new Location(-1, 2),
                    currentLocation + new Location(1, -2),
                    currentLocation + new Location(1, 2)
                };

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
                    pieceMovements.AddRange(Game.GetInstance().chessBoard.GetAvailableMovesInDirection(currentLocation, movement.Rank, movement.File, Game.GetInstance().GetPlayerColor()));

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
                    pieceMovements.AddRange(Game.GetInstance().chessBoard.GetAvailableMovesInDirection(currentLocation, movement.Rank, movement.File, Game.GetInstance().GetPlayerColor()));

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

                pieceMovements.AddRange(Game.GetInstance().chessBoard.GetAvailableMovesInDirection(currentLocation, -1, -1, Game.GetInstance().GetPlayerColor())); // Top-left
                pieceMovements.AddRange(Game.GetInstance().chessBoard.GetAvailableMovesInDirection(currentLocation, -1, 1, Game.GetInstance().GetPlayerColor()));  // Top-right
                pieceMovements.AddRange(Game.GetInstance().chessBoard.GetAvailableMovesInDirection(currentLocation, 1, -1, Game.GetInstance().GetPlayerColor()));  // Bottom-left
                pieceMovements.AddRange(Game.GetInstance().chessBoard.GetAvailableMovesInDirection(currentLocation, 1, 1, Game.GetInstance().GetPlayerColor()));   // Bottom-right
                pieceMovements.AddRange(Game.GetInstance().chessBoard.GetAvailableMovesInDirection(currentLocation, 0, -1, Game.GetInstance().GetPlayerColor())); // Left
                pieceMovements.AddRange(Game.GetInstance().chessBoard.GetAvailableMovesInDirection(currentLocation, 0, 1, Game.GetInstance().GetPlayerColor()));  // Right
                pieceMovements.AddRange(Game.GetInstance().chessBoard.GetAvailableMovesInDirection(currentLocation, -1, 0, Game.GetInstance().GetPlayerColor())); // Up
                pieceMovements.AddRange(Game.GetInstance().chessBoard.GetAvailableMovesInDirection(currentLocation, 1, 0, Game.GetInstance().GetPlayerColor()));  // Down

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
                List<Location> pieceMovements = new()
                {
                    currentLocation + new Location(-1, -1),
                    currentLocation + new Location(-1, 0),
                    currentLocation + new Location(-1, 1),
                    currentLocation + new Location(0, -1),
                    currentLocation + new Location(0, 1),
                    currentLocation + new Location(1, -1),
                    currentLocation + new Location(1, 0),
                    currentLocation + new Location(1, 1)
                };

                if (!Game.GetInstance().gameState.check)
                {
                    // FIXME Check if a piece is blocking the castling (+1, -1) before allowing (+2, -2) using attackedLocations list
                    if (Game.GetInstance().gameState.playerTurnColor == Color.White)
                    {
                        if (Game.GetInstance().castling.whiteCastlingAllowedKingSide == true)
                            pieceMovements.Add(currentLocation + new Location(0, -2));
                        if (Game.GetInstance().castling.whiteCastlingAllowedQueenSide == true)
                            pieceMovements.Add(currentLocation + new Location(0, 2));
                    }
                    else if (Game.GetInstance().gameState.playerTurnColor == Color.Black)
                    {
                        if (Game.GetInstance().castling.blackCastlingAllowedKingSide == true)
                            pieceMovements.Add(currentLocation + new Location(0, -2));
                        if (Game.GetInstance().castling.blackCastlingAllowedQueenSide == true)
                            pieceMovements.Add(currentLocation + new Location(0, 2));
                    }
                }

                if (Game.GetInstance().gameState.playerTurnColor == this.pieceColor)
                    pieceMovements.RemoveAll(location => Game.GetInstance().GetAllAttackedLocations().Contains(location));
                return pieceMovements;
            }
        }
    }
}