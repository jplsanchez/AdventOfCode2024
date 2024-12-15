using System.Diagnostics;

Stopwatch sw = new();
sw.Start();

// Read the file
const bool IS_TEST = true;

StreamReader stream = IS_TEST switch
{
    true => new("..\\..\\..\\res\\sample01.txt"),
    _ => new("..\\..\\..\\res\\input01.txt"),
};

string? line;
List<List<char>> map = [];

while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;
    map.Add([.. line.ToCharArray()]);
}

Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();

# region Part 1

// PART 1
List<Region> regions = [];

// Find Regions
for (int y = 0; y < map.Count; y++)
{
    for (int x = 0; x < map[y].Count; x++)
    {
        char value = map[y][x];
        Position position = new(x, y);

        if (regions.Contains(position, map) continue;

        Region.Create(value, position);
    }
}


long sum = 0;

Console.WriteLine($"Part 1 sum: {sum}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();
# endregion


# region Part 2
// PART 2
sum = 0;


Console.WriteLine($"Part 2 sum: {sum}");
Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();
# endregion


internal class Region
{
    public char Type { get; init; }
    private readonly HashSet<Position> _positions = [];
    public bool Contains(Position position) => _positions.Contains(position);
    public bool Add(Position position) => _positions.Add(position);

    public static Region Create(Position position, List<List<char>> map)
    {
        Region region = new() { Type = map[position.Y][position.X] };
        region.Add(position);

        Position current = position;
        Position next = position;

        // Load region
        next = new(current.X, current.Y + 1);

        ..TODO






        return region;
    }


}

public struct Position(int X, int Y)
{
    public int X { get; } = X;
    public int Y { get; } = Y;
}


public static class Extensions
{
    public static bool Contains(this List<Region> regions, Position position)
    {
        foreach (Region region in regions)
        {
            if (region.Contains(position)) return true;
        }
        return false;
    }
}