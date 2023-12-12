using ChessGame.GameNamespace;
using ChessGame.Global;

namespace ChessGame.Strategy
{
    internal abstract class ClickStrategy
    {
        public abstract void ProcessClick(Location clickLocation);
    }

    internal class ShowMovesStrategy : ClickStrategy
    {
        public override void ProcessClick(Location clickLocation)
        {
            IPiece? clickedPiece = Game.GetInstance().ChessBoard.GetPieceAt(clickLocation);
            List<Location> possibleMovements = clickedPiece.GetAvailableMovesOnBoard(clickLocation);
            Game.GetInstance().ChessBoard.RemoveInvalidMoves(possibleMovements, clickedPiece.PieceColor);
            if (Game.GetInstance().GameState.Check)
                Game.GetInstance().LimitPiecesMovements(clickedPiece, possibleMovements);

            Game.GetInstance().ShowAvailableMoves(possibleMovements);
            Game.GetInstance().GameState.ClickedPieceLocation = clickLocation;
            Game.GetInstance().GameState.PossibleMovements = possibleMovements;
        }
    }

    internal class MoveStrategy : ClickStrategy
    {
        public override void ProcessClick(Location clickLocation)
        {
            if (Game.GetInstance().GameState.PossibleMovements.Contains(clickLocation))
            {
                Game.GetInstance().MovePiece(Game.GetInstance().GameState.ClickedPieceLocation, clickLocation);

                Game.GetInstance().Castling.UpdateCastlingCondition(Game.GetInstance().GameState.PlayerTurnColor, Game.GetInstance().ChessBoard.Matrix[clickLocation.Rank, clickLocation.File]);
                if (Game.GetInstance().ChessBoard.GetPieceAt(clickLocation) is King)
                    Game.GetInstance().CheckAndHandleCastling(Game.GetInstance().GameState.ClickedPieceLocation, clickLocation);
                else if (Game.GetInstance().ChessBoard.GetPieceAt(clickLocation) is Pawn)
                    Game.GetInstance().CheckAndHandlePawnPromotion(clickLocation);


                Game.GetInstance().GameState.ResetGameCheckVariables();

                CheckAndHandleKingCheck(clickLocation);
                AlternateColors();
                if (Game.GetInstance().GameState.VsAI == true && Game.GetInstance().GameState.PlayerTurnColor != Game.GetInstance().GameState.PlayerChosenColor)
                    Game.Communicator.moveAI();
            }
        }

        private void CheckAndHandleKingCheck(Location clickLocation)
        {
            List<Location> pieceMovements = Game.GetInstance().ChessBoard.GetPieceAt(clickLocation)!.GetAvailableMovesOnBoard(clickLocation);
            Game.GetInstance().ChessBoard.RemoveInvalidMoves(pieceMovements, Game.GetInstance().ChessBoard.GetPieceAt(clickLocation)!.PieceColor);
            foreach (Location location in pieceMovements)
            {
                IPiece? piece = Game.GetInstance().ChessBoard.Matrix[location.Rank, location.File];
                if (piece != null && piece is King && piece.PieceColor != Game.GetInstance().GameState.PlayerTurnColor)
                {
                    Game.ChessboardPictureBoxes[location.Rank, location.File].BackColor = Color.Red;
                    Game.GetInstance().GameState.CheckingLocation = clickLocation;
                    Game.GetInstance().GameState.Check = true;
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
            Game.GetInstance().GameState.PlayerTurnColor = Game.GetInstance().GameState.PlayerTurnColor == Color.White ? Color.Black : Color.White;
        }
        private bool IsCheckmate()
        {
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    IPiece? piece = Game.GetInstance().ChessBoard.GetPieceAt(new Location(rank, file));
                    if (piece != null && piece.PieceColor == Game.GetInstance().GameState.PlayerTurnColor)
                    {
                        List<Location> movements = piece.GetAvailableMovesOnBoard(new Location(rank, file));
                        Game.GetInstance().LimitPiecesMovements(piece, movements);
                        Game.GetInstance().ChessBoard.RemoveInvalidMoves(movements, piece.PieceColor);
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
