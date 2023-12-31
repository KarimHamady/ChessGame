﻿using ChessGame.Global;
using System.Text;

namespace ChessGame.Subsystems
{
    internal class GameState
    {

        public Location clickedPieceLocation;
        public List<Location> possibleMovements;
        public Color playerTurnColor;
        public Color playerChosenColor;
        public bool vsAI;
        public bool gameStarted;

        public bool check;
        public Location checkingLocation;
        public Location whiteKingLocation;
        public Location blackKingLocation;
        public static StringBuilder Moves = new StringBuilder();
        public GameState()
        {
            clickedPieceLocation = new(-1, -1);
            possibleMovements = new();
            playerTurnColor = Color.White;
            playerChosenColor = Color.White;
            vsAI = false;
            gameStarted = false;
            check = false;
            checkingLocation = new(-1, -1);
            whiteKingLocation = new(0, 3);
            blackKingLocation = new(7, 3);
        }

        public void ResetGameCheckVariables()
        {
            clickedPieceLocation = new Location(-1, -1);
            checkingLocation = new Location(-1, -1);
            check = false;
        }

    }
}
