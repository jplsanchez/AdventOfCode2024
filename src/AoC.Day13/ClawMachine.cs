public class ClawMachine(Point A, Point B, Point Prize)
{
    private const long ADJUSTED_VALUE = 10_000_000_000_000;
    private Point _adjustedPrize => new(Prize.X + ADJUSTED_VALUE, Prize.Y + ADJUSTED_VALUE);

    public long? GetNumberOfPresses()
    {
        // [A.x B.x][pressesA]=[Prize.x]
        // [A.y B.y][pressesB] [Prize.y]
        long det = A.X * B.Y - B.X * A.Y;
        if (det == 0) return null;

        if ((Prize.X * B.Y - Prize.Y * B.X) % det != 0) return null;
        long pressesA = (Prize.X * B.Y - Prize.Y * B.X) / det;

        if ((Prize.Y * A.X - Prize.X * A.Y) % det != 0) return null;
        long pressesB = (Prize.Y * A.X - Prize.X * A.Y) / det;

        if (pressesA > 100 || pressesB > 100) return null;

        return pressesA * 3 + pressesB;
    }

    public long? GetAdjustedNumberOfPresses()
    {
        // [A.x B.x][pressesA]=[_adjustedPrize.x]
        // [A.y B.y][pressesB] [_adjustedPrize.y]
        long det = A.X * B.Y - B.X * A.Y;
        if (det == 0) return null;

        if ((_adjustedPrize.X * B.Y - _adjustedPrize.Y * B.X) % det != 0) return null;
        long pressesA = (_adjustedPrize.X * B.Y - _adjustedPrize.Y * B.X) / det;

        if ((_adjustedPrize.Y * A.X - _adjustedPrize.X * A.Y) % det != 0) return null;
        long pressesB = (_adjustedPrize.Y * A.X - _adjustedPrize.X * A.Y) / det;

        return pressesA * 3 + pressesB;
    }
}