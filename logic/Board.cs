using ChessGame.Global;
using ChessGame.Logic.PieceNamespace;
using ChessGame.Statics;

namespace ChessGame.Logic
{
    namespace BoardNamespace
    {
        internal class Board
        {
            private static Board? gameBoard = null;
            public Piece?[,] matrix;

            private Board()
            {
                matrix = new Piece[Constants.NUMBER_OF_RANKS, Constants.NUMBER_OF_FILES];
                AddPiecesToBoard();
            }
            public static Board GetBoard()
            {
                if (gameBoard == null)
                    gameBoard = new Board();
                return gameBoard;
            }

            public static void AddPieceAtLocation(Piece piece, Location location)
            {
                Board.GetBoard().matrix[location.Rank, location.File] = piece;
            }

            public static void RemovePieceAtLocation(Location location)
            {
                Board.GetBoard().matrix[location.Rank, location.File] = null;
            }

            public static Piece GetPieceAtLocation(Location location)
            {
                return gameBoard.matrix[location.Rank, location.File] != null ? gameBoard.matrix[location.Rank, location.File] : null;
            }
            private void AddPiecesToBoard()
            {
                // Add white pieces on the first rank
                Color pieceColor = Color.White;
                matrix[0, 0] = new Rook(pieceColor, RookSide.KingSide);
                matrix[0, 1] = new Knight(pieceColor);
                matrix[0, 2] = new Bishop(pieceColor);
                matrix[0, 3] = new King(pieceColor);
                matrix[0, 4] = new Queen(pieceColor);
                matrix[0, 5] = new Bishop(pieceColor);
                matrix[0, 6] = new Knight(pieceColor);
                matrix[0, 7] = new Rook(pieceColor, RookSide.QueenSide);

                // Add white pawns on the second rank
                for (int file = 0; file < Constants.NUMBER_OF_FILES; file++)
                {
                    matrix[1, file] = new Pawn(pieceColor);
                }

                // Add black pawns on the seventh rank
                pieceColor = Color.Black;
                for (int file = 0; file < Constants.NUMBER_OF_FILES; file++)
                {
                    matrix[6, file] = new Pawn(pieceColor);
                }

                // Add black pieces on the eighth rank
                matrix[7, 0] = new Rook(pieceColor, RookSide.KingSide);
                matrix[7, 1] = new Knight(pieceColor);
                matrix[7, 2] = new Bishop(pieceColor);
                matrix[7, 3] = new King(pieceColor);
                matrix[7, 4] = new Queen(pieceColor);
                matrix[7, 5] = new Bishop(pieceColor);
                matrix[7, 6] = new Knight(pieceColor);
                matrix[7, 7] = new Rook(pieceColor, RookSide.QueenSide);
            }
        }


    }
}
