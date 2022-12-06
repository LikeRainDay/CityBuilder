public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj is Point point)
        {
            return this.X == point.X && this.Y == point.Y;
        }

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 6949;
            hash = hash * 7907 + X.GetHashCode();
            hash = hash * 7907 + Y.GetHashCode();
            return hash;
        }
    }
}