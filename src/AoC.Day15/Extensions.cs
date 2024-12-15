namespace AoC.Day15;

internal static class Extensions
{
    internal static bool Contains(this HashSet<(Position, Position)> values, Position value)
    {
        foreach (var (a, b) in values)
        {
            if (a == value || b == value) return true;
        }
        return false;
    }
}
