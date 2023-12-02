namespace ChessGame.logic
{
    internal abstract class Player
    {
        public Color color;
        // public List<Piece> pieces = new ;
    }
    internal class WhitePlayer : Player
    {
        public static WhitePlayer whitePlayer = null;
        static Color color = Color.White;
        private WhitePlayer()
        {
        }
        /*public Player opponent()
        {
            return this == WhitePlayer ? blackPlayer : whitePlayer;
        }*/
    }
}
