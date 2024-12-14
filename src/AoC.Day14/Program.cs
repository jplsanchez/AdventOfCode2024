using System.Diagnostics;
using System.Text.RegularExpressions;

Stopwatch sw = new();
sw.Start();

// Read the file
const bool IS_TEST = false;

const long X_LENGHT = IS_TEST ? 11 : 101;
const long Y_LENGHT = IS_TEST ? 7 : 103;
const long NUMBER_OF_TIMES = 100;

StreamReader stream = IS_TEST switch
{
    true => new("..\\..\\..\\res\\sample01.txt"),
    _ => new("..\\..\\..\\res\\input01.txt"),
};

string? line;
List<Robot> robots = [];

while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;

    var data = Regex.Match(line, @"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");

    robots.Add(new Robot(
        new Position(int.Parse(data.Groups[1].Value), int.Parse(data.Groups[2].Value)),
        new Speed(int.Parse(data.Groups[3].Value), int.Parse(data.Groups[4].Value))
    ));
}

List<Robot> robotsBackup = robots.Select(static r => new Robot(r.Position, r.Speed)).ToList();

Console.WriteLine($"Robots loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();


// PART 1

foreach (var robot in robots)
{
    robot.Move(X_LENGHT, Y_LENGHT, NUMBER_OF_TIMES);
}
Console.WriteLine();


long middleX = X_LENGHT / 2;
long middleY = Y_LENGHT / 2;

int mult = 1;
mult *= robots.Count(robots => robots.Position.X < middleX && robots.Position.Y < middleY);
mult *= robots.Count(robots => robots.Position.X < middleX && robots.Position.Y > middleY);
mult *= robots.Count(robots => robots.Position.X > middleX && robots.Position.Y < middleY);
mult *= robots.Count(robots => robots.Position.X > middleX && robots.Position.Y > middleY);

Console.WriteLine($"Part 1 mult: {mult}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 2
robots = [.. robotsBackup];

for (int i = 0; i < 10000; i++)
{
    foreach (var robot in robots)
    {
        robot.Move(X_LENGHT, Y_LENGHT);
    }

    var mapAsStrings = MapHelper.MapToStrings(robots, X_LENGHT, Y_LENGHT);

    foreach (var mapLine in mapAsStrings)
    {
        if (mapLine.Contains("########"))
        {
            MapHelper.PrintMap(MapHelper.MapToStrings(robots, X_LENGHT, Y_LENGHT));

            Console.WriteLine($"Part 2 tree found at {i + 1}s");
            Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
            sw.Stop();

            return;
        }
    }
}