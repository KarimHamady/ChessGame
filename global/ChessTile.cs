namespace ChessGame.Global
{
    internal class LegacyTile : PictureBox { }
    internal class ChessTile : LegacyTile
    {
        Action<Location> OnPressed { get; set; }
        public ChessTile(Location location, Color color, Image image, Action<Location> onPressed) : base()
        {
            Size = new Size(Static.SQUARE_SIZE, Static.SQUARE_SIZE);
            BorderStyle = BorderStyle.FixedSingle;
            SizeMode = PictureBoxSizeMode.Zoom;
            Location = AdaptLocation(location);
            BackColor = color;
            Image = image;
            Click += AdaptOnPressed;
            OnPressed = onPressed;
        }

        private static Point AdaptLocation(Location location)
        {
            return new Point(location.File * Static.SQUARE_SIZE, location.Rank * Static.SQUARE_SIZE);
        }

        private void AdaptOnPressed(object? sender, EventArgs e)
        {
            Location clickedLocation = new(-1, -1);

            if (sender is PictureBox clickedPictureBox)
            {
                clickedLocation.Rank = clickedPictureBox.Location.Y / Static.SQUARE_SIZE;
                clickedLocation.File = clickedPictureBox.Location.X / Static.SQUARE_SIZE;
            }

            if (clickedLocation.Rank != -1 && clickedLocation.File != -1)
            {
                OnPressed(clickedLocation);
            }
        }
    }
}
