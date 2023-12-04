using ChessGame.logic;
using ChessGame.global;
using ChessGame.statics;

namespace ChessGame.view
{
    internal class Facade
    {
        private static Facade? instance;
        const int SQUARE_SIZE = 80;
        static Board gameBoard = Board.GetBoard();
        public PictureBox[,] chessboardPictureBoxes = new PictureBox[Constants.NUMBER_OF_RANKS, Board.NUMBER_OF_FILES];
        private Facade() { }
        public static Facade GetInstance()
        {
            if (instance == null)
                instance = new Facade();
            return instance;
        }
        public void updateImageAtLocation(Location location)
        {
            chessboardPictureBoxes[location.Rank, location.File].Image = LoadPieceImage(Board.GetBoard().matrix[location.Rank, location.File]._pieceOnSquare);
        }
        public void removeImageAtLocation(Location location)
        {
            chessboardPictureBoxes[location.Rank, location.File].Image = null;
        }
        public static Image? LoadPieceImage(Piece piece)
        {
            if (piece == null)
                return null;

            char colorPrefix = piece.pieceColor == Color.White ? 'w' : 'b';
            String? name = Enum.GetName(typeof(statics.PieceType), piece.pieceType);
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
        public void HandlePieceClick(Location clickLocation) {
            int rank = clickLocation.Rank;
            int file = clickLocation.File;
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
                        Form1.AddCheckmate();
                        return;
                    }
                }

                Form1.ResetSquareColors();
                Form1.ColorLocations(possibleMovements, Color.Green);
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

                    Form1.ResetSquareColors();
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


        public static void LimitPiecesMovements(Piece clickedPiece, List<Location> possibleMovements)
        {
            if (clickedPiece is not King)
                Checker.removeInvalidMovesForCheck(possibleMovements);
            else
                possibleMovements.RemoveAll(location => Game.attackLocations.Contains(location));
        }

        public static Color GetColor(int rank, int file)
        {
            return gameBoard.matrix[rank, file]._color;
        }

        public static Image? GetImage(int rank, int file)
        {
            return LoadPieceImage(gameBoard.matrix[rank, file]._pieceOnSquare);
        }
    }
}
