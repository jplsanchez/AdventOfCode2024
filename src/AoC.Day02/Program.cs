using AoC.Day02.Types;

//var stream = new StreamReader("..\\..\\..\\res\\sample01.txt");
var stream = new StreamReader("..\\..\\..\\res\\input01.txt");

long sum = 0;
long sumTolerate = 0;

string? line;
while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;
    var list = line.Split(' ').Select(int.Parse).ToList();

    // Part 1
    if (CheckIfIsSafe(list))
    {
        sum++;
        continue;
    }

    // Part 2

    //// IM DONE WITH THIS - BRUTE FORCE HERE I COME...

    for (int i = 0; i < list.Count; i++)
    {
        if (CheckIfIsSafeRemovingAt(list, i))
        {
            sumTolerate++;
            break;
        }
    }
}

Console.WriteLine($"The sum is {sum}");
Console.WriteLine($"The sum with tolerate level is {sum + sumTolerate}");

static bool CheckIfIsSafeRemovingAt(List<int> list, int position)
{
    List<int> listCopy = [.. list];
    listCopy.RemoveAt(position);
    return CheckIfIsSafe(listCopy);
}

static IsSafeResult CheckIfIsSafe(List<int> list)
{
    // Set Ordering Type
    OrderingType type = (list[0], list[1]) switch
    {
        (int a, int b) when a < b => OrderingType.Increase,
        (int a, int b) when a > b => OrderingType.Decrease,
        _ => OrderingType.None
    };

    if (type == OrderingType.None) return new IsSafeResult(false, 0);

    //Check
    for (int i = 1; i < list.Count; i++)
    {
        var diff = list[i] - list[i - 1];

        if (!IsSafe(type, diff)) return new IsSafeResult(false, i + 1);

    }
    return new IsSafeResult(true);
}


static bool IsSafe(OrderingType type, int diff)
{
    if (type == OrderingType.Increase && (diff > 3 || diff < 1)) return false;
    if (type == OrderingType.Decrease && (diff < -3 || diff > -1)) return false;
    return true;
}
