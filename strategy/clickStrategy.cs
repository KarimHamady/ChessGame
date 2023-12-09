using ChessGame.GameNamespace;
using ChessGame.Global;

namespace ChessGame.Strategy
{
    internal abstract class ClickStrategy
    {
        public abstract void processClick(Location clickLocation);
    }

    internal class ShowMovesStrategy : ClickStrategy
    {
        public override void processClick(Location clickLocation)
        {
            IPiece? clickedPiece = Game.GetInstance().chessBoard.GetPieceAt(clickLocation);
            List<Location> possibleMovements = clickedPiece.GetAvailableMovesOnBoard(clickLocation);
            Game.GetInstance().chessBoard.RemoveInvalidMoves(possibleMovements, clickedPiece.PieceColor);
            if (Game.GetInstance().gameState.check)
                Game.GetInstance().LimitPiecesMovements(clickedPiece, possibleMovements);

            Game.GetInstance().ShowAvailableMoves(possibleMovements);
            Game.GetInstance().gameState.clickedPieceLocation = clickLocation;
            Game.GetInstance().gameState.possibleMovements = possibleMovements;
        }
    }

    internal class MoveStrategy : ClickStrategy
    {
        public override void processClick(Location clickLocation)
        {
            if (Game.GetInstance().gameState.possibleMovements.Contains(clickLocation))
            {
                Game.GetInstance().MovePiece(Game.GetInstance().gameState.clickedPieceLocation, clickLocation);

                Game.GetInstance().castling.UpdateCastlingCondition(Game.GetInstance().gameState.playerTurnColor, Game.GetInstance().chessBoard.matrix[clickLocation.Rank, clickLocation.File]);
                if (Game.GetInstance().chessBoard.GetPieceAt(clickLocation) is King)
                    Game.GetInstance().CheckAndHandleCastling(Game.GetInstance().gameState.clickedPieceLocation, clickLocation);
                else if (Game.GetInstance().chessBoard.GetPieceAt(clickLocation) is Pawn)
                    Game.GetInstance().CheckAndHandlePawnPromotion(clickLocation);


                Game.GetInstance().gameState.ResetGameCheckVariables();

                CheckAndHandleKingCheck(clickLocation);
                AlternateColors();
                if (Game.GetInstance().gameState.vsAI == true && Game.GetInstance().gameState.playerTurnColor != Game.GetInstance().gameState.playerChosenColor)
                    Game.communicator.moveAI();
            }
        }

        private void CheckAndHandleKingCheck(Location clickLocation)
        {
            List<Location> pieceMovements = Game.GetInstance().chessBoard.GetPieceAt(clickLocation)!.GetAvailableMovesOnBoard(clickLocation);
            Game.GetInstance().chessBoard.RemoveInvalidMoves(pieceMovements, Game.GetInstance().chessBoard.GetPieceAt(clickLocation)!.PieceColor);
            foreach (Location location in pieceMovements)
            {
                IPiece? piece = Game.GetInstance().chessBoard.matrix[location.Rank, location.File];
                if (piece != null && piece is King && piece.PieceColor != Game.GetInstance().gameState.playerTurnColor)
                {
                    Game.chessboardPictureBoxes[location.Rank, location.File].BackColor = Color.Red;
                    Game.GetInstance().gameState.checkingLocation = clickLocation;
                    Game.GetInstance().gameState.check = true;
                    AlternateColors();
                    if (IsCheckmate())
                    {
                        Game.GetInstance().HandleCheckmate();
                    }
                    else
                    {
                        Game.GetInstance().HandleCheck();
                    }
                    AlternateColors();
                    break;
                }
            }
        }

        private void AlternateColors()
        {
            Game.GetInstance().gameState.playerTurnColor = Game.GetInstance().gameState.playerTurnColor == Color.White ? Color.Black : Color.White;
        }
        private bool IsCheckmate()
        {
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    IPiece? piece = Game.GetInstance().chessBoard.GetPieceAt(new Location(rank, file));
                    if (piece != null && piece.PieceColor == Game.GetInstance().gameState.playerTurnColor)
                    {
                        List<Location> movements = piece.GetAvailableMovesOnBoard(new Location(rank, file));
                        Game.GetInstance().LimitPiecesMovements(piece, movements);
                        Game.GetInstance().chessBoard.RemoveInvalidMoves(movements, piece.PieceColor);
                        if (movements.Count != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
