using ChessGame.Global;

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
                green: 200,
                blue: 200
                ) : Color.FromArgb(
                    alpha: 255,
                    red: 100,
                    green: 100,
                    blue: 100
                    );
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
