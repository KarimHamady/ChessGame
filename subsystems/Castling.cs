using ChessGame.Global;

namespace ChessGame.Subsystems
{
    internal class Castling
    {
        public bool WhiteCastlingAllowedKingSide {  get; set; }
        public bool BlackCastlingAllowedKingSide { get; set; }
        public bool WhiteCastlingAllowedQueenSide { get; set; }
        public bool BlackCastlingAllowedQueenSide { get; set; }

        public Castling() {
            WhiteCastlingAllowedKingSide = true;
            BlackCastlingAllowedKingSide = true;
            WhiteCastlingAllowedQueenSide = true;
            BlackCastlingAllowedQueenSide = true;
        }

        public void UpdateCastlingCondition(Color playerColor, IPiece currentPiece)
        {
            if (currentPiece is King)
            {
                King piece = (King)currentPiece;
                if (playerColor == Color.White)
                {
                    WhiteCastlingAllowedKingSide = false;
                    WhiteCastlingAllowedQueenSide = false;
                }
                else
                {
                    BlackCastlingAllowedKingSide = false;
                    BlackCastlingAllowedQueenSide = false;
                }

            }
            else if (currentPiece is Rook)
            {
                Rook piece = (Rook)currentPiece;
                if (playerColor == Color.White)
                {
                    if (piece.RookSide == RookSide.KingSide)
                        WhiteCastlingAllowedKingSide = false;
                    else if (piece.RookSide == RookSide.QueenSide)
                        WhiteCastlingAllowedQueenSide = false;
                }
                else
                {
                    if (piece.RookSide == RookSide.KingSide)
                        BlackCastlingAllowedKingSide = false;
                    else if (piece.RookSide == RookSide.QueenSide)
                        BlackCastlingAllowedQueenSide = false;
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
