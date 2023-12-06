using ChessGame.GameNamespace;
using ChessGame.global;
using ChessGame.Global;
using ChessGame.Statics;

namespace ChessGame
{
    public partial class GUI : Form
    {
        private static Form? instance;
        static readonly PictureBox[,] chessboardPictureBoxes = Game.chessboardPictureBoxes;

        public GUI()
        {
            instance = this;
            InitializeComponent();
            DisplayBoard(Static.WHITE_PLAYER_UP);
        }
        public static Form? Instance { get { return instance; } }
        public static PieceType ShowCustomPieceTypeDialog()
        {
            using (var promoteDialog = new PromoteDialog())
            {
                DialogResult result = promoteDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // User clicked one of the piece type buttons
                    return promoteDialog.SelectedPieceType;

                }
                return PieceType.Queen;
                // You can optionally handle DialogResult.Cancel if needed
            }
        }

        public void DisplayBoard(bool isWhiteUp)
        {
            // Populate the chessboardPictureBoxes array
            for (int rank = 0; rank < Static.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Static.NUMBER_OF_FILES; file++)
                {
                    Location chessTileLocation = isWhiteUp ? new Location(rank, file) : new Location(7 - rank, 7 - file);
                    chessboardPictureBoxes[rank, file] = new ChessTile(
                            location: chessTileLocation,
                            color: Statics.Static.GetSquareColor(rank, file),
                            image: Game.GetInstance().GetImage(rank, file),
                            onPressed: (tileLocation) => Game.GetInstance().HandlePieceClick(isWhiteUp ? tileLocation : tileLocation.Inverted)
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
            for (int rank = 0; rank < Static.NUMBER_OF_RANKS; rank++)
                for (int file = 0; file < Static.NUMBER_OF_FILES; file++)
                    chessboardPictureBoxes[rank, file].BackColor = Statics.Static.GetSquareColor(rank, file);
        }

        public static void ShowCheckmateWindows()
        {
            CheckmateWindow checkmateWindow = new();
            checkmateWindow.ShowDialog();
            Application.Exit();
        }
    }
}