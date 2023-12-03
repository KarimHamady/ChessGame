using ChessGame.logic;

namespace ChessGame.statics
{
    public static class InitialPieceLocations
    {
        public static Location GetRookLocationFromCastlingSide(CastlingSide castlingSide)
        {
            if (Game.playerTurnColor == Color.White)
            {
                if (castlingSide == CastlingSide.KingSide)
                    return new Location(0, 0);
                else if (castlingSide == CastlingSide.QueenSide)
                    return new Location(0, 7);
            }
            else if (Game.playerTurnColor == Color.Black)
            {
                if (castlingSide == CastlingSide.KingSide)
                    return new Location(7, 0);
                else if (castlingSide == CastlingSide.QueenSide)
                    return new Location(7, 7);
            }
            return new Location(-1, -1);
        }
    }
}
