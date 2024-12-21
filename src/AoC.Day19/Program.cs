using System.Diagnostics;
using System.Text.RegularExpressions;

Stopwatch sw = new();
sw.Start();

// Read the file
#if (DEBUG)
const bool IS_TEST = true;
#else 
const bool IS_TEST = false;
#endif

StreamReader stream = IS_TEST switch
{
    true => new("..\\..\\..\\res\\sample01.txt"),
    _ => new("..\\..\\..\\res\\input01.txt"),
};

string? line;

List<string> patterns = [.. stream.ReadLine()!.Split(", ")];
List<string> towels = [];
while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;
    towels.Add(line);
}

Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();


// PART 1
long sum = 0;

string regex = @"^(" + string.Join("|", patterns) + ")+$";

foreach (var towel in towels)
{
    if (Regex.IsMatch(towel, regex)) sum++;
}

Console.WriteLine($"Part 1 sum: {sum}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 2
sum = 0;

for (int i = 0; i < towels.Count; i++)
{
    var internalSW = new Stopwatch();
    internalSW.Start();
    string? towel = towels[i];
    Console.WriteLine($"{i + 1}/{towels.Count}");
    sum += new Towel(towels[i], patterns).CountMatches();
    Console.WriteLine($"Took {internalSW.ElapsedMilliseconds}ms");
    internalSW.Stop();
}


Console.WriteLine($"Part 2 sum: {sum}");
Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();

public class Towel(string Pattern, List<string> SubPatterns)
{
    public long CountMatches(int pos = 0)
    {
        if (pos == Pattern.Length) return 1;

        long currentCount = 0;

        foreach (var subPattern in SubPatterns)
        {
            if (Pattern[pos..].StartsWith(subPattern)) currentCount += CountMatches(pos + subPattern.Length);

            // TODO add memoization
        }

        return currentCount;
    }
}
