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

string? line = stream.ReadLine();
stream.Close();

List<long> stones = line!.Split(' ').Select(long.Parse).ToList();
List<long> backup = [.. stones];

Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 1

int blinks = 25;
TimeSpan time = sw.Elapsed;

for (int b = 0; b < blinks; b++)
{
    time = sw.Elapsed;
    List<long> newStones = [];
    for (int i = 0; i < stones.Count; i++)
    {
        var str = stones[i].ToString();

        if (stones[i] == 0)
        {
            newStones.Add(1);
        }
        else if (str.Length % 2 == 0)
        {
            newStones.Add(long.Parse(str[..(str.Length / 2)]));
            newStones.Add(long.Parse(str[(str.Length / 2)..]));
        }
        else
        {
            newStones.Add(stones[i] * 2024);
        }
    }
    stones = newStones;
}

long sum = 0;
sum = stones.Count;

Console.WriteLine($"Part 1 sum: {sum}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 2
blinks = 75;
time = sw.Elapsed;
stones = [.. backup];

Dictionary<(long, int), long> PossibleResultsMap = [];
long CountStonesRecursiveWithMap(long value, int blinkLeft)
{
    if (PossibleResultsMap.TryGetValue((value, blinkLeft), out long cacheCount)) return cacheCount;

    if (blinkLeft == 0) return 1;

    var str = value.ToString();

    long count = str switch
    {
        "0" => CountStonesRecursiveWithMap(1, blinkLeft - 1),
        _ when str.Length % 2 == 0 =>
            CountStonesRecursiveWithMap(long.Parse(str[..(str.Length / 2)]), blinkLeft - 1)
            + CountStonesRecursiveWithMap(long.Parse(str[(str.Length / 2)..]), blinkLeft - 1),
        _ => CountStonesRecursiveWithMap(value * 2024, blinkLeft - 1),
    };

    PossibleResultsMap.Add((value, blinkLeft), count);
    return count;
}

sum = 0;
sum += stones.AsParallel().Aggregate(sum, (acc, stone) => acc + CountStonesRecursiveWithMap(stone, blinks));

Console.WriteLine($"Part 2 sum: {sum}");
Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();
