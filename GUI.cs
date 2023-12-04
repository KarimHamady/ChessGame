using ChessGame.FacadeNamespace;
using ChessGame.global;
using ChessGame.Global;
using ChessGame.Statics;

namespace ChessGame
{
    public partial class GUI : Form
    {
        private static Form? instance;
        static readonly PictureBox[,] chessboardPictureBoxes = Facade.GetInstance().chessboardPictureBoxes;

        public GUI()
        {
            instance = this;
            InitializeComponent();
            DisplayBoard();

        }

        public void DisplayBoard()
        {
            // Populate the chessboardPictureBoxes array
            for (int rank = 0; rank < Constants.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Constants.NUMBER_OF_FILES; file++)
                {
                    /*chessboardPictureBoxes[rank, file] = new PictureBox
                    {
                        Size = new Size(Constants.SQUARE_SIZE, Constants.SQUARE_SIZE),
                        Location = new Point(file * Constants.SQUARE_SIZE, rank * Constants.SQUARE_SIZE),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Facade.GetColor(rank, file),
                        Image = Facade.GetImage(rank, file),
                        SizeMode = PictureBoxSizeMode.Zoom,
                    };
                    chessboardPictureBoxes[rank, file].Click += PictureBox_Click;*/
                    Location chessTileLocation = new Location(rank, file);
                    chessboardPictureBoxes[rank, file] = new ChessTile(
                            location: chessTileLocation,
                            color: Facade.GetColor(rank, file),
                            image: Facade.GetImage(rank, file),
                            onPressed: (chessTileLocation) => Facade.HandlePieceClick(chessTileLocation)
                        );
                    Controls.Add(chessboardPictureBoxes[rank, file]);
                }
            }
        }


        /*private void PictureBox_Click(object? sender, EventArgs e)
        {
            //use adapter
            Location clickedLocation = new(-1, -1);

            if (sender is PictureBox clickedPictureBox)
            {
                clickedLocation.Rank = clickedPictureBox.Location.Y / Constants.SQUARE_SIZE;
                clickedLocation.File = clickedPictureBox.Location.X / Constants.SQUARE_SIZE;
            }

            if (clickedLocation.Rank != -1)
            {
                Facade.HandlePieceClick(clickedLocation);
            }
        }*/


        public static void ColorLocations(List<Location> locations, Color color)
        {
            foreach (Location location in locations)
                chessboardPictureBoxes[location.Rank, location.File].BackColor = color;
        }

        public static void ResetSquareColors()
        {
            for (int rank = 0; rank < Constants.NUMBER_OF_RANKS; rank++)
                for (int file = 0; file < Constants.NUMBER_OF_FILES; file++)
                    chessboardPictureBoxes[rank, file].BackColor = FacadeNamespace.Facade.GetColor(rank, file);
        }

        public static void ResetPiecesPictures()
        {
            foreach (PictureBox pictureBox in chessboardPictureBoxes)
                pictureBox.Image = null;
        }

        public static void AddCheckmateLabel()
        {
            Label checkmateLabel = new()
            {
                Text = "Checkmate!",
                Font = new Font("Arial", 24, FontStyle.Bold), // Adjust font size and style
                Location = new Point(8 * Constants.SQUARE_SIZE + 14, 100),
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
                Location = new Point(8 * Constants.SQUARE_SIZE + 14, 100),
                AutoSize = true
            };
            instance!.Controls.Add(checkmateLabel);
        }
    }
}