using ChessGame.FacadeNamespace;
using ChessGame.global;
using ChessGame.Global;
using ChessGame.Logic.BoardNamespace;
using ChessGame.Statics;

namespace ChessGame
{
    public partial class GUI : Form
    {
        private static Form? instance;
        static readonly PictureBox[,] chessboardPictureBoxes = Facade.chessboardPictureBoxes;

        public GUI()
        {
            instance = this;
            InitializeComponent();
            Board.GetBoard();
            DisplayBoard();

        }

        public void DisplayBoard()
        {
            // Populate the chessboardPictureBoxes array
            for (int rank = 0; rank < Constants.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Constants.NUMBER_OF_FILES; file++)
                {
                    Location chessTileLocation = new Location(rank, file);
                    chessboardPictureBoxes[rank, file] = new ChessTile(
                            location: chessTileLocation,
                            color: Static.GetSquareColor(rank, file),
                            image: Facade.GetImage(rank, file),
                            onPressed: (chessTileLocation) => Facade.HandlePieceClick(chessTileLocation)
                        );
                    Controls.Add(chessboardPictureBoxes[rank, file]);
                }
            }
        }

        public static void ColorLocations(List<Location> locations, Color color)
        {
            foreach (Location location in locations)
                chessboardPictureBoxes[location.Rank, location.File].BackColor = color;
        }

        public static void ResetSquareColors()
        {
            for (int rank = 0; rank < Constants.NUMBER_OF_RANKS; rank++)
                for (int file = 0; file < Constants.NUMBER_OF_FILES; file++)
                    chessboardPictureBoxes[rank, file].BackColor = Static.GetSquareColor(rank, file);
        }

        public static void AddCheckmateLabel()
        {
            Label checkmateLabel = new()
            {
                Text = "Checkmate!",
                Font = new Font("Arial", 24, FontStyle.Bold), // Adjust font size and style
                Location = new Point(Constants.CHECK_LABEL_LOCATION, 100),
                AutoSize = true
            };
            instance!.Controls.Add(checkmateLabel);
        }

        public static void AddCheckLabel()
        {
            Label checkmateLabel = new()
            {
                Text = "Check!",
                Font = new Font("Arial", 24, FontStyle.Bold), // Adjust font size and style
                Location = new Point(Constants.CHECK_LABEL_LOCATION, 100),
                AutoSize = true
            };
            instance!.Controls.Add(checkmateLabel);
        }
    }
}