using AoC.Day04;
using System.Diagnostics;

Stopwatch sw = new();
sw.Start();

//var stream = new StreamReader("..\\..\\..\\res\\sample01.txt");
var stream = new StreamReader("..\\..\\..\\res\\input01.txt");


long sum = 0;
List<List<char>> chars = [];

// Read the file
string? line;
while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;

    chars.Add([.. line.ToCharArray()]);
}

Console.WriteLine($"Read lines in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 1
//  Count the appearance of xmax in a axis
Console.WriteLine("Horizontal");
sum += chars.CountXMAS();

// count the appearance of xmax in a vertical axis
Console.WriteLine("\nVertical");
sum += chars.Transpose().CountXMAS();


// Count the appearance of xmax in first diagonal axis
Console.WriteLine("\n/");
sum += chars.RotateMatrixBy45Degrees().CountXMAS();


// Count the appearance of xmax second diagonal axis
Console.WriteLine("\n\\");
sum += chars.MirrorVertical().RotateMatrixBy45Degrees().CountXMAS();



Console.WriteLine($"Part 1: Sum of matches found: {sum}");
Console.WriteLine($"Part 1: Elapsed time: {sw.ElapsedMilliseconds}ms");
sw.Restart();

// Part 2
sum = 0;

for (int i = 0; i < chars[0].Count; i++)
{
    for (int j = 0; j < chars.Count; j++)
    {

        if (chars.CheckX_MAS((i, j))) sum++;
    }
}

Console.WriteLine($"Part 2: Sum of matches found: {sum}");
Console.WriteLine($"Part 2: Elapsed time: {sw.ElapsedMilliseconds}ms");
sw.Stop();




