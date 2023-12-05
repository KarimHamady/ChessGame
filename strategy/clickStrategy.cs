using ChessGame.FacadeNamespace;
using ChessGame.Global;
using ChessGame.logic;
using ChessGame.Logic.BoardNamespace;
using ChessGame.Logic.GameNamespace;
using ChessGame.Logic.PieceNamespace;

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
            Piece clickedPiece = Board.GetPieceAtLocation(clickLocation);
            List<Location> possibleMovements = clickedPiece.GetAvailableMovesOnBoard(clickLocation);
            if (Game.check)
            {
                Facade.LimitPiecesMovements(clickedPiece, possibleMovements);
                // TODO: improve
                /*if (possibleMovements.Count == 0)
                {
                    GUI.AddCheckLabel();
                    return;
                }*/
            }

            GUI.ResetSquareColors();
            GUI.ColorLocations(possibleMovements, Color.Gold);
            Game.clickedLocation = clickLocation;
            Game.possibleMovements = possibleMovements;
        }
    }

    internal class ClickAfterMoveStrategy : ClickStrategy
    {
        public override void processClick(Location clickLocation)
        {
            if (Game.possibleMovements.Contains(clickLocation))
            {
                Facade.MovePieceFromSquareToSquare(Game.clickedLocation, clickLocation);

                Castle.UpdateCastlingCondition(Game.playerTurnColor, Board.GetBoard().matrix[Game.clickedLocation.Rank, Game.clickedLocation.File]);
                if (Board.GetBoard().matrix[clickLocation.Rank, clickLocation.File] is King)
                    Facade.CheckAndHandleCastling(Game.clickedLocation, clickLocation);

                GUI.ResetSquareColors();
                Game.ResetGameCheckVariables();
                foreach (Location location in Board.GetPieceAtLocation(clickLocation).GetAvailableMovesOnBoard(clickLocation))
                {
                    Piece? piece = Board.GetBoard().matrix[location.Rank, location.File];
                    if (piece != null && piece is King && piece.pieceColor != Game.playerTurnColor)
                    {
                        Facade.chessboardPictureBoxes[location.Rank, location.File].BackColor = Color.Red;
                        Game.checkingLocation = clickLocation;
                        Game.check = true;
                    }
                }
                Game.playerTurnColor = Game.playerTurnColor == Color.White ? Color.Black : Color.White;
            }
        }
    }
}
