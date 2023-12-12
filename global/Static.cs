namespace ChessGame.Global
{
    public static class Static
    {
        public const int NUMBER_OF_RANKS = 8;
        public const int NUMBER_OF_FILES = 8;
        public static bool WHITE_PLAYER_UP = false;
        public static int selectedPalette = 0;
        public static int selectedElo = 1320;
        public static Color[,] colorPalette =
        {
            { Color.FromArgb(255, 240, 217, 181), Color.FromArgb(255, 181, 136, 99) }, // wooden
            { Color.FromArgb(255, 238, 238, 210), Color.FromArgb(255, 118, 150, 86) }, // chess.com
            { Color.FromArgb(255, 240, 240, 240), Color.FromArgb(255, 100, 100, 100)}, // blue gray
            { Color.FromArgb(255, 230, 184, 183), Color.FromArgb(255, 162, 70, 55) },  // Coral
            { Color.FromArgb(255, 191, 219, 255), Color.FromArgb(255, 49, 112, 143) },  // Sky Blue
            { Color.FromArgb(255, 255, 232, 186), Color.FromArgb(255, 230, 168, 56) }   // Gold
        };


        public static int SQUARE_SIZE { get { return Screen.PrimaryScreen.WorkingArea.Height / 10; } }
        public static Color GetSquareColor(int rank, int file)
        {
            return (rank + file) % 2 == 0 ? colorPalette[selectedPalette, 0] : colorPalette[selectedPalette, 1];
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
