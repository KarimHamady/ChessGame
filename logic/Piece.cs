namespace ChessGame.logic
{
    public enum PieceType
    {
        None,
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    enum RookSide
    {
        KingSide,
        QueenSide
    }

    public abstract class Piece
    {
        public PieceType pieceType = PieceType.None;
        public Color pieceColor;
        protected bool capturesLikeItsMove = false;
        public abstract List<Location> getAvailableMovesOnBoard(Location currentLocation);
    }

    internal class Pawn : Piece
    {
        public Pawn(Color color)
        {
            pieceType = PieceType.Pawn;
            pieceColor = color;
            capturesLikeItsMove = false;
        }
        public override List<Location> getAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            int rankDirection = this.pieceColor == Color.White ? 1 : -1;
            Location forwardOne = currentLocation + new Location(rankDirection * 1, 0);
            Piece piece;

            piece = Board.GetBoard().matrix[forwardOne.Rank, forwardOne.File]._pieceOnSquare;
            if (piece == null)
                pieceMovements.Add(forwardOne); // Move forward

            Location fowardTwo = currentLocation + new Location(rankDirection * 2, 0);
            piece = Board.GetBoard().matrix[fowardTwo.Rank, fowardTwo.File]._pieceOnSquare;
            if (piece == null && (rankDirection * currentLocation.Rank == 1 || currentLocation.Rank * rankDirection == -6))
                pieceMovements.Add(fowardTwo); // Move forward (initial double move)

            // Pawn can only capture diagonal if there is an opponent piece
            Location diagonalLeft = currentLocation + new Location(rankDirection * 1, -1);
            if (Checker.isMoveWithinBoard(diagonalLeft))
            {
                piece = Board.GetBoard().matrix[diagonalLeft.Rank, diagonalLeft.File]._pieceOnSquare;
                if (piece != null && piece.pieceColor != Game.playerTurnColor)
                    pieceMovements.Add(diagonalLeft); // Capture diagonally left
            }

            Location diagonalRight = currentLocation + new Location(rankDirection * 1, 1);
            if (Checker.isMoveWithinBoard(diagonalRight))
            {
                piece = Board.GetBoard().matrix[diagonalRight.Rank, diagonalRight.File]._pieceOnSquare;
                if (piece != null && piece.pieceColor != Game.playerTurnColor)
                    pieceMovements.Add(diagonalRight); // Capture diagonally right
            }
            Checker.removeInvalidMoves(pieceMovements);
            return pieceMovements;
        }
    }

    internal class Knight : Piece
    {
        public Knight(Color color)
        {
            pieceType = PieceType.Knight;
            pieceColor = color;
            capturesLikeItsMove = true;
        }
        public override List<Location> getAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            pieceMovements.Add(currentLocation + new Location(-2, -1));
            pieceMovements.Add(currentLocation + new Location(-2, 1));
            pieceMovements.Add(currentLocation + new Location(2, -1));
            pieceMovements.Add(currentLocation + new Location(2, 1));
            pieceMovements.Add(currentLocation + new Location(-1, -2));
            pieceMovements.Add(currentLocation + new Location(-1, 2));
            pieceMovements.Add(currentLocation + new Location(1, -2));
            pieceMovements.Add(currentLocation + new Location(1, 2));

            Checker.removeInvalidMoves(pieceMovements);
            return pieceMovements;
        }
    }

    internal class Bishop : Piece
    {
        List<Location> movementDirection = new List<Location> { new Location(-1, -1), new Location(-1, 1), new Location(1, -1), new Location(1, 1) };
        public Bishop(Color color)
        {
            pieceType = PieceType.Bishop;
            pieceColor = color;
            capturesLikeItsMove = true;
        }

        public override List<Location> getAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            foreach (Location movement in movementDirection)
                pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, movement.Rank, movement.File));

            Checker.removeInvalidMoves(pieceMovements);
            return pieceMovements;
        }
    }

    internal class Rook : Piece
    {
        List<Location> movementDirection = new List<Location> { new Location(0, -1), new Location(0, 1), new Location(-1, 0), new Location(1, 0) };
        public bool hasMoved = false;
        public RookSide _rookSide;

        public Rook(Color color, RookSide rookSide)
        {
            pieceType = PieceType.Rook;
            pieceColor = color;
            capturesLikeItsMove = true;
            _rookSide = rookSide;
        }

        public override List<Location> getAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            foreach (Location movement in movementDirection)
                pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, movement.Rank, movement.File));

            Checker.removeInvalidMoves(pieceMovements);
            return pieceMovements;
        }

    }

    internal class Queen : Piece
    {
        List<Location> movementDirection = new List<Location> { new Location(-1, -1), new Location(-1, 1), new Location(1, -1), new Location(1, 1), new Location(0, -1), new Location(0, 1), new Location(-1, 0), new Location(1, 0) };
        public Queen(Color color)
        {
            pieceType = PieceType.Queen;
            pieceColor = color;
            capturesLikeItsMove = true;
        }
        public override List<Location> getAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, -1, -1)); // Top-left
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, -1, 1));  // Top-right
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 1, -1));  // Bottom-left
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 1, 1));   // Bottom-right
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 0, -1)); // Left
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 0, 1));  // Right
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, -1, 0)); // Up
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 1, 0));  // Down

            Checker.removeInvalidMoves(pieceMovements);
            return pieceMovements;
        }
    }

    internal class King : Piece
    {
        public bool hasMoved = false;
        public King(Color color)
        {
            pieceType = PieceType.King;
            pieceColor = color;
            capturesLikeItsMove = true;
        }
        public override List<Location> getAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            pieceMovements.Add(currentLocation + new Location(-1, -1));
            pieceMovements.Add(currentLocation + new Location(-1, 0));
            pieceMovements.Add(currentLocation + new Location(-1, 1));
            pieceMovements.Add(currentLocation + new Location(0, -1));
            pieceMovements.Add(currentLocation + new Location(0, 1));
            pieceMovements.Add(currentLocation + new Location(1, -1));
            pieceMovements.Add(currentLocation + new Location(1, 0));
            pieceMovements.Add(currentLocation + new Location(1, 1));

            if (Game.playerTurnColor == Color.White)
            {
                if (Game.whiteCastlingAllowedKingSide == true)
                    pieceMovements.Add(currentLocation + new Location(0, -2));
                else if (Game.whiteCastlingAllowedQueenSide == true)
                    pieceMovements.Add(currentLocation + new Location(0, 2));
            }
            else if (Game.playerTurnColor == Color.Black)
            {
                if (Game.blackCastlingAllowedKingSide == true)
                    pieceMovements.Add(currentLocation + new Location(0, -2));
                else if (Game.blackCastlingAllowedQueenSide == true)
                    pieceMovements.Add(currentLocation + new Location(0, 2));
            }

            Checker.removeInvalidMoves(pieceMovements);
            return pieceMovements;
        }
    }
}