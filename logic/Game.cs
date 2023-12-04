using ChessGame.Global;

namespace ChessGame.Logic
{
    namespace GameNamespace
    {
        internal class Game
        {

            public static Location clickedLocation = new(-1, -1);
            public static List<Location> possibleMovements = new();
            public static Color playerTurnColor = Color.White;

            public static bool check = false;
            public static Location checkingLocation = new(-1, -1);
            public static Location whiteKingLocation = new(0, 3);
            public static Location blackKingLocation = new(7, 3);

            public static void ResetGameCheckVariables()
            {
                clickedLocation = new Location(-1, -1);
                checkingLocation = new Location(-1, -1);
                check = false;
            }

        }
    }
}
