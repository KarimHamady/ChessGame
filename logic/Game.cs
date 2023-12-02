namespace ChessGame.logic
{
    internal class Game
    {
        public static Color playerTurnColor = Color.White;
        public static void alternateTurns()
        {
            playerTurnColor = playerTurnColor == Color.White ? Color.Black : Color.White;
        }

        public void play()
        {
            while (gameValid())
            {
                // PictureBox_Click, works only for the correct player turn
                // isMoveValid();
                // movePiece();
                alternateTurns();
            }
        }
        public bool gameValid()
        {
            return !IsCheckMate() && !IsDraw();
        }

        public bool IsCheckMate()
        {
            return false;
        }
        public bool IsDraw()
        {
            return false;
        }

    }
}
