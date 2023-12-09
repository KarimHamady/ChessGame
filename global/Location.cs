namespace ChessGame.Global
{
    public struct Location
    {
        public int Rank { get; set; }
        public int File { get; set; }
        public Location(int rank, int file)
        {
            Rank = rank;
            File = file;
        }
        public readonly Location Inverted
        {
            get { return new Location(7 - Rank, 7 - File); }
        }
        public static Location operator +(Location a, Location b)
        {
            return new Location(a.Rank + b.Rank, a.File + b.File);
        }

        public static bool operator ==(Location a, Location b)
        {
            return a.Rank == b.Rank && a.File == b.File;
        }
        public static bool operator !=(Location a, Location b)
        {
            return !(a.Rank == b.Rank && a.File == b.File);
        }

        public override bool Equals(object? obj)
        {
            return obj is Location location &&
                   Rank == location.Rank &&
                   File == location.File;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Rank, File);
        }
    }
}
