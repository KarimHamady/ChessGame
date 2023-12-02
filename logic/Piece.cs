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
        public List<Location> pieceMovements = new List<Location>();
        protected bool capturesLikeItsMove = false;
    }

    internal class Pawn : Piece
    {
        public Pawn()
        {
            pieceType = PieceType.Pawn;

            pieceMovements.Add(new Location(1, 0)); // Move forward
            pieceMovements.Add(new Location(2, 0)); // Move forward (initial double move)
            pieceMovements.Add(new Location(1, -1)); // Capture diagonally left
            pieceMovements.Add(new Location(1, 1)); // Capture diagonally right

            capturesLikeItsMove = false;
        }
    }

    internal class Knight : Piece
    {
        public Knight()
        {
            pieceType = PieceType.Knight;

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
        public Bishop()
        {
            pieceType = PieceType.Bishop;

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
        public Rook()
        {
            pieceType = PieceType.Rook;

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
        public Queen()
        {
            pieceType = PieceType.Queen;

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
        public King()
        {
            pieceType = PieceType.King;

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
