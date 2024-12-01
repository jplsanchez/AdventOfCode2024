List<int> listA = [];
List<int> listB = [];


// Read the file
//var stream = new StreamReader("..\\..\\..\\res\\sample01.txt");
var stream = new StreamReader("..\\..\\..\\res\\input01.txt");
string? line;
while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;

    listA.Add(int.Parse(line.Split(' ')[0]));
    listB.Add(int.Parse(line.Split(' ')[^1]));
}

// Order the lists
List<int> sortedListA = [.. listA];
sortedListA.Sort();

List<int> sortedListB = [.. listB];
sortedListB.Sort();

// ################
// #### PART 1 ####
// ################

// Find the differences
long sum = 0;

for (int i = 0; i < sortedListA.Count; i++)
{
    var diff = sortedListB[i] - sortedListA[i];
    if (diff < 0) diff *= -1;
    //Console.WriteLine($"|{sortedListB[i]} - {sortedListA[i]}| = {diff}");
    sum += diff;
}

Console.WriteLine($"Sum of differences is: {sum}");

// ################
// #### PART 2 ####
// ################

// Find appearances of each number in list B
Dictionary<int, long> appearancesInB = [];

for (int i = 0; i < sortedListB.Count; i++)
{
    var currentNumber = sortedListB[i];
    var currentCount = 1;

    while (i + 1 < sortedListB.Count && sortedListB[i] == sortedListB[i + 1])
    {
        i++;
        currentCount++;
    }

    appearancesInB.Add(currentNumber, currentCount);
}

// Sum of Similarity Scores
long totalSimilarityScore = 0;

foreach (var key in listA)
{
    if (appearancesInB.TryGetValue(key, out long value))
    {
        totalSimilarityScore += value * key;
        //Console.WriteLine(value: $"{key} => {value * key}");
    }
    //else Console.WriteLine($"{key} => 0");
}

Console.WriteLine($"Total Similarity Score is: {totalSimilarityScore}");