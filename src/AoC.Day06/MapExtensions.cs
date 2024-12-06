namespace AoC.Day06;

internal static class MapExtensions
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
    internal static bool IsCircularPath(this List<List<char>> map, Guard guard)
    {
        HashSet<PositionLog> log = [];

        while (!log.Contains(PositionLog.LogFrom(guard)))
        {
            log.Add(PositionLog.LogFrom(guard));

            if (guard.NextStep().x < 0
                || guard.NextStep().x >= map[0].Count
                || guard.NextStep().y < 0
                || guard.NextStep().y >= map.Count) return false;

            char nextChar = map[guard.NextStep().y][guard.NextStep().x];

            if (nextChar == '#' || nextChar == 'O') guard.TurnRight();
            else guard.Move();
        }
        return true;
    }
}

