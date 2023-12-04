using ChessGame.Global;
using ChessGame.Logic.PieceNamespace;
using ChessGame.Statics;

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


            public static bool whiteCastlingAllowedKingSide = true;
            public static bool blackCastlingAllowedKingSide = true;
            public static bool whiteCastlingAllowedQueenSide = true;
            public static bool blackCastlingAllowedQueenSide = true;

            public static bool IsKingSideCastling(Location currentKingLocation, Location newKingLocation)
            {
                return currentKingLocation.File - newKingLocation.File == 2;
            }
            public static bool IsQueenSideCastling(Location currentKingLocation, Location newKingLocation)
            {
                return newKingLocation.File - currentKingLocation.File == 2;
            }

            public static void UpdateCastlingCondition(Piece currentPiece)
            {
                if (currentPiece is King)
                {
                    King piece = (King)currentPiece;
                    if (playerTurnColor == Color.White)
                    {
                        whiteCastlingAllowedKingSide = false;
                        whiteCastlingAllowedQueenSide = false;
                    }
                    else
                    {
                        blackCastlingAllowedKingSide = false;
                        blackCastlingAllowedQueenSide = false;
                    }

                }
                else if (currentPiece is Rook)
                {
                    Rook piece = (Rook)currentPiece;
                    if (playerTurnColor == Color.White)
                    {
                        if (piece._rookSide == RookSide.KingSide)
                            whiteCastlingAllowedKingSide = false;
                        else if (piece._rookSide == RookSide.QueenSide)
                            whiteCastlingAllowedQueenSide = false;
                    }
                    else
                    {
                        if (piece._rookSide == RookSide.KingSide)
                            blackCastlingAllowedKingSide = false;
                        else if (piece._rookSide == RookSide.QueenSide)
                            blackCastlingAllowedQueenSide = false;
                    }
                }
            }
            public static void ResetGameCheckVariables()
            {
                clickedLocation = new Location(-1, -1);
                checkingLocation = new Location(-1, -1);
                check = false;
            }

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
}
