global using Position = (int x, int y);
using System.Diagnostics;

Stopwatch sw = new();
sw.Start();

// Read the file
const bool IS_TEST = false;

StreamReader stream = IS_TEST switch
{
    true => new("..\\..\\..\\res\\sample01.txt"),
    _ => new("..\\..\\..\\res\\input01.txt"),
};

string? line;
List<List<int>> map = [];

while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;

    map.Add(line.Select(c => int.Parse(c.ToString())).ToList());
}

Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();


// PART 1

// find zeros
List<Position> zeros = [];

for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[i].Count; j++)
    {
        if (map[i][j] == 0) zeros.Add((j, i));
    }
}

var sum = 0;

foreach (var startingPosition in zeros)
{
    sum += FindTrails(startingPosition).Count;
}

// Count how many finishing points exists in paths that finish in a 9
HashSet<Position> FindTrails(Position startingPosition, int startingValue = 0)
{
    if (startingValue == 9)
    {
        return [startingPosition];
    }

    HashSet<Position> finishingPositions = [];

    var (x, y) = startingPosition;

    // Check north
    if (y > 0 && map[y - 1][x] == startingValue + 1)
    {
        finishingPositions.UnionWith(FindTrails((x, y - 1), startingValue + 1));
    }
    // Check south
    if (y < map.Count - 1 && map[y + 1][x] == startingValue + 1)
    {
        finishingPositions.UnionWith(FindTrails((x, y + 1), startingValue + 1));
    }
    // Check west
    if (x > 0 && map[y][x - 1] == startingValue + 1)
    {
        finishingPositions.UnionWith(FindTrails((x - 1, y), startingValue + 1));
    }
    // Check east
    if (x < map[y].Count - 1 && map[y][x + 1] == startingValue + 1)
    {
        finishingPositions.UnionWith(FindTrails((x + 1, y), startingValue + 1));
    }

    return finishingPositions;
}

Console.WriteLine($"Part 1 sum: {sum}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();

Console.WriteLine($"Part 1 sum: {sum}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();


// PART 2
sum = 0;

foreach (var startingPosition in zeros)
{
    sum += CountTrails(startingPosition).Count;
}

// Count how many paths exists finishing in a 9
List<Position> CountTrails(Position startingPosition, int startingValue = 0)
{
    if (startingValue == 9)
    {
        return [startingPosition];
    }

    List<Position> finishingPositions = [];

    var (x, y) = startingPosition;

    // Check north
    if (y > 0 && map[y - 1][x] == startingValue + 1)
    {
        finishingPositions.AddRange(CountTrails((x, y - 1), startingValue + 1));
    }
    // Check south
    if (y < map.Count - 1 && map[y + 1][x] == startingValue + 1)
    {
        finishingPositions.AddRange(CountTrails((x, y + 1), startingValue + 1));
    }
    // Check west
    if (x > 0 && map[y][x - 1] == startingValue + 1)
    {
        finishingPositions.AddRange(CountTrails((x - 1, y), startingValue + 1));
    }
    // Check east
    if (x < map[y].Count - 1 && map[y][x + 1] == startingValue + 1)
    {
        finishingPositions.AddRange(CountTrails((x + 1, y), startingValue + 1));
    }

    return finishingPositions;
}

Console.WriteLine($"Part 2 sum: {sum}");
Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();
