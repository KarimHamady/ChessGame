using ChessGame.Global;
using ChessGame.Statics;

namespace ChessGame.global
{
    internal class ChessTile : PictureBox
    {
        Action<Location> OnPressed { get; set; }
        public ChessTile(Location location, Color color, Image image, Action<Location> onPressed) : base() {
            Size = new Size(Constants.SQUARE_SIZE, Constants.SQUARE_SIZE);
            BorderStyle = BorderStyle.FixedSingle;
            SizeMode = PictureBoxSizeMode.Zoom;
            Location = AdaptLocation(location);
            BackColor = color;
            Image = image;
            Click += AdaptOnPressed;
            OnPressed = onPressed;
            }

        private static Point AdaptLocation(Location location) {
            return new Point(location.File * Constants.SQUARE_SIZE, location.Rank * Constants.SQUARE_SIZE);
        }

        private void AdaptOnPressed(object? sender, EventArgs e)
        {
            Location clickedLocation = new(-1, -1);

            if (sender is PictureBox clickedPictureBox)
            {
                clickedLocation.Rank = clickedPictureBox.Location.Y / Constants.SQUARE_SIZE;
                clickedLocation.File = clickedPictureBox.Location.X / Constants.SQUARE_SIZE;
            }

            if (clickedLocation.Rank != -1 && clickedLocation.File != -1)
            {
                OnPressed(clickedLocation);
            }
        }
    }
}
