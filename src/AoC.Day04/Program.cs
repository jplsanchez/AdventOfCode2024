using System.Text.RegularExpressions;
using Xunit;

TestRotateMatrixBy45Degrees();


return;

var stream = new StreamReader("..\\..\\..\\res\\sample01.txt");
//var stream = new StreamReader("..\\..\\..\\res\\input01.txt");


long sum = 0;
List<List<char>> chars = [];

// Read the file
string? line;
while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;

    chars.Add([.. line.ToCharArray()]);
}


// PART 1
//  Count the appearance of xmax in x axis
foreach (var row in chars)
{
    Regex regex = new Regex("XMAS|SAMX");
    sum += regex.Count(new string([.. row])!);
}

// Count the appearance of xmax in diagonal 1 axis
//List<List<char>> rotated45 = RotateMatrixBy45Degrees(values);

var rotated45 = RotateMatrixBy45Degrees(chars);
foreach (var row in rotated45)
{
    Regex regex = new Regex("XMAS|SAMX");
    sum += regex.Count(new string([.. row])!);
}

// Count the appearance of xmax in y axis




// Count the appearance of xmax in diagonal 2 axis


// Can rotate only rectangular matrizes => the result cannot be introduce din the func
static List<List<T>> RotateMatrixBy45Degrees<T>(List<List<T>> values)
{
    List<List<T>> rotated = [];
    int length = values[0].Count;
    int height = values.Count;
    int newLength = height + length - 1;
    Console.WriteLine($"length is {length}");
    Console.WriteLine($"height is {height}");
    for (int r = 0; r <= newLength; r++)
    {
        List<T> newRow = [];
        int i = r < length ? 0 : (r + 1) - length;
        int j = r < length ? r : (length - 1);
        while (i < length && j >= 0)
        {
            newRow.Add(values[i][j]);
            Console.WriteLine($"{values[i][j]} - {i}{j}");
            j--;
            i++;
        }
        rotated.Add(newRow);
    }
    return rotated;
}

Console.WriteLine($"Sum of matches found: {sum}");



[Fact]
void TestRotateMatrixBy45Degrees()
{
    List<List<int>> chars = [[1, 2], [3, 4], [5, 6], [7, 8]];

    printList(chars);

    printList(RotateMatrixBy45Degrees(chars));

    chars = [[1, 2, 3, 4], [5, 6, 7, 8]];

    printList(chars);

    printList(RotateMatrixBy45Degrees(chars));
}

void printList(List<List<int>> chars)
{
    foreach (var item in chars)
    {
        Console.WriteLine(new string([.. item.Select(i => char.Parse($"{i}"))])!);
    };
}