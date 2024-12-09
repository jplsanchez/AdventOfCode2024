using System.Diagnostics;
using ID = System.Int32;

Stopwatch sw = new();
sw.Start();

// Read the file
const bool IS_TEST = false;

StreamReader stream = IS_TEST switch
{
    true => new("..\\..\\..\\res\\sample01.txt"),
    _ => new("..\\..\\..\\res\\input01.txt"),
};

List<int> input = stream.ReadLine()!.ToCharArray().Select(c => int.Parse(c.ToString())).ToList();

Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 1
List<(ID?, int)> diskMap = [];
List<int?> diskSpread = [];

// mapping fileMapping
for (int i = 0; i < input.Count; i++)
{
    int quantity = input[i];

    if (i % 2 == 0)
    {
        int id = i / 2;
        diskMap.Add((id, quantity));
        Enumerable.Range(0, quantity).ToList().ForEach(x => diskSpread.Add(id));
    }
    else
    {
        diskMap.Add((null, quantity));
        Enumerable.Range(0, quantity).ToList().ForEach(x => diskSpread.Add(null));
    }
}
Console.WriteLine();

// reordering
for (int i = 0; i < diskSpread.Count; i++)
{
    var id = diskSpread[i];
    if (id is not null) continue;

    // find and swap the first file found backwards
    for (int j = diskSpread.Count - 1; j >= 0; j--)
    {
        var searchId = diskSpread[j];
        if (searchId is not null)
        {
            diskSpread[i] = searchId;
            diskSpread[j] = null;
            break;
        }
    }
}

// ajust last element
for (int i = 0; i < diskSpread.Count; i++)
{
    int? item = diskSpread[i];
    if (item is null)
    {
        diskSpread[i] = diskSpread.Last();
        diskSpread[^1] = null;
        break;
    }
}

// count checksum
long sum = 0;

for (int i = 0; i < diskSpread.Count; i++)
{
    sum += i * diskSpread[i] ?? 0;
}

Console.WriteLine($"Part 1 sum: {sum}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 2

var currentId = diskMap[^1].Item1;
while (currentId > 0)
{
    var size = diskMap.First(x => x.Item1 == currentId).Item2;
    var pos = diskMap.IndexOf((currentId, size));

    for (int i = 0; i < pos; i++)
    {
        var (searchId, searchSize) = diskMap[i];
        if (searchId is not null || searchSize < size) continue;

        diskMap[pos] = (null, size);
        diskMap[i] = (currentId, size);
        if (searchSize != size)
        {
            diskMap.Insert(i + 1, (null, searchSize - size));
        }
        break;
    }

    currentId--;
}

// to spread diskMap representation
diskSpread = [];
foreach (var (id, size) in diskMap)
{
    for (int i = 0; i < size; i++)
    {
        diskSpread.Add(id);
    }
}

// count checksum
sum = 0;

for (int i = 0; i < diskSpread.Count; i++)
{
    sum += i * diskSpread[i] ?? 0;
}

Console.WriteLine($"Part 2 sum: {sum}");
Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();