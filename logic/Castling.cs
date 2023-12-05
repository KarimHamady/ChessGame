using ChessGame.Global;
using ChessGame.Logic.PieceNamespace;
using ChessGame.Statics;

namespace ChessGame.Logic
{
    namespace CastleNamespace
    {
        internal class Castling
        {
            public bool whiteCastlingAllowedKingSide;
            public bool blackCastlingAllowedKingSide;
            public bool whiteCastlingAllowedQueenSide;
            public bool blackCastlingAllowedQueenSide;

            public Castling() {
                whiteCastlingAllowedKingSide = true;
                blackCastlingAllowedKingSide = true;
                whiteCastlingAllowedQueenSide = true;
                blackCastlingAllowedQueenSide = true;
            }

            public void UpdateCastlingCondition(Color playerColor, Piece currentPiece)
            {
                if (currentPiece is King)
                {
                    King piece = (King)currentPiece;
                    if (playerColor == Color.White)
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
                    if (playerColor == Color.White)
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

            public  bool IsKingSideCastling(Location currentKingLocation, Location newKingLocation)
            {
                return currentKingLocation.File - newKingLocation.File == 2;
            }
            public bool IsQueenSideCastling(Location currentKingLocation, Location newKingLocation)
            {
                return newKingLocation.File - currentKingLocation.File == 2;
            }

            public Location GetRookLocationFromCastlingSide(Color playerColor, CastlingSide castlingSide)
            {
                if (playerColor == Color.White)
                {
                    if (castlingSide == CastlingSide.KingSide)
                        return new Location(0, 0);
                    else if (castlingSide == CastlingSide.QueenSide)
                        return new Location(0, 7);
                }
                else if (playerColor == Color.Black)
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
