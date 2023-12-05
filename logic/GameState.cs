using ChessGame.Global;

namespace ChessGame.Logic
{
    namespace GameStateNamespace
    {
        internal class GameState
        {

            public Location clickedLocation;
            public List<Location> possibleMovements;
            public Color playerTurnColor;

            public bool check;
            public Location checkingLocation;
            public Location whiteKingLocation;
            public Location blackKingLocation;

            public GameState()
            {
                clickedLocation = new(-1, -1);
                possibleMovements = new();
                playerTurnColor = Color.White;
                check = false;
                checkingLocation = new(-1, -1);
                whiteKingLocation = new(0, 3);
                blackKingLocation = new(7, 3);
            }

            public void ResetGameCheckVariables()
            {
                clickedLocation = new Location(-1, -1);
                checkingLocation = new Location(-1, -1);
                check = false;
            }

        }
    }
}
