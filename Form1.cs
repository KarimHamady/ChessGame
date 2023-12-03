using ChessGame.logic;

namespace ChessGame
{
    public partial class Form1 : Form
    {
        const int SQUARE_SIZE = 80;
        Board gameBoard = Board.GetBoard();
        PictureBox[,] chessboardPictureBoxes = view.GUI.ChessGui().chessboardPictureBoxes;

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
                        BackColor = gameBoard.matrix[rank, file]._color,
                        Image = view.GUI.ChessGui().LoadPieceImage(gameBoard.matrix[rank, file]._pieceOnSquare),
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
                        LimitPiecesMovements(clickedPiece, possibleMovements);
                        if (possibleMovements.Count == 0)
                        {
                            Label checkmate = view.GUI.ChessGui().handleCheckMate();
                            Controls.Add(checkmate);
                            return;
                        }
                    }

                    ResetSquareColors();
                    ColorLocations(possibleMovements, Color.Green);
                    Game.clickedLocation = new Location(rank, file);
                    Game.possibleMovements = possibleMovements;
                }
                else if (Game.clickedLocation.Rank != -1 && Game.clickedLocation.File != -1)
                {
                    Location newLocation = new Location(rank, file);
                    if (Game.possibleMovements.Contains(newLocation))
                    {
                        Game.movePieceFromSquareToSquare(Game.clickedLocation, new Location(rank, file));
                        Game.UpdateCastlingCondition(gameBoard.matrix[Game.clickedLocation.Rank, Game.clickedLocation.File]);
                        if (gameBoard.matrix[newLocation.Rank, newLocation.File]._pieceOnSquare is King)
                            Game.CheckAndHandleCastling(Game.clickedLocation, newLocation);

                        ResetSquareColors();
                        Game.ResetGameCheckVariables();
                        foreach (Location location in Board.GetBoard().matrix[rank, file]._pieceOnSquare.getAvailableMovesOnBoard(newLocation))
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


        private void ColorLocations(List<Location> locations, Color color)
        {
            foreach (Location location in locations)
                chessboardPictureBoxes[location.Rank, location.File].BackColor = color;
        }

        private void ResetSquareColors()
        {
            for (int rank = 0; rank < Board.NUMBER_OF_RANKS; rank++)
                for (int file = 0; file < Board.NUMBER_OF_FILES; file++)
                    chessboardPictureBoxes[rank, file].BackColor = gameBoard.matrix[rank, file]._color;
        }

        public void LimitPiecesMovements(Piece clickedPiece, List<Location> possibleMovements)
        {
            if (clickedPiece is not King)
                Checker.removeInvalidMovesForCheck(possibleMovements);
            else
                possibleMovements.RemoveAll(location => Game.attackLocations.Contains(location));
        }


        public void ResetPiecesPictures()
        {
            foreach (PictureBox pictureBox in chessboardPictureBoxes)
                pictureBox.Image = null;
        }

    }
}