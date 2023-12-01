using ChessGame.logic;

namespace ChessGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            const int SQUARE_SIZE = 80;
            Board gameBoard = Board.GetBoard();
            PictureBox[,] chessboardPictureBoxes = new PictureBox[Board.NUMBER_OF_RANKS, Board.NUMBER_OF_FILES];

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
    }
}