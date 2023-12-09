using ChessGame.GameNamespace;
using ChessGame.Global;

namespace ChessGame
{
    public partial class GUI : Form
    {
        private static Form? instance;
        static PictureBox[,] chessboardPictureBoxes = Game.chessboardPictureBoxes;

        public GUI()
        {
            instance = this;
            InitializeComponent();
            panel1.Size = new Size(Static.SQUARE_SIZE * 8, Static.SQUARE_SIZE * 8);
            panel2.Location = new Point(Static.SQUARE_SIZE * 8, 0);
            panel2.Size = new Size(200, Static.SQUARE_SIZE * 8);
            InitializeButtons();
            DisplayBoard(Static.WHITE_PLAYER_UP);
        }
        public static PieceType ShowPromoteDialog()
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

        public static void ShowCheckmateDialog()
        {
            new CheckmateDialog().ShowDialog();
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
                            color: Static.GetSquareColor(rank, file),
                            image: Game.GetInstance().GetPieceImageAt(rank, file),
                            onPressed: (tileLocation) => Game.GetInstance().HandlePieceClick(isWhiteUp ? tileLocation : tileLocation.Inverted)
                        );
                    panel1.Controls.Add(chessboardPictureBoxes[rank, file]);
                }
            }
        }


        public static void ColorLocations(List<Location> locations, Color color)
        {
            foreach (Location location in locations)
            {
                chessboardPictureBoxes[location.Rank, location.File].BackColor = color;
                chessboardPictureBoxes[location.Rank, location.File].BorderStyle = BorderStyle.FixedSingle;

            }
        }

        public static void ResetSquareColors()
        {
            for (int rank = 0; rank < Static.NUMBER_OF_RANKS; rank++)
                for (int file = 0; file < Static.NUMBER_OF_FILES; file++)
                {
                    chessboardPictureBoxes[rank, file].BackColor = Static.GetSquareColor(rank, file);
                    chessboardPictureBoxes[rank, file].BorderStyle = BorderStyle.None;
                }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            panel1.Controls.Clear();
            if (radioButton.Checked)
            {
                Static.WHITE_PLAYER_UP = false;
                Game.GetInstance().gameState.playerChosenColor = Color.White;
                DisplayBoard(Static.WHITE_PLAYER_UP);
            }
            else
            {
                Static.WHITE_PLAYER_UP = true;
                Game.GetInstance().gameState.playerChosenColor = Color.Black;
                DisplayBoard(Static.WHITE_PLAYER_UP);
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton AI = sender as RadioButton;
            if (AI.Checked == true)
            {
                label1.Visible = true;
                trackBar1.Visible = true;
                Game.GetInstance().gameState.vsAI = true;
            }
            else
            {
                label1.Visible = false;
                trackBar1.Visible = false;
                Game.GetInstance().gameState.vsAI = false;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Controls.Clear();
            Game.GetInstance().gameState.gameStarted = true;
            if (Game.GetInstance().gameState.vsAI == true && Game.GetInstance().gameState.playerTurnColor != Game.GetInstance().gameState.playerChosenColor)
                Game.communicator.moveAI();
            panel2.Controls.Add(getResignButton());
        }

        private Button getResignButton()
        {
            Button resignButton = new Button();
            resignButton.Name = "btnResign";
            resignButton.Size = new Size(80, 30);
            resignButton.Text = "Resign";
            resignButton.Location = new Point(55, panel2.Height / 2);
            resignButton.Click += btnResign_Click;
            return resignButton;
        }

        private void btnResign_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }
        private void button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {

                int paletteIndex = Convert.ToInt32(clickedButton.Tag);

                Static.selectedPalette = paletteIndex;

                UpdateChessboardColors();
            }
        }

        private void UpdateChessboardColors()
        {
            for (int rank = 0; rank < Static.NUMBER_OF_RANKS; rank++)
                for (int file = 0; file < Static.NUMBER_OF_FILES; file++)
                    chessboardPictureBoxes[rank, file].BackColor = Static.GetSquareColor(rank, file);
        }

        private void InitializeButtons()
        {
            int buttonCount = Static.colorPalette.GetLength(0);

            for (int i = 0; i < buttonCount; i++)
            {
                Button paletteButton = new Button();
                paletteButton.Tag = i;
                paletteButton.Click += button_Click;
                paletteButton.BackColor = Static.colorPalette[i, 1];

                paletteButton.Location = new Point(panel2.Width / 2 - 50, 10 + i * 30);
                paletteButton.Size = new Size(100, 25);

                panel2.Controls.Add(paletteButton);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Static.selectedElo = trackBar1.Value;
        }
    }
}