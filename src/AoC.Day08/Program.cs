global using Position = (int x, int y);
global using Vector = (int x, int y);
using AoC.Day08;
using System.Diagnostics;

Stopwatch sw = new();
sw.Start();

const bool IS_TEST = true;

StreamReader stream = IS_TEST switch
{
    true => new("..\\..\\..\\res\\sample01.txt"),
    _ => new("..\\..\\..\\res\\input01.txt"),
};

#region Reading
string? line;

List<List<char>> map = [];

while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;

    map.Add([.. line.ToCharArray()]);
}

List<List<char>> backupMap = map.DeepCopy();

Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();
#endregion

#region PART 1

// PART 1
Dictionary<char, List<Position>> positions = [];

for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[0].Count; j++)
    {
        if (map[i][j] == '.') continue;
        if (!positions.ContainsKey(map[i][j])) positions[map[i][j]] = [];

        positions[map[i][j]].Add((j, i));
    }
}

HashSet<Position> antinodes = [];

// find resonant nodes
foreach (var item in positions)
{
    for (int i = 0; i < item.Value.Count; i++)
    {
        var reference = item.Value[i];
        for (int j = i + 1; j < item.Value.Count; j++)
        {
            var node = item.Value[j];

            Vector distance = (node.x - reference.x, node.y - reference.y);

            antinodes.Add((node.x + distance.x, node.y + distance.y));
            antinodes.Add((reference.x - distance.x, reference.y - distance.y));
        }
    }
}

// Paint antinodes
foreach (var (x, y) in antinodes)
{
    try
    {
        map[y][x] = '#';
    }
    catch (ArgumentOutOfRangeException)
    {
        continue;
    }
}

// sum antinodes
var sum = 0;

for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[0].Count; j++)
    {
        if (map[i][j] == '#') sum++;
    }
}

Console.WriteLine($"Part 1 sum: {sum}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();
#endregion

#region PART 2
// PART 2

map = backupMap.DeepCopy();
antinodes.Clear();

List<Position> allNodes = [];

foreach (var item in positions)
{
    allNodes.AddRange(item.Value);
}


HashSet<(Position, Position)> checkedPair = [];
var hasNewNode = true;
while (hasNewNode)
{
    hasNewNode = false;
    for (int i = 0; i < allNodes.Count; i++)
    {
        var reference = allNodes[i];
        for (int j = i + 1; j < allNodes.Count; j++)
        {
            var node = allNodes[j];

            if (checkedPair.Contains((reference, node))) continue;
            checkedPair.Add((reference, node));

            Vector distance = (node.x - reference.x, node.y - reference.y);
            Position antinode1 = (node.x + distance.x, node.y + distance.y);

            if (antinode1.x >= 0 && antinode1.x < map[0].Count && antinode1.y >= 0 && antinode1.y < map.Count)
            {
                map[antinode1.y][antinode1.x] = '#';
                antinodes.Add(antinode1);
                hasNewNode = true;

            }
            Position antinode2 = (reference.x - distance.x, reference.y - distance.y);
            if (antinode2.x >= 0 && antinode2.x < map[0].Count && antinode2.y >= 0 && antinode2.y < map.Count)
            {
                map[antinode2.y][antinode2.x] = '#';
                antinodes.Add(antinode2);
                hasNewNode = true;
            }
        }
    }

}


// sum antinodes
sum = 0;

for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[0].Count; j++)
    {
        if (map[i][j] == '#') sum++;
    }
}

map.ForEach(x => Console.WriteLine(string.Join("", x)));

Console.WriteLine($"Part 2 sum: {sum}");
Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();
#endregion


