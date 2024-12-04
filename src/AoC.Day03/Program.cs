
//var stream = new StreamReader("..\\..\\..\\res\\sample01.txt");
var stream = new StreamReader("..\\..\\..\\res\\input01.txt");


long sum = 0;

var text = stream.ReadToEnd();

// PART 1
foreach (var line in text.Split(')'))
{
    var found = line.LastIndexOf("mul(");
    if (found < 0) continue;

    var values = line.Substring(found + 3, (line.Length) - (found + 3)).Split([',', ')', '('], options: StringSplitOptions.RemoveEmptyEntries);

    if (values.Length < 2) continue;

    if (!long.TryParse(values[0], out long a) || !long.TryParse(values[1], out long b)) continue;

    sum += long.Parse(values[0]) * long.Parse(values[1]);
}

Console.WriteLine($"The sum of the multiplications is {sum}");

// PART 2
sum = 0;
bool isEnable = true;

foreach (var line in text.Split(')'))
{
    // check for don't
    if (line.LastIndexOf("don't(") >= 0)
    {
        isEnable = false;
        continue;
    }

    // check for do
    if (line.LastIndexOf("do(") >= 0)
    {
        isEnable = true;
        continue;
    }

    // if is enabled, do normal stuff
    if (!isEnable) continue;

    var found = line.LastIndexOf("mul(");
    if (found < 0) continue;


    var values = line.Substring(found + 3, (line.Length) - (found + 3)).Split([',', ')', '('], options: StringSplitOptions.RemoveEmptyEntries);

    if (values.Length < 2) continue;

    if (!long.TryParse(values[0], out long a) || !long.TryParse(values[1], out long b)) continue;

    sum += long.Parse(values[0]) * long.Parse(values[1]);
}

Console.WriteLine($"The sum of the multiplications with more instructions is {sum}");