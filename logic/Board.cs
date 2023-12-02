using System.Text;

namespace ChessGame.logic
{
    enum SquareColor
    {
        Black,
        White
    }

    internal struct Location
    {
        public int Rank { get; set; }
        public int File { get; set; }
        public Location(int rank, int file)
        {
            Rank = rank;
            File = file;
        }
        public static Location operator +(Location a, Location b)
        {
            return new Location(a.Rank + b.Rank, a.File + b.File);
        }

    }
    internal class Board
    {
        private static Board gameBoard = null;
        public BoardSquare[,] matrix;

        public const int NUMBER_OF_RANKS = 8;
        public const int NUMBER_OF_FILES = 8;

        private Board()
        {
            matrix = new BoardSquare[NUMBER_OF_RANKS, NUMBER_OF_FILES];
            addColorsToSquares();
            addPiecesToBoard();
        }
        public static Board GetBoard()
        {
            if (gameBoard == null)
                gameBoard = new Board();
            return gameBoard;
        }
        private void addColorsToSquares()
        {
            for (int rank = 0; rank < NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < NUMBER_OF_FILES; file++)
                {
                    SquareColor color = (rank + file) % 2 == 0 ? SquareColor.White : SquareColor.Black;
                    matrix[rank, file] = new BoardSquare(color);
                }
            }
        }
        private void addPiecesToBoard()
        {
            // Add white pieces on the first rank
            Color pieceColor = Color.White;
            matrix[0, 0].AddPieceToSquare(new Rook(pieceColor));
            matrix[0, 1].AddPieceToSquare(new Knight(pieceColor));
            matrix[0, 2].AddPieceToSquare(new Bishop(pieceColor));
            matrix[0, 3].AddPieceToSquare(new King(pieceColor));
            matrix[0, 4].AddPieceToSquare(new Queen(pieceColor));
            matrix[0, 5].AddPieceToSquare(new Bishop(pieceColor));
            matrix[0, 6].AddPieceToSquare(new Knight(pieceColor));
            matrix[0, 7].AddPieceToSquare(new Rook(pieceColor));

            // Add white pawns on the second rank
            for (int file = 0; file < NUMBER_OF_FILES; file++)
            {
                matrix[1, file].AddPieceToSquare(new Pawn(pieceColor));
            }

            // Add black pawns on the seventh rank
            pieceColor = Color.Black;
            for (int file = 0; file < NUMBER_OF_FILES; file++)
            {
                matrix[6, file].AddPieceToSquare(new Pawn(pieceColor));
            }

            // Add black pieces on the eighth rank
            matrix[7, 0].AddPieceToSquare(new Rook(pieceColor));
            matrix[7, 1].AddPieceToSquare(new Knight(pieceColor));
            matrix[7, 2].AddPieceToSquare(new Bishop(pieceColor));
            matrix[7, 3].AddPieceToSquare(new King(pieceColor));
            matrix[7, 4].AddPieceToSquare(new Queen(pieceColor));
            matrix[7, 5].AddPieceToSquare(new Bishop(pieceColor));
            matrix[7, 6].AddPieceToSquare(new Knight(pieceColor));
            matrix[7, 7].AddPieceToSquare(new Rook(pieceColor));
        }

        public void DisplayBoard()
        {
            StringBuilder boardString = new StringBuilder();

            for (int rank = 0; rank < NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < NUMBER_OF_FILES; file++)
                {
                    Piece piece = matrix[rank, file]._pieceOnSquare;

                    if (piece != null)
                    {
                        boardString.Append(piece.pieceType);
                    }
                    else
                    {
                        boardString.Append("-");
                    }

                    boardString.Append(" ");
                }

                boardString.AppendLine(); // Move to the next rank
            }

            MessageBox.Show(boardString.ToString(), "Chess Board");
        }


    }

    internal class BoardSquare
    {
        public SquareColor _color;
        public Piece _pieceOnSquare = null;

        public BoardSquare(SquareColor color)
        {
            _color = color;
        }

        public BoardSquare(SquareColor color, Piece pieceOnSquare) : this(color)
        {
            _pieceOnSquare = pieceOnSquare;
        }

        public void AddPieceToSquare(Piece piece)
        {
            _pieceOnSquare = piece;
        }

        public void RemovePieceFromSquare()
        {
            if (_pieceOnSquare == null)
            {
                throw new InvalidOperationException("There is no piece on the square to remove.");
            }
            _pieceOnSquare = null; //FIXME
        }

    }
}
