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
        protected int[,] pieceMovements = { };
        protected bool capturesLikeItsMove = false;
    }

    internal class Pawn : Piece
    {
        public Pawn()
        {
            pieceType = PieceType.Pawn;

            pieceMovements = new int[,]
            {
            {1, 0}, // Move forward
            {2, 0}, // Move forward (initial double move)
            {-1, 0}, // Capture diagonally left
            {-1, 0} // Capture diagonally right
                    // Add other pawn moves as needed
            };

            capturesLikeItsMove = false;
        }
    }

    internal class Knight : Piece
    {
        public Knight()
        {
            pieceType = PieceType.Knight;

            pieceMovements = new int[,]
            {
            {-2, -1},
            {-2, 1},
            {2, -1},
            {2, 1},
            {-1, -2},
            {-1, 2},
            {1, -2},
            {1, 2}
            };

            capturesLikeItsMove = true;
        }
    }

    internal class Bishop : Piece
    {
        public Bishop()
        {
            pieceType = PieceType.Bishop;

            pieceMovements = new int[,]
            {
            {-1, -1},
            {-1, 1},
            {1, -1},
            {1, 1}
            };

            capturesLikeItsMove = true;
        }
    }

    internal class Rook : Piece
    {
        public Rook()
        {
            pieceType = PieceType.Rook;

            pieceMovements = new int[,]
            {
            {-1, 0},
            {1, 0},
            {0, -1},
            {0, 1}
            };

            capturesLikeItsMove = true;
        }
    }

    internal class Queen : Piece
    {
        public Queen()
        {
            pieceType = PieceType.Queen;

            pieceMovements = new int[,]
            {
            {-1, -1},
            {-1, 1},
            {1, -1},
            {1, 1},
            {-1, 0},
            {1, 0},
            {0, -1},
            {0, 1}
            };

            capturesLikeItsMove = true;
        }
    }

    internal class King : Piece
    {
        public King()
        {
            pieceType = PieceType.King;

            pieceMovements = new int[,]
            {
            {-1, -1},
            {-1, 0},
            {-1, 1},
            {0, -1},
            {0, 1},
            {1, -1},
            {1, 0},
            {1, 1}
            };

            capturesLikeItsMove = true;
        }
    }
}
