namespace ChessGame.logic
{
    enum PieceType
    {
        None,
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    internal abstract class Piece
    {
        public PieceType pieceType = PieceType.None;
        public Color pieceColor;
        public List<Location> pieceMovements = new List<Location>();
        protected bool capturesLikeItsMove = false;
    }

    internal class Pawn : Piece
    {
        public Pawn(Color color)
        {
            pieceType = PieceType.Pawn;

            pieceColor = color;

            int rankDirection = pieceColor == Color.White ? 1 : -1;
            pieceMovements.Add(new Location(rankDirection * 1, 0)); // Move forward
            pieceMovements.Add(new Location(rankDirection * 2, 0)); // Move forward (initial double move)
            pieceMovements.Add(new Location(rankDirection * 1, -1)); // Capture diagonally left
            pieceMovements.Add(new Location(rankDirection * 1, 1)); // Capture diagonally right


            capturesLikeItsMove = false;
        }
    }

    internal class Knight : Piece
    {
        public Knight(Color color)
        {
            pieceType = PieceType.Knight;

            pieceColor = color;

            pieceMovements.Add(new Location(-2, -1));
            pieceMovements.Add(new Location(-2, 1));
            pieceMovements.Add(new Location(2, -1));
            pieceMovements.Add(new Location(2, 1));
            pieceMovements.Add(new Location(-1, -2));
            pieceMovements.Add(new Location(-1, 2));
            pieceMovements.Add(new Location(1, -2));
            pieceMovements.Add(new Location(1, 2));

            capturesLikeItsMove = true;
        }
    }

    internal class Bishop : Piece
    {
        public Bishop(Color color)
        {
            pieceType = PieceType.Bishop;

            pieceColor = color;
            for (int movement = 1; movement < 8; movement++)
            {
                pieceMovements.Add(new Location(-movement, -movement));
                pieceMovements.Add(new Location(movement, -movement));
                pieceMovements.Add(new Location(-movement, movement));
                pieceMovements.Add(new Location(movement, movement));
            };

            capturesLikeItsMove = true;
        }
    }

    internal class Rook : Piece
    {
        public Rook(Color color)
        {
            pieceType = PieceType.Rook;

            pieceColor = color;

            for (int movement = 1; movement < 8; movement++)
            {
                pieceMovements.Add(new Location(movement, 0));
                pieceMovements.Add(new Location(0, movement));
                pieceMovements.Add(new Location(-movement, 0));
                pieceMovements.Add(new Location(0, -movement));
            }

            capturesLikeItsMove = true;
        }
    }

    internal class Queen : Piece
    {
        public Queen(Color color)
        {
            pieceType = PieceType.Queen;

            pieceColor = color;

            for (int movement = 1; movement < 8; movement++)
            {
                //Bishop
                pieceMovements.Add(new Location(-movement, -movement));
                pieceMovements.Add(new Location(movement, -movement));
                pieceMovements.Add(new Location(-movement, movement));
                pieceMovements.Add(new Location(movement, movement));

                // Rook
                pieceMovements.Add(new Location(movement, 0));
                pieceMovements.Add(new Location(0, movement));
                pieceMovements.Add(new Location(-movement, 0));
                pieceMovements.Add(new Location(0, -movement));
            };

            capturesLikeItsMove = true;
        }
    }

    internal class King : Piece
    {
        public King(Color color)
        {
            pieceType = PieceType.King;

            pieceColor = color;

            pieceMovements.Add(new Location(-1, -1));
            pieceMovements.Add(new Location(-1, 0));
            pieceMovements.Add(new Location(-1, 1));
            pieceMovements.Add(new Location(0, -1));
            pieceMovements.Add(new Location(0, 1));
            pieceMovements.Add(new Location(1, -1));
            pieceMovements.Add(new Location(1, 0));
            pieceMovements.Add(new Location(1, 1));

            capturesLikeItsMove = true;
        }
    }
}
