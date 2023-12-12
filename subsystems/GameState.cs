using ChessGame.Global;
using System.Text;

namespace ChessGame.Subsystems
{
    internal class GameState
    {

        public Location ClickedPieceLocation {  get; set; }
        public List<Location> PossibleMovements { get; set; }
        public Color PlayerTurnColor { get; set; }
        public Color PlayerChosenColor { get; set; }
        public bool VsAI { get; set; }
        public bool GameStarted { get; set; }

        public bool Check { get; set; }
        public Location CheckingLocation { get; set; }
        public Location WhiteKingLocation { get; set; }
        public Location BlackKingLocation { get; set; }
        public static StringBuilder? Moves { get; set; }
        public GameState()
        {
            Moves = new StringBuilder();
            ClickedPieceLocation = new(-1, -1);
            PossibleMovements = new();
            PlayerTurnColor = Color.White;
            PlayerChosenColor = Color.White;
            VsAI = false;
            GameStarted = false;
            Check = false;
            CheckingLocation = new(-1, -1);
            WhiteKingLocation = new(0, 3);
            BlackKingLocation = new(7, 3);
        }

        public void ResetGameCheckVariables()
        {
            ClickedPieceLocation = new Location(-1, -1);
            CheckingLocation = new Location(-1, -1);
            Check = false;
        }

    }
}
