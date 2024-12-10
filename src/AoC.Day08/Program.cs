global using Position = (int x, int y);
global using Vector = (int x, int y);
using AoC.Day08;
using System.Diagnostics;

Stopwatch sw = new();
sw.Start();

const bool IS_TEST = false;

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
            Vector normal = distance;
            while (true)
            {
                if (normal.x % 2 != 0 || normal.y % 2 != 0) break;
                normal = (normal.x / 2, normal.y / 2);
                if (normal.x <= 1 && normal.y <= 1) break;
                map.ForEach(x => Console.WriteLine(string.Join("", x)));

            }

            var position1 = (reference.x, reference.y);
            while (IsInbounds(position1))
            {
                antinodes.Add(position1);
                position1 = (position1.x + normal.x, position1.y + normal.y);
            }

            var position2 = (node.x, node.y);
            while (IsInbounds(position2))
            {
                antinodes.Add(position2);
                position2 = (position2.x - normal.x, position2.y - normal.y);
            }

        }
    }
}

bool IsInbounds(Position pos)
{
    if (pos.x < 0 || pos.y < 0) return false;
    if (pos.y >= map.Count || pos.x >= map[0].Count) return false;
    return true;
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


