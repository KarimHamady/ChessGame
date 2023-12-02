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
        const int SQUARE_SIZE = 80;
        PictureBox[,] chessboardPictureBoxes = new PictureBox[Board.NUMBER_OF_RANKS, Board.NUMBER_OF_FILES];
        Dictionary<PictureBox, Piece> pictureToPieceMap = new Dictionary<PictureBox, Piece>();

        private static Board gameBoard = null;
        public BoardSquare[,] matrix;

        public const int NUMBER_OF_RANKS = 8;
        public const int NUMBER_OF_FILES = 8;

        private Board()
        {
            matrix = new BoardSquare[NUMBER_OF_RANKS, NUMBER_OF_FILES];
            addColorsToSquares();
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

            List<Piece> chessPieces = new List<Piece>()
            {
                new Rook  (pieceColor, new Location(0, 0)),
                new Knight(pieceColor, new Location(0, 1)),
                new Bishop(pieceColor, new Location(0, 2)),
                new King(pieceColor, new Location(0, 3)),
                new Queen(pieceColor, new Location(0, 4)),
                new Bishop(pieceColor, new Location(0, 5)),
                new Knight(pieceColor, new Location(0, 6)),
                new Rook(pieceColor, new Location(0, 7))
            };


            // Add white pawns on the second rank
            for (int file = 0; file < NUMBER_OF_FILES; file++)
            {
                chessPieces.Add(new Pawn(pieceColor, new Location(1, file)));
            }

            // Add black pawns on the seventh rank
            pieceColor = Color.Black;
            for (int file = 0; file < NUMBER_OF_FILES; file++)
            {
                chessPieces.Add(new Pawn(pieceColor, new Location(6, file)));
            }

            // Add black pieces on the eighth rank
            chessPieces.AddRange(new List<Piece>()
            {
                new Rook  (pieceColor, new Location(7, 0)),
                new Knight(pieceColor, new Location(7, 1)),
                new Bishop(pieceColor, new Location(7, 2)),
                new Queen(pieceColor, new Location(7, 4)),
                new King(pieceColor, new Location(7, 3)),
                new Bishop(pieceColor, new Location(7, 5)),
                new Knight(pieceColor, new Location(7, 6)),
                new Rook(pieceColor, new Location(7, 7))
            });

        }

        public void displayBoard()
        {
            // Populate the chessboardPictureBoxes array
            for (int rank = 0; rank < Board.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Board.NUMBER_OF_FILES; file++)
                {
                    chessboardPictureBoxes[rank, file] = new PictureBox
                    {
                        Size = new Size(SQUARE_SIZE, SQUARE_SIZE),
                        Location = new Point(file * SQUARE_SIZE, rank * SQUARE_SIZE),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = gameBoard.matrix[rank, file]._color == SquareColor.White ? Color.White : Color.Black,
                        SizeMode = PictureBoxSizeMode.StretchImage
                    };
                    chessboardPictureBoxes[rank, file].Click += PictureBox_Click;
                    Controls.Add(chessboardPictureBoxes[rank, file]);
                }
            }
        }

        private Image LoadPieceImage(Piece piece, char colorPrefix)
        {
            if (piece == null)
                return null;

            String name = Enum.GetName(typeof(PieceType), piece.pieceType);
            // Assuming the piece images are stored in a "data" folder within your project
            string imagePath = $"../../../data/{(name != null ? name.ToLower() : "")}_{colorPrefix}.png";

            if (System.IO.File.Exists(imagePath))
            {
                return Image.FromFile(imagePath);
            }

            // Return null if the image doesn't exist
            return null;
        }
        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;

            if (clickedPictureBox != null)
            {
                // Find the corresponding chessboard position
                int rank = clickedPictureBox.Location.Y / SQUARE_SIZE;
                int file = clickedPictureBox.Location.X / SQUARE_SIZE;

                // Get the piece on the BoardSquare
                Piece clickedPiece = pictureToPieceMap[clickedPictureBox];

                if (clickedPiece != null && clickedPiece.pieceColor == Game.playerTurnColor)
                {
                    // Get the possible movements for the clicked piece
                    List<Location> pieceMovements = clickedPiece.pieceMovements;
                    List<Location> possibleMovements = new List<Location>();
                    foreach (Location location in pieceMovements)
                    {
                        int newRank = rank + location.Rank;
                        int newFile = file + location.File;
                        Location newLocation = new Location(newRank, newFile);
                        if (Checker.isMoveWithinBoard(newLocation))
                            possibleMovements.Add(newLocation);
                    }
                    // Update the colors of the corresponding squares to green
                    UpdateSquareColors(possibleMovements);
                }
            }
        }

        private void UpdateSquareColors(List<Location> locations)
        {
            // Reset all square colors to their original colors
            ResetSquareColors();

            // Update the colors of squares with valid moves to green
            foreach (Location location in locations)
            {
                chessboardPictureBoxes[location.Rank, location.File].BackColor = Color.Green;
            }
        }

        private void ResetSquareColors()
        {
            // Reset all square colors to their original colors
            for (int rank = 0; rank < Board.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Board.NUMBER_OF_FILES; file++)
                {
                    chessboardPictureBoxes[rank, file].BackColor = gameBoard.matrix[rank, file]._color == SquareColor.White ? Color.White : Color.Black;
                }
            }
        }
    }

    internal class BoardSquare
    {
        public SquareColor _color;

        public BoardSquare(SquareColor color)
        {
            _color = color;
        }
    }
}
