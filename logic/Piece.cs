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
        public Location[] pieceMovements = { };
        protected bool capturesLikeItsMove = false;
    }

    internal class Pawn : Piece
    {
        public Pawn()
        {
            pieceType = PieceType.Pawn;

            pieceMovements = new Location[]
            {
            new Location(1, 0), // Move forward
            new Location(2, 0), // Move forward (initial double move)
            new Location(1, -1), // Capture diagonally left
            new Location(1, 1) // Capture diagonally right
            };

            capturesLikeItsMove = false;
        }
    }

    internal class Knight : Piece
    {
        public Knight()
        {
            pieceType = PieceType.Knight;

            pieceMovements = new Location[]
            {
            new Location(-2, -1),
            new Location(-2, 1),
            new Location(2, -1),
            new Location(2, 1),
            new Location(-1, -2),
            new Location(-1, 2),
            new Location(1, -2),
            new Location(1, 2)
            };

            capturesLikeItsMove = true;
        }
    }

    internal class Bishop : Piece
    {
        public Bishop()
        {
            pieceType = PieceType.Bishop;

            pieceMovements = new Location[32];
            for (int movement = 0; movement < 32; movement += 4)
            {
                pieceMovements[movement] = new Location(-movement / 4, -movement / 4);
                pieceMovements[movement + 1] = new Location(movement / 4, -movement / 4);
                pieceMovements[movement + 2] = new Location(-movement / 4, movement / 4);
                pieceMovements[movement + 3] = new Location(movement / 4, movement / 4);
            };

            capturesLikeItsMove = true;
        }
    }

    internal class Rook : Piece
    {
        public Rook()
        {
            pieceType = PieceType.Rook;

            pieceMovements = new Location[]
            {
            new Location(-1, 0),
            new Location(1, 0),
            new Location(0, -1),
            new Location(0, 1)
            };

            capturesLikeItsMove = true;
        }
    }

    internal class Queen : Piece
    {
        public Queen()
        {
            pieceType = PieceType.Queen;

            pieceMovements = new Location[]
            {
            new Location(-1, -1),
            new Location(-1, 1),
            new Location(1, -1),
            new Location(1, 1),
            new Location(-1, 0),
            new Location(1, 0),
            new Location(0, -1),
            new Location(0, 1)
            };

            capturesLikeItsMove = true;
        }
    }

    internal class King : Piece
    {
        public King()
        {
            pieceType = PieceType.King;

            pieceMovements = new Location[]
            {
            new Location(-1, -1),
            new Location(-1, 0),
            new Location(-1, 1),
            new Location(0, -1),
            new Location(0, 1),
            new Location(1, -1),
            new Location(1, 0),
            new Location(1, 1)
            };

            capturesLikeItsMove = true;
        }
    }
}
