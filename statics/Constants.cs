namespace ChessGame.Statics
{
    public static class Constants
    {
        public const int NUMBER_OF_RANKS = 8;
        public const int NUMBER_OF_FILES = 8;
        public const int SQUARE_SIZE = 80;
    }

    public static class Static
    {
        public static Color GetSquareColor(int rank, int file)
        {
            return (rank + file) % 2 == 0 ? Color.White : Color.Black;
        }
    }
}
