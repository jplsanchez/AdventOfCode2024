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
long sum = 0;
long sum2 = 0;

while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;

    // PART 1
    long result = long.Parse(line.Split(':')[0]);
    List<long> numbers = line.Split(' ').Select(long.Parse).Skip(1).ToList();


    Calculator calculator = new(result, numbers);

    if (calculator.PossibleSolutionExists())
    {
        sum += result;
    }

    // PART 2
    CalculatorExpanded calculatorExpanded = new(result, numbers);

    if (calculatorExpanded.PossibleSolutionExists())
    {
        sum2 += result;
    }
}

Console.WriteLine($"Part 1 Sum: {sum}");
Console.WriteLine($"Part 2 Sum: {sum2}");
Console.WriteLine($"Eerything ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();
