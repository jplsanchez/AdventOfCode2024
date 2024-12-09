namespace AoC.Day08;

internal static class Extensions
{
    internal static List<List<char>> DeepCopy(this List<List<char>> map)
    {
        List<List<char>> copy = [];
        foreach (var row in map)
        {
            copy.Add([.. row]);
        }
        return copy;
    }
}
