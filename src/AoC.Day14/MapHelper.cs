public static class MapHelper
{
    public static void PrintMap(List<Robot> robots, long xLenght, long yLength)
    {
        Dictionary<Position, int> positions = [];

        foreach (var robot in robots)
        {
            if (!positions.ContainsKey(robot.Position))
            {
                positions[robot.Position] = 0;
            }

            positions[robot.Position] += 1;
        }

        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLenght; x++)
            {
                if (positions.ContainsKey(new Position(x, y)))
                {
                    Console.Write(positions[new Position(x, y)]);
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }

    public static List<string> MapToStrings(List<Robot> robots, long xLenght, long yLength)
    {
        Dictionary<Position, int> positions = [];

        foreach (var robot in robots)
        {
            if (!positions.ContainsKey(robot.Position))
            {
                positions[robot.Position] = 0;
            }

            positions[robot.Position] += 1;
        }

        List<List<char>> mapChar = [];

        for (int y = 0; y < yLength; y++)
        {
            mapChar.Add(new List<char>());
            for (int x = 0; x < xLenght; x++)
            {
                if (positions.ContainsKey(new Position(x, y)))
                {
                    mapChar[y].Add('#');
                }
                else
                {
                    mapChar[y].Add('.');
                }
            }
        }

        return mapChar.Select(x => string.Join("", x)).ToList();
    }

    public static void PrintMap(List<string> map)
    {
        foreach (var line in map)
        {
            Console.WriteLine(line);
        }
    }
}