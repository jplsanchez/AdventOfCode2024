using AoC.Day06;
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
List<List<char>> map = [];
Guard guard = new((0, 0), Orientation.UP);

while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;
    if (line.Contains('^')) guard = new((line.IndexOf('^'), map.Count), Orientation.UP);
    map.Add([.. line.ToCharArray()]);
}

List<List<char>> mapBackup = map.DeepCopy();
Guard guardBackup = guard.DeepCopy();

Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();


// PART 1
try
{
    while (true)
    {
        map[guard.Position.y][guard.Position.x] = 'X';
        var next = guard.NextStep();
        var nextChar = map[next.y][next.x];

        if (nextChar == '#') guard.TurnRight();
        guard.Move();
    }
}
catch (ArgumentOutOfRangeException) { }

long sum = 0;
foreach (var row in map)
{
    sum += row.Count(c => c.Equals('X'));
}

Console.WriteLine($"Sum of tiles passed by the guard: {sum}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 2
sum = 0;

Parallel.For(0, map.Count, i =>
{
    var localMap = mapBackup.DeepCopy();
    var localGuard = guardBackup.DeepCopy();

    for (int j = 0; j < localMap[i].Count; j++)
    {
        if (localMap[i][j] == '#' || localMap[i][j] == '^') continue;
        localMap[i][j] = 'O';

        if (localMap.IsCircularPath(localGuard)) sum++;

        localMap = mapBackup.DeepCopy();
        localGuard = guardBackup.DeepCopy();
    }
});

Console.WriteLine($"Number of obstructions: {sum}");
Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();
