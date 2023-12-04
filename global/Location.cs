
namespace ChessGame.global
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

    }
}
