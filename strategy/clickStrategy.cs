using ChessGame.GameNamespace;
using ChessGame.Global;
using ChessGame.Logic.PieceNamespace;
using ChessGame.Statics;

namespace ChessGame.strategy
{
    internal abstract class ClickStrategy
    {
        public abstract void processClick(Location clickLocation);
    }

    internal class ClickBeforeMoveStrategy : ClickStrategy
    {
        public override void processClick(Location clickLocation)
        {
            Piece clickedPiece = Game.GetInstance().chessBoard.GetPieceAt(clickLocation);
            List<Location> possibleMovements = clickedPiece.GetAvailableMovesOnBoard(clickLocation);
            Game.GetInstance().chessBoard.RemoveInvalidMoves(possibleMovements, clickedPiece.pieceColor);
            if (Game.GetInstance().gameState.check)
                Game.GetInstance().LimitPiecesMovements(clickedPiece, possibleMovements);

            GUI.ResetSquareColors();
            GUI.ColorLocations(possibleMovements, Color.Gold);
            Game.GetInstance().gameState.clickedLocation = clickLocation;
            Game.GetInstance().gameState.possibleMovements = possibleMovements;
        }
    }

    internal class ClickAfterMoveStrategy : ClickStrategy
    {
        public override void processClick(Location clickLocation)
        {
            if (Game.GetInstance().gameState.possibleMovements.Contains(clickLocation))
            {

                Sound.PlayMoveSound();

                Game.GetInstance().MovePieceFromSquareToSquare(Game.GetInstance().gameState.clickedLocation, clickLocation);

                Game.GetInstance().castling.UpdateCastlingCondition(Game.GetInstance().gameState.playerTurnColor, Game.GetInstance().chessBoard.matrix[clickLocation.Rank, clickLocation.File]);
                if (Game.GetInstance().chessBoard.GetPieceAt(clickLocation) is King)
                    Game.GetInstance().CheckAndHandleCastling(Game.GetInstance().gameState.clickedLocation, clickLocation);
                else if (Game.GetInstance().chessBoard.GetPieceAt(clickLocation) is Pawn)
                    Game.GetInstance().CheckAndHandlePawnPromotion(clickLocation);


                GUI.ResetSquareColors();
                Game.GetInstance().gameState.ResetGameCheckVariables();

                CheckAndHandleKingCheck(clickLocation);
                AlternateColors();
            }
        }

        public void CheckAndHandleKingCheck(Location clickLocation)
        {
            List<Location> pieceMovements = Game.GetInstance().chessBoard.GetPieceAt(clickLocation).GetAvailableMovesOnBoard(clickLocation);
            Game.GetInstance().chessBoard.RemoveInvalidMoves(pieceMovements, Game.GetInstance().chessBoard.GetPieceAt(clickLocation).pieceColor);
            foreach (Location location in pieceMovements)
            {
                Piece? piece = Game.GetInstance().chessBoard.matrix[location.Rank, location.File];
                if (piece != null && piece is King && piece.pieceColor != Game.GetInstance().gameState.playerTurnColor)
                {
                    Game.chessboardPictureBoxes[location.Rank, location.File].BackColor = Color.Red;
                    Game.GetInstance().gameState.checkingLocation = clickLocation;
                    Game.GetInstance().gameState.check = true;
                    AlternateColors();
                    if (isCheckmate())
                    {
                        Sound.PlayCheckmateSound();
                        DialogResult result = new CheckmateWindow().ShowDialog();
                    } else
                    {
                        Sound.PlayCheckSound();
                    }
                    AlternateColors();
                    break;
                }
            }
        }

        public void AlternateColors()
        {
            Game.GetInstance().gameState.playerTurnColor = Game.GetInstance().gameState.playerTurnColor == Color.White ? Color.Black : Color.White;
        }
        public bool isCheckmate()
        {
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    Piece piece = Game.GetInstance().chessBoard.GetPieceAt(new Location(rank, file));
                    if (piece != null && piece.pieceColor == Game.GetInstance().gameState.playerTurnColor)
                    {
                        List<Location> movements = piece.GetAvailableMovesOnBoard(new Location(rank, file));
                        Game.GetInstance().LimitPiecesMovements(piece, movements);
                        Game.GetInstance().chessBoard.RemoveInvalidMoves(movements, piece.pieceColor);
                        if (movements.Count != 0)
                        {
                            //Game.GetInstance().gameState.playerTurnColor = Game.GetInstance().gameState.playerTurnColor == Color.White ? Color.Black : Color.White;
                            return false;
                        }
                    }
                }
            }
            //Game.GetInstance().gameState.playerTurnColor = Game.GetInstance().gameState.playerTurnColor == Color.White ? Color.Black : Color.White;
            return true;
        }
    }
}
