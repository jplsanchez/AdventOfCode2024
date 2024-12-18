using System.Diagnostics;
using System.Text.RegularExpressions;

Stopwatch sw = new();
sw.Start();

// Read the file
const bool IS_TEST = false;

(int x, int y) LastPosition = IS_TEST ? (6, 6) : (70, 70);
int Range = IS_TEST ? 12 : 1024;

StreamReader stream = IS_TEST switch
{
    true => new("..\\..\\..\\res\\sample01.txt"),
    _ => new("..\\..\\..\\res\\input01.txt"),
};

string? line;

List<(int x, int y)> allFallPositions = [];



while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;

    var match = Regex.Match(line, @"(\d+),(\d+)");

    allFallPositions.Add((int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)));
}


List<(int x, int y)> fallPositions = allFallPositions[..Range];

Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();


// PART 1
(int x, int y)[] DIRECTIONS = [(1, 0), (-1, 0), (0, 1), (0, -1)];

HashSet<(int x, int y)> visited = [];
Dictionary<(int x, int y), (int distance, (int x, int y) last)> distances = [];
(int x, int y) current = (0, 0);

distances[current] = (0, current);

while (current != LastPosition)
{
    visited.Add(current);
    var currentDistance = distances[current].distance;

    foreach (var direction in DIRECTIONS)
    {
        (int x, int y) next = (current.x + direction.x, current.y + direction.y);

        if (visited.Contains(next)) continue;
        if (fallPositions.Contains(next)) continue;
        if (next.x < 0 || next.y < 0) continue;
        if (next.x > LastPosition.x || next.y > LastPosition.y) continue;

        if (!distances.ContainsKey(next) || distances[next].distance > currentDistance + 1)
        {
            distances[next] = (currentDistance + 1, current);
        }
    }

    current = distances.Where(v => !visited.Contains(v.Key)).OrderBy(v => v.Value.distance).FirstOrDefault().Key;
    if (current == default) break;
}

HashSet<(int x, int y)> path = [];

current = LastPosition;
while (current != (0, 0))
{
    path.Add(current);
    current = distances[current].last;
}

long sum = path.Count;

Console.WriteLine($"Part 1 sum: {sum}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 2

(int x, int y) resultPosition = default;
int count = Range;

while (!ItBreaks(fallPositions, out resultPosition))
{
    count++;
    fallPositions = allFallPositions[..count];
}

Console.WriteLine($"Part 2 position is: {resultPosition.x},{resultPosition.y}");
Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();

bool ItBreaks(List<(int x, int y)> fallPositions, out (int x, int y) breakPosition)
{
    breakPosition = fallPositions.LastOrDefault();
    (int x, int y)[] DIRECTIONS = [(1, 0), (-1, 0), (0, 1), (0, -1)];

    HashSet<(int x, int y)> visited = [];
    Dictionary<(int x, int y), (int distance, (int x, int y) last)> distances = [];
    (int x, int y) current = (0, 0);

    distances[current] = (0, current);

    while (current != LastPosition)
    {
        visited.Add(current);
        var currentDistance = distances[current].distance;

        foreach (var direction in DIRECTIONS)
        {
            (int x, int y) next = (current.x + direction.x, current.y + direction.y);

            if (visited.Contains(next)) continue;
            if (fallPositions.Contains(next)) continue;
            if (next.x < 0 || next.y < 0) continue;
            if (next.x > LastPosition.x || next.y > LastPosition.y) continue;

            if (!distances.ContainsKey(next) || distances[next].distance > currentDistance + 1)
            {
                distances[next] = (currentDistance + 1, current);
            }
        }

        current = distances.Where(v => !visited.Contains(v.Key)).OrderBy(v => v.Value.distance).FirstOrDefault().Key;
        if (current == default) break;
        if (current == LastPosition) visited.Add(LastPosition);
    }

    if (!visited.Contains(LastPosition)) return true;

    return false;
}