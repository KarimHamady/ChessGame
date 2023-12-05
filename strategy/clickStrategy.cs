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
            if (Game.GetInstance().gameState.check)
            {
                Game.GetInstance().LimitPiecesMovements(clickedPiece, possibleMovements);
                // TODO: improve
                /*if (possibleMovements.Count == 0)
                {
                    GUI.AddCheckLabel();
                    return;
                }*/
            }

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
                Game.GetInstance().MovePieceFromSquareToSquare(Game.GetInstance().gameState.clickedLocation, clickLocation);

                Game.GetInstance().castling.UpdateCastlingCondition(Game.GetInstance().gameState.playerTurnColor, Game.GetInstance().chessBoard.matrix[Game.GetInstance().gameState.clickedLocation.Rank, Game.GetInstance().gameState.clickedLocation.File]);
                if (Game.GetInstance().chessBoard.matrix[clickLocation.Rank, clickLocation.File] is King)
                    Game.GetInstance().CheckAndHandleCastling(Game.GetInstance().gameState.clickedLocation, clickLocation);

                GUI.ResetSquareColors();
                Game.GetInstance().gameState.ResetGameCheckVariables();
                foreach (Location location in Game.GetInstance().chessBoard.GetPieceAt(clickLocation).GetAvailableMovesOnBoard(clickLocation))
                {
                    Piece? piece = Game.GetInstance().chessBoard.matrix[location.Rank, location.File];
                    if (piece != null && piece is King && piece.pieceColor != Game.GetInstance().gameState.playerTurnColor)
                    {
                        Game.chessboardPictureBoxes[location.Rank, location.File].BackColor = Color.Red;
                        Game.GetInstance().gameState.checkingLocation = clickLocation;
                        Game.GetInstance().gameState.check = true;
                    }
                }
                Game.GetInstance().gameState.playerTurnColor = Game.GetInstance().gameState.playerTurnColor == Color.White ? Color.Black : Color.White;
                Sound.PlayMoveSound();
            }
        }
    }
}
