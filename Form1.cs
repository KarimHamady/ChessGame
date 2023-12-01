using ChessGame.logic;

namespace ChessGame
{
    public partial class Form1 : Form
    {
        const int SQUARE_SIZE = 80;
        Board gameBoard = Board.GetBoard();
        PictureBox[,] chessboardPictureBoxes = new PictureBox[Board.NUMBER_OF_RANKS, Board.NUMBER_OF_FILES];
        public Form1()
        {
            InitializeComponent();




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
                        Image = LoadPieceImage(gameBoard.matrix[rank, file]._pieceOnSquare, rank < 2 ? 'w' : 'b'),
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
                Piece clickedPiece = gameBoard.matrix[rank, file]._pieceOnSquare;

                if (clickedPiece != null)
                {
                    // Get the possible movements for the clicked piece
                    Location[] pieceMovements = clickedPiece.pieceMovements;
                    List<Location> possibleMovements = new List<Location>();
                    for (int i = 0; i < pieceMovements.Length; i++)
                    {
                        int newRank = rank + pieceMovements[i].Rank;
                        int newFile = file + pieceMovements[i].File;
                        if (newRank >= 0 && newRank < Board.NUMBER_OF_RANKS && newFile >= 0 && newFile < Board.NUMBER_OF_FILES)
                            possibleMovements.Add(new Location(newRank, newFile));
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
}