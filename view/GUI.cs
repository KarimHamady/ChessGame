using ChessGame.logic;

namespace ChessGame.view
{
    internal class GUI
    {
        private static GUI chessGUI = null;
        public PictureBox[,] chessboardPictureBoxes = new PictureBox[Board.NUMBER_OF_RANKS, Board.NUMBER_OF_FILES];
        private GUI()
        { }
        public static GUI ChessGui()
        {
            if (chessGUI == null)
                chessGUI = new GUI();
            return chessGUI;
        }
        public void updateImageAtLocation(Location location)
        {
            chessboardPictureBoxes[location.Rank, location.File].Image = LoadPieceImage(Board.GetBoard().matrix[location.Rank, location.File]._pieceOnSquare);
        }
        public void removeImageAtLocation(Location location)
        {
            chessboardPictureBoxes[location.Rank, location.File].Image = null;
        }
        public Image LoadPieceImage(Piece piece)
        {
            if (piece == null)
                return null;

            char colorPrefix = piece.pieceColor == Color.White ? 'w' : 'b';
            String name = Enum.GetName(typeof(statics.PieceType), piece.pieceType);
            string imagePath = $"../../../data/{(name != null ? name.ToLower() : "")}_{colorPrefix}.png";

            return System.IO.File.Exists(imagePath) ? Image.FromFile(imagePath) : null;
        }
        public Label handleCheckMate()
        {
            Label checkmateLabel = new Label();
            checkmateLabel.Text = "Checkmate!";
            checkmateLabel.Font = new Font("Arial", 24, FontStyle.Bold); // Adjust font size and style
            checkmateLabel.Location = new Point(700, 100);
            checkmateLabel.AutoSize = true;

            return checkmateLabel;
        }
    }
}
