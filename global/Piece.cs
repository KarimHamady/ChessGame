using ChessGame.GameNamespace;

namespace ChessGame.Global
{
    public class PiecePromoter
    {
        public static IPiece CreatePiece(PieceType pieceType, Color color)
        {
            string typeName = pieceType.ToString();

            // Assumes all piece types are in the same namespace as the PiecePromoter class
            string fullTypeName = $"{typeof(PiecePromoter).Namespace}.{typeName}";

            Type? pieceTypeClass = Type.GetType(fullTypeName);

            if (pieceTypeClass != null && typeof(IPiece).IsAssignableFrom(pieceTypeClass))
            {
                return Activator.CreateInstance(pieceTypeClass, color) as IPiece;

            }

            // Handle the case when the type is not found or not derived from IPiece
            throw new InvalidOperationException($"Cannot create a piece of type {pieceType}");
        }
    }
    public interface IPiece
    {
        public PieceType PieceType { get; set; }
        public Color PieceColor {  get; set; }
        public abstract List<Location> GetAvailableMovesOnBoard(Location currentLocation);
    }

    internal interface IPawn : IPiece
    {
        public int RankDirection { get; }
        public abstract List<Location> GetPawnAttackLocations(Location currentLocation);
    }

    internal interface IKnight : IPiece { }

    internal interface IBishop : IPiece
    {
        protected List<Location> MovementDirection {  get; set; }
    }

    internal interface IRook : IPiece {
        public List<Location> MovementDirection { get; set; }
        public RookSide RookSide { get; set; }
    }

    internal interface IQueen : IPiece
    {
        public List<Location> MovementDirection { get; set; }
    }

    internal interface IKing : IPiece { }

    internal class Pawn : IPawn
    {
        public PieceType PieceType { get; set; }
        public Color PieceColor { get; set; }
        public int RankDirection { get { return PieceColor == Color.White ? 1 : -1; } }
        public Pawn(Color color)
        {
            PieceType = PieceType.Pawn;
            PieceColor = color;
        }
        public List<Location> GetAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new();

            Location forwardOne = currentLocation + new Location(RankDirection * 1, 0);
            IPiece? piece;

            if (Game.GetInstance().ChessBoard.IsMoveWithinBoard(forwardOne))
            {
                piece = Game.GetInstance().ChessBoard.Matrix[forwardOne.Rank, forwardOne.File];
                if (piece == null)
                    pieceMovements.Add(forwardOne); // Move forward
            }
            Location fowardTwo = currentLocation + new Location(RankDirection * 2, 0);
            if (Game.GetInstance().ChessBoard.IsMoveWithinBoard(fowardTwo))
            {
                piece = Game.GetInstance().ChessBoard.Matrix[fowardTwo.Rank, fowardTwo.File];
                if (piece == null && pieceMovements.Count > 0 && (RankDirection * currentLocation.Rank == 1 || currentLocation.Rank * RankDirection == -6))
                    pieceMovements.Add(fowardTwo); // Move forward (initial double move)
            }
            // Pawn can only capture diagonal if there is an opponent piece
            Location diagonalLeft = currentLocation + new Location(RankDirection * 1, -1);
            if (Game.GetInstance().ChessBoard.IsMoveWithinBoard(diagonalLeft))
            {
                piece = Game.GetInstance().ChessBoard.Matrix[diagonalLeft.Rank, diagonalLeft.File];
                if (piece != null && piece.PieceColor != Game.GetInstance().GameState.PlayerTurnColor)
                    pieceMovements.Add(diagonalLeft); // Capture diagonally left
            }

            Location diagonalRight = currentLocation + new Location(RankDirection * 1, 1);
            if (Game.GetInstance().ChessBoard.IsMoveWithinBoard(diagonalRight))
            {
                piece = Game.GetInstance().ChessBoard.Matrix[diagonalRight.Rank, diagonalRight.File];
                if (piece != null && piece.PieceColor != Game.GetInstance().GameState.PlayerTurnColor)
                    pieceMovements.Add(diagonalRight); // Capture diagonally right
            }
            return pieceMovements;
        }
        public List<Location> GetPawnAttackLocations(Location currentLocation)
        {
            int RankDirection = this.PieceColor == Color.White ? 1 : -1;
            Location diagonalLeft = currentLocation + new Location(RankDirection * 1, -1);
            Location diagonalRight = currentLocation + new Location(RankDirection * 1, 1);

            return new List<Location> { diagonalLeft, diagonalRight };
        }
    }

    internal class Knight : IKnight
    {
        public PieceType PieceType { get; set; }
        public Color PieceColor { get; set; }
        public Knight(Color color)
        {
            PieceType = PieceType.Knight;
            PieceColor = color;

        }
        public List<Location> GetAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new()
            {
                currentLocation + new Location(-2, -1),
                currentLocation + new Location(-2, 1),
                currentLocation + new Location(2, -1),
                currentLocation + new Location(2, 1),
                currentLocation + new Location(-1, -2),
                currentLocation + new Location(-1, 2),
                currentLocation + new Location(1, -2),
                currentLocation + new Location(1, 2)
            };

            return pieceMovements;
        }
    }

    internal class Bishop : IBishop
    {
        public PieceType PieceType { get; set; }
        public Color PieceColor { get; set; }

        public List<Location> MovementDirection { get; set; }
        public Bishop(Color color)
        {
            PieceType = PieceType.Bishop;
            PieceColor = color;
            MovementDirection = new() { new Location(-1, -1), new Location(-1, 1), new Location(1, -1), new Location(1, 1) };
        }

        public List<Location> GetAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new();

            foreach (Location movement in MovementDirection)
                pieceMovements.AddRange(Game.GetInstance().ChessBoard.GetAvailableMovesInDirection(currentLocation, movement.Rank, movement.File, Game.GetInstance().GetPlayerColor()));

            return pieceMovements;
        }
    }

    internal class Rook : IRook
    {
        public PieceType PieceType { get; set; }
        public Color PieceColor { get; set; }
        public List<Location> MovementDirection { get; set; }
        public RookSide RookSide { get; set; }

        public Rook(Color color)
        {
            PieceType = PieceType.Rook;
            PieceColor = color;
            RookSide = RookSide.KingSide;
            MovementDirection = new() { new Location(0, -1), new Location(0, 1), new Location(-1, 0), new Location(1, 0) };
        }
        public Rook(Color color, RookSide side)
        {
            PieceType = PieceType.Rook;
            PieceColor = color;
            MovementDirection = new() { new Location(0, -1), new Location(0, 1), new Location(-1, 0), new Location(1, 0) };
            RookSide = side;
        }

        public List<Location> GetAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new();

            foreach (Location movement in MovementDirection)
                pieceMovements.AddRange(Game.GetInstance().ChessBoard.GetAvailableMovesInDirection(currentLocation, movement.Rank, movement.File, Game.GetInstance().GetPlayerColor()));

            return pieceMovements;
        }

    }

    internal class Queen : IQueen
    {
        public PieceType PieceType { get; set; }
        public Color PieceColor { get; set; }

        public List<Location> MovementDirection { get; set; }
        public Queen(Color color)
        {
            PieceType = PieceType.Queen;
            PieceColor = color;
            MovementDirection = new() { new Location(-1, -1), new Location(-1, 1), new Location(1, -1), new Location(1, 1), new Location(0, -1), new Location(0, 1), new Location(-1, 0), new Location(1, 0) };
        }
        public List<Location> GetAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new();

            pieceMovements.AddRange(Game.GetInstance().ChessBoard.GetAvailableMovesInDirection(currentLocation, -1, -1, Game.GetInstance().GetPlayerColor())); // Top-left
            pieceMovements.AddRange(Game.GetInstance().ChessBoard.GetAvailableMovesInDirection(currentLocation, -1, 1, Game.GetInstance().GetPlayerColor()));  // Top-right
            pieceMovements.AddRange(Game.GetInstance().ChessBoard.GetAvailableMovesInDirection(currentLocation, 1, -1, Game.GetInstance().GetPlayerColor()));  // Bottom-left
            pieceMovements.AddRange(Game.GetInstance().ChessBoard.GetAvailableMovesInDirection(currentLocation, 1, 1, Game.GetInstance().GetPlayerColor()));   // Bottom-right
            pieceMovements.AddRange(Game.GetInstance().ChessBoard.GetAvailableMovesInDirection(currentLocation, 0, -1, Game.GetInstance().GetPlayerColor())); // Left
            pieceMovements.AddRange(Game.GetInstance().ChessBoard.GetAvailableMovesInDirection(currentLocation, 0, 1, Game.GetInstance().GetPlayerColor()));  // Right
            pieceMovements.AddRange(Game.GetInstance().ChessBoard.GetAvailableMovesInDirection(currentLocation, -1, 0, Game.GetInstance().GetPlayerColor())); // Up
            pieceMovements.AddRange(Game.GetInstance().ChessBoard.GetAvailableMovesInDirection(currentLocation, 1, 0, Game.GetInstance().GetPlayerColor()));  // Down

            return pieceMovements;
        }
    }

    internal class King : IKing
    {
        public PieceType PieceType { get; set; }
        public Color PieceColor { get; set; }

        public King(Color color)
        {
            PieceType = PieceType.King;
            PieceColor = color;

        }
        public List<Location> GetAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new()
            {
                currentLocation + new Location(-1, -1),
                currentLocation + new Location(-1, 0),
                currentLocation + new Location(-1, 1),
                currentLocation + new Location(0, -1),
                currentLocation + new Location(0, 1),
                currentLocation + new Location(1, -1),
                currentLocation + new Location(1, 0),
                currentLocation + new Location(1, 1)
            };

            if (!Game.GetInstance().GameState.Check)
            {
                // FIXME Check if a piece is blocking the Castling (+1, -1) before allowing (+2, -2) using attackedLocations list
                if (Game.GetInstance().GameState.PlayerTurnColor == Color.White)
                {
                    if (Game.GetInstance().Castling.WhiteCastlingAllowedKingSide == true)
                        pieceMovements.Add(currentLocation + new Location(0, -2));
                    if (Game.GetInstance().Castling.WhiteCastlingAllowedQueenSide == true)
                        pieceMovements.Add(currentLocation + new Location(0, 2));
                }
                else if (Game.GetInstance().GameState.PlayerTurnColor == Color.Black)
                {
                    if (Game.GetInstance().Castling.BlackCastlingAllowedKingSide == true)
                        pieceMovements.Add(currentLocation + new Location(0, -2));
                    if (Game.GetInstance().Castling.BlackCastlingAllowedQueenSide == true)
                        pieceMovements.Add(currentLocation + new Location(0, 2));
                }
            }

            if (Game.GetInstance().GameState.PlayerTurnColor == this.PieceColor)
                pieceMovements.RemoveAll(location => Game.GetInstance().GetAllAttackedLocations().Contains(location));
            return pieceMovements;
        }
    }
}