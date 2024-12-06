using System.Diagnostics;

var stopwatch = new Stopwatch();
stopwatch.Start();
// Read the file
//var stream = new StreamReader("..\\..\\..\\res\\sample01.txt");
var stream = new StreamReader("..\\..\\..\\res\\input01.txt");

RulesBuilder rulesBuilder = new();
List<List<int>> updates = [];

string? line;
while ((line = stream.ReadLine()) != null)
{
    // part 1
    if (string.IsNullOrWhiteSpace(line)) continue;

    // first section
    if (line.Contains('|')) rulesBuilder.Add((int.Parse(line.Split('|')[0]), int.Parse(line.Split('|')[1])));

    // second section
    else updates.Add(line.Split(',').Select(int.Parse).ToList());
}

// PART 1
long sum = 0;

Rules rules = rulesBuilder.Build();

foreach (var update in updates)
{
    if (rules.AreRespectedBy(update))
    {
        sum += update[update.Count / 2];
    }
}

Console.WriteLine($"sum is: {sum}");
Console.WriteLine("Elapsed time: {0} ms", stopwatch.ElapsedMilliseconds);
stopwatch.Restart();

// PART 2
sum = 0;

foreach (var update in updates)
{
    if (rules.AreRespectedBy(update)) continue;

    int notBefore, value;

    while (!rules.AreRespectedBy(update, out notBefore, out value))
    {
        update.Remove(notBefore);
        update.Insert(update.IndexOf(value), notBefore);
    }

    sum += update[update.Count / 2];
}


Console.WriteLine($"sum of corrected value is: {sum}");
Console.WriteLine("Elapsed time: {0} ms", stopwatch.ElapsedMilliseconds);
stopwatch.Stop();