using ChessGame.logic;

namespace ChessGame
{
    public class GUI
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

    public partial class Form1 : Form
    {
        const int SQUARE_SIZE = 80;
        Board gameBoard = Board.GetBoard();
        PictureBox[,] chessboardPictureBoxes = GUI.ChessGui().chessboardPictureBoxes;

        public Form1()
        {
            InitializeComponent();
            displayB();

        }
        public void displayB()
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
                        Image = GUI.ChessGui().LoadPieceImage(gameBoard.matrix[rank, file]._pieceOnSquare),
                        SizeMode = PictureBoxSizeMode.StretchImage
                    };
                    chessboardPictureBoxes[rank, file].Click += PictureBox_Click;
                    Controls.Add(chessboardPictureBoxes[rank, file]);
                }
            }
        }


        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;

            if (clickedPictureBox != null)
            {
                int rank = clickedPictureBox.Location.Y / SQUARE_SIZE;
                int file = clickedPictureBox.Location.X / SQUARE_SIZE;

                // Get the piece on the BoardSquare
                Piece clickedPiece = gameBoard.matrix[rank, file]._pieceOnSquare;

                if (clickedPiece != null && clickedPiece.pieceColor == Game.playerTurnColor)
                {
                    // Get the possible movements for the clicked piece
                    List<Location> possibleMovements = clickedPiece.getAvailableMovesOnBoard(new Location(rank, file));
                    if (Game.check)
                    {
                        if (clickedPiece is not King)
                            Checker.removeInvalidMovesForCheck(possibleMovements);
                        else
                            possibleMovements.RemoveAll(location => Game.attackLocations.Contains(location));

                        if (possibleMovements.Count == 0)
                        {
                            Label checkmate = GUI.ChessGui().handleCheckMate();
                            Controls.Add(checkmate);
                            return;
                        }
                    }

                    UpdateSquareColors(possibleMovements);
                    Game.clickedLocation = new Location(rank, file);
                    Game.possibleMovements = possibleMovements;
                }
                else if (Game.clickedLocation.Rank != -1 && Game.clickedLocation.File != -1)
                {
                    Location newLocation = new Location(rank, file);
                    if (Game.possibleMovements.Contains(newLocation))
                    {
                        Game.movePieceFromSquareToSquare(Game.clickedLocation, new Location(rank, file));
                        Game.clickedLocation = new Location(-1, -1);

                        List<Location> newPossibleMoves = Board.GetBoard().matrix[rank, file]._pieceOnSquare.getAvailableMovesOnBoard(newLocation);
                        ResetSquareColors();
                        if (Game.check)
                        {
                            Game.checkingLocation = new Location(-1, -1);
                            Game.check = false;
                        }
                        foreach (Location location in newPossibleMoves)
                        {
                            Piece piece = Board.GetBoard().matrix[location.Rank, location.File]._pieceOnSquare;
                            if (piece != null && piece is King && piece.pieceColor != Game.playerTurnColor)
                            {
                                chessboardPictureBoxes[location.Rank, location.File].BackColor = Color.Red;
                                Game.checkingLocation = new Location(rank, file);
                                Game.check = true;
                            }
                        }
                        Game.attackLocations = Game.getAllAttackedLocations();
                        Game.playerTurnColor = Game.playerTurnColor == Color.White ? Color.Black : Color.White;
                    }
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

        public void ResetPiecesPictures()
        {
            for (int rank = 0; rank < Board.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Board.NUMBER_OF_FILES; file++)
                {
                    chessboardPictureBoxes[rank, file].Image = null;
                }
            }
        }

    }
}