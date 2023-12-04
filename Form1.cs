using ChessGame.view;
using ChessGame.global;
using ChessGame.statics;

namespace ChessGame
{
    public partial class Form1 : Form
    {
        private static Form instance;
        const int SQUARE_SIZE = 80;
        static PictureBox[,] chessboardPictureBoxes = Facade.GetInstance().chessboardPictureBoxes;

        public Form1()
        {
            instance = this;
            InitializeComponent();
            displayB();

        }

        public void displayB()
        {
            // Populate the chessboardPictureBoxes array
            for (int rank = 0; rank < Constants.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Constants.NUMBER_OF_FILES; file++)
                {
                    chessboardPictureBoxes[rank, file] = new PictureBox
                    {
                        Size = new Size(SQUARE_SIZE, SQUARE_SIZE),
                        Location = new Point(file * SQUARE_SIZE, rank * SQUARE_SIZE),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Facade.GetColor(rank, file),
                        Image = Facade.GetImage(rank, file),
                        SizeMode = PictureBoxSizeMode.Zoom,
                    };
                    chessboardPictureBoxes[rank, file].Click += PictureBox_Click;
                    Controls.Add(chessboardPictureBoxes[rank, file]);
                }
            }
        }


        private void PictureBox_Click(object sender, EventArgs e)
        {
            //use adapter
            Location clickedLocation = new(-1, -1);

            if (sender is PictureBox clickedPictureBox)
            {
                clickedLocation.Rank = clickedPictureBox.Location.Y / SQUARE_SIZE;
                clickedLocation.File = clickedPictureBox.Location.X / SQUARE_SIZE;
            }

            if (clickedLocation.Rank != -1)
            {
                Facade.GetInstance().HandlePieceClick(clickedLocation);
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
                    chessboardPictureBoxes[rank, file].BackColor = Facade.GetColor(rank, file);
        }



        public void ResetPiecesPictures()
        {
            foreach (PictureBox pictureBox in chessboardPictureBoxes)
                pictureBox.Image = null;
        }

        public static void AddCheckmate()
        {
            Label checkmate = view.Facade.GetInstance().handleCheckMate();
            instance.Controls.Add(checkmate);
        }
    }
}