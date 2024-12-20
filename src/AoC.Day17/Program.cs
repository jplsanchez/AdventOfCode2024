using System.Diagnostics;
using System.Text.RegularExpressions;

Stopwatch sw = new();
sw.Start();

// Read the file
const bool IS_TEST = true;

StreamReader stream = IS_TEST switch
{
    true => new("..\\..\\..\\res\\sample01.txt"),
    _ => new("..\\..\\..\\res\\input01.txt"),
};

string? line;
(uint A, uint B, uint C) registers = (0, 0, 0);
List<uint> program = [];
while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;

    if (Regex.IsMatch(line, @"^Register A:")) registers.A = uint.Parse(line.Split(' ').Last());
    if (Regex.IsMatch(line, @"^Register B:")) registers.B = uint.Parse(line.Split(' ').Last());
    if (Regex.IsMatch(line, @"^Register C:")) registers.C = uint.Parse(line.Split(' ').Last());
    if (Regex.IsMatch(line, @"^Program: ")) program = line.Split([',', ' '])[1..].Select(uint.Parse).ToList();
}



Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();


// PART 1
string result = string.Empty;

Computer computer = new(registers, program);
result = string.Join(",", computer.Run());

Console.WriteLine($"Part 1 result: {result}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 2
result = string.Empty;


Console.WriteLine($"Part 2 result: {result}");
Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();
