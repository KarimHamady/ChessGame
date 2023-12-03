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
            capturesLikeItsMove = false;
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
    }

    internal class Bishop : Piece
    {
        public Bishop(Color color)
        {
            pieceType = PieceType.Bishop;
            pieceColor = color;
            capturesLikeItsMove = true;
        }
    }

    internal class Rook : Piece
    {
        public Rook(Color color)
        {
            pieceType = PieceType.Rook;
            pieceColor = color;
            capturesLikeItsMove = true;
        }
    }

    internal class Queen : Piece
    {
        public Queen(Color color)
        {
            pieceType = PieceType.Queen;
            pieceColor = color;
            capturesLikeItsMove = true;
        }
    }

    internal class King : Piece
    {
        public King(Color color)
        {
            pieceType = PieceType.King;
            pieceColor = color;
            capturesLikeItsMove = true;
        }
    }
}
