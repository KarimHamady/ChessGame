﻿namespace ChessGame.logic
{
    enum PieceType
    {
        None,
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    internal abstract class Piece
    {
        public PieceType pieceType = PieceType.None;
        public Color pieceColor;
        protected bool capturesLikeItsMove = false;
        public abstract List<Location> getAvailableMovesOnBoard(Location currentLocation);
    }

    internal class Pawn : Piece
    {
        public Pawn(Color color)
        {
            pieceType = PieceType.Pawn;
            pieceColor = color;
            capturesLikeItsMove = false;
        }
        public override List<Location> getAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            int rankDirection = this.pieceColor == Color.White ? 1 : -1;
            pieceMovements.Add(currentLocation + new Location(rankDirection * 1, 0)); // Move forward
            pieceMovements.Add(currentLocation + new Location(rankDirection * 2, 0)); // Move forward (initial double move)
            pieceMovements.Add(currentLocation + new Location(rankDirection * 1, -1)); // Capture diagonally left
            pieceMovements.Add(currentLocation + new Location(rankDirection * 1, 1)); // Capture diagonally right

            Checker.removeInvalidMoves(pieceMovements);
            return pieceMovements;
        }
    }

    internal class Knight : Piece
    {
        public Knight(Color color)
        {
            pieceType = PieceType.Knight;
            pieceColor = color;
            capturesLikeItsMove = true;
        }
        public override List<Location> getAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            pieceMovements.Add(currentLocation + new Location(-2, -1));
            pieceMovements.Add(currentLocation + new Location(-2, 1));
            pieceMovements.Add(currentLocation + new Location(2, -1));
            pieceMovements.Add(currentLocation + new Location(2, 1));
            pieceMovements.Add(currentLocation + new Location(-1, -2));
            pieceMovements.Add(currentLocation + new Location(-1, 2));
            pieceMovements.Add(currentLocation + new Location(1, -2));
            pieceMovements.Add(currentLocation + new Location(1, 2));

            Checker.removeInvalidMoves(pieceMovements);
            return pieceMovements;
        }
    }

    internal class Bishop : Piece
    {
        public Bishop(Color color)
        {
            pieceType = PieceType.Bishop;
            pieceColor = color;
            capturesLikeItsMove = true;
        }

        public override List<Location> getAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, -1, -1)); // Top-left
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, -1, 1));  // Top-right
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 1, -1));  // Bottom-left
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 1, 1));   // Bottom-right

            Checker.removeInvalidMoves(pieceMovements);
            return pieceMovements;
        }
    }

    internal class Rook : Piece
    {
        public Rook(Color color)
        {
            pieceType = PieceType.Rook;
            pieceColor = color;
            capturesLikeItsMove = true;
        }
        public override List<Location> getAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 0, -1)); // Left
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 0, 1));  // Right
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, -1, 0)); // Up
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 1, 0));  // Down

            Checker.removeInvalidMoves(pieceMovements);
            return pieceMovements;
        }
    }

    internal class Queen : Piece
    {
        public Queen(Color color)
        {
            pieceType = PieceType.Queen;
            pieceColor = color;
            capturesLikeItsMove = true;
        }
        public override List<Location> getAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, -1, -1)); // Top-left
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, -1, 1));  // Top-right
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 1, -1));  // Bottom-left
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 1, 1));   // Bottom-right
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 0, -1)); // Left
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 0, 1));  // Right
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, -1, 0)); // Up
            pieceMovements.AddRange(Checker.GetAvailableMovesInDirection(currentLocation, 1, 0));  // Down

            Checker.removeInvalidMoves(pieceMovements);
            return pieceMovements;
        }
    }

    internal class King : Piece
    {
        public King(Color color)
        {
            pieceType = PieceType.King;
            pieceColor = color;
            capturesLikeItsMove = true;
        }
        public override List<Location> getAvailableMovesOnBoard(Location currentLocation)
        {
            List<Location> pieceMovements = new List<Location>();

            pieceMovements.Add(currentLocation + new Location(-1, -1));
            pieceMovements.Add(currentLocation + new Location(-1, 0));
            pieceMovements.Add(currentLocation + new Location(-1, 1));
            pieceMovements.Add(currentLocation + new Location(0, -1));
            pieceMovements.Add(currentLocation + new Location(0, 1));
            pieceMovements.Add(currentLocation + new Location(1, -1));
            pieceMovements.Add(currentLocation + new Location(1, 0));
            pieceMovements.Add(currentLocation + new Location(1, 1));

            Checker.removeInvalidMoves(pieceMovements);
            return pieceMovements;
        }
    }
}
