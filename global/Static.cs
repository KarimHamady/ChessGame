namespace ChessGame.Global
{
    public static class Static
    {
        public const int NUMBER_OF_RANKS = 8;
        public const int NUMBER_OF_FILES = 8;
        public const bool WHITE_PLAYER_UP = false;
        public static int SQUARE_SIZE { get { return Screen.PrimaryScreen.WorkingArea.Height / 10; } }
        public static Color GetSquareColor(int rank, int file)
        {
            return (rank + file) % 2 == 0 ? Color.FromArgb(
                alpha: 255,
                red: 200,
                green: 230,
                blue: 201
                ) : Color.FromArgb(
                    alpha: 255,
                    red: 85,
                    green: 139,
                    blue: 110
                    );
            /*Color.FromArgb(
            alpha: 255,
            red: 240,
            green: 217,
            blue: 181
            ) : Color.FromArgb(
                alpha: 255,
                red: 181,
                green: 136,
                blue: 99
                );*/
        }
        public static Image? LoadPieceImage(IPiece? piece)
        {
            if (piece == null)
                return null;

            char colorPrefix = piece.PieceColor == Color.White ? 'w' : 'b';
            string? name = Enum.GetName(typeof(PieceType), piece.PieceType);
            string imagePath = $"../../../data/{(name != null ? name.ToLower() : "")}_{colorPrefix}.png";

            return File.Exists(imagePath) ? Image.FromFile(imagePath) : null;
        }
    }
}
