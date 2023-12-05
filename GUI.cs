using ChessGame.GameNamespace;
using ChessGame.global;
using ChessGame.Global;
using ChessGame.Statics;

namespace ChessGame
{
    public class CustomMessageBox : Form
    {
        public PieceType SelectedPieceType { get; private set; }

        public CustomMessageBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AddPieceTypeButton(PieceType.Rook);
            AddPieceTypeButton(PieceType.Knight);
            AddPieceTypeButton(PieceType.Bishop);
            AddPieceTypeButton(PieceType.Queen);

            // Handle form events as needed
            this.Text = "Promote to";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false;
        }

        private void AddPieceTypeButton(PieceType pieceType)
        {
            var button = new Button
            {
                Text = Enum.GetName(typeof(Statics.PieceType), pieceType),
                Size = new System.Drawing.Size(75, 23),
                Location = new System.Drawing.Point(10, 10),  // Adjust button position
            };

            button.Click += (sender, e) =>
            {
                SelectedPieceType = pieceType;
                DialogResult = DialogResult.OK;
                Close();
            };

            Controls.Add(button);

            // Adjust vertical position for the next button
            int verticalSpacing = 35;
            foreach (Control control in Controls)
            {
                control.Location = new System.Drawing.Point(control.Location.X, control.Location.Y + verticalSpacing);
            }
        }
    }
    public partial class GUI : Form
    {
        private static Form? instance;
        static readonly PictureBox[,] chessboardPictureBoxes = Game.chessboardPictureBoxes;

        public GUI()
        {
            instance = this;
            InitializeComponent();
            DisplayBoard(Constants.WHITE_PLAYER_UP);
        }
        public static PieceType ShowCustomPieceTypeDialog()
        {
            using (var customMessageBox = new CustomMessageBox())
            {
                DialogResult result = customMessageBox.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // User clicked one of the piece type buttons
                    return customMessageBox.SelectedPieceType;

                }
                return PieceType.Queen;
                // You can optionally handle DialogResult.Cancel if needed
            }
        }

        public void DisplayBoard(bool isWhiteUp)
        {
            // Populate the chessboardPictureBoxes array
            for (int rank = 0; rank < Constants.NUMBER_OF_RANKS; rank++)
            {
                for (int file = 0; file < Constants.NUMBER_OF_FILES; file++)
                {
                    Location chessTileLocation = isWhiteUp ? new Location(rank, file) : new Location(7 - rank, 7 - file);
                    chessboardPictureBoxes[rank, file] = new ChessTile(
                            location: chessTileLocation,
                            color: Static.GetSquareColor(rank, file),
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
    }
}