using System.Diagnostics;
using System.Text.RegularExpressions;

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
List<ClawMachine> clawMachines = [];

while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;

    // Parse A
    var regex = Regex.Match(line, @"X\+(\d+), Y\+(\d+)$");
    Point a = new(long.Parse(regex.Groups[1].Value), long.Parse(regex.Groups[2].Value));

    // Parse B
    line = stream.ReadLine();
    regex = Regex.Match(line!, @"X\+(\d+), Y\+(\d+)$");
    Point b = new(long.Parse(regex.Groups[1].Value), long.Parse(regex.Groups[2].Value));

    // Parse Prize
    line = stream.ReadLine();
    regex = Regex.Match(line!, @"X=(\d+), Y=(\d+)$");
    Point prize = new(long.Parse(regex.Groups[1].Value), long.Parse(regex.Groups[2].Value));


    clawMachines.Add(new(a, b, prize));
}

Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();


// PART 1
long sum = 0;
foreach (var clawMachine in clawMachines)
{
    var presses = clawMachine.GetNumberOfPresses();
    if (presses.HasValue)
    {
        sum += presses.Value;
    }
}

Console.WriteLine($"Part 1 sum: {sum}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 2
sum = 0;
foreach (var clawMachine in clawMachines)
{
    var presses = clawMachine.GetAdjustedNumberOfPresses();
    if (presses.HasValue)
    {
        sum += presses.Value;
    }
}

Console.WriteLine($"Part 2 sum: {sum}");
Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();


public record struct Point(long X, long Y);
