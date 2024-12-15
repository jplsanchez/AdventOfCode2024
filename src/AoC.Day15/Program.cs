using AoC.Day15;
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

bool isMovesMapping = false;
List<char> moves = [];
List<List<char>> grid = [];

while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        isMovesMapping = true;
        continue;
    }

    if (!isMovesMapping)
    {
        grid.Add([.. line.ToCharArray()]);
    }
    else
    {
        moves.AddRange(line.ToCharArray());
    }
}

Map map = new(grid);
WideMap wideMap = new(grid);


Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();


// PART 1
long sum = 0;

foreach (char move in moves)
{
    map.TryMove((Direction)move);
}

foreach (var box in map.Boxes)
{
    sum += box.X + 100 * box.Y;
}

Console.WriteLine($"Part 1 sum: {sum}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 2

////  Uncomment to play the game

//while (true)
//{
//    var key = Console.ReadKey();

//    if (key.Key == ConsoleKey.Escape) break;

//    char move = key.Key switch
//    {
//        ConsoleKey.W or ConsoleKey.UpArrow => '^',
//        ConsoleKey.S or ConsoleKey.DownArrow => 'v',
//        ConsoleKey.A or ConsoleKey.LeftArrow => '<',
//        ConsoleKey.D or ConsoleKey.RightArrow => '>',
//        _ => ' '
//    };

//    if (move == ' ') continue;
//    Console.Clear();

//    Console.WriteLine($"Move {move}:");
//    Console.WriteLine(wideMap.CanMoveRobot((Direction)move));
//    if (wideMap.CanMoveRobot((Direction)move)) wideMap.MoveRobot((Direction)move);
//    wideMap.Print(move);
//    Task.Delay(200).Wait();
//}

sum = 0;
foreach (char move in moves)
{
    if (wideMap.CanMoveRobot((Direction)move)) wideMap.MoveRobot((Direction)move);
}

foreach (var box in wideMap.Boxes)
{
    sum += box.Item1.X + 100 * box.Item1.Y;
}
Console.WriteLine($"Part 2 sum: {sum}");
Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();

