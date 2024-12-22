using System.Diagnostics;

namespace AoC.Day22;

public static class Program
{
    private static void Main(string[] args)
    {
        Stopwatch sw = new();
        sw.Start();

        // Read the file
# if (DEBUG)
        const bool IS_TEST = true;
#else
        const bool IS_TEST = false;
#endif

        StreamReader stream = IS_TEST switch
        {
            true => new("..\\..\\..\\res\\sample02.txt"),
            _ => new("..\\..\\..\\res\\input01.txt"),
        };

        string? line;
        List<ulong> baseSecrets = [];

        while ((line = stream.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            baseSecrets.Add(ulong.Parse(line));
        }



        Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
        sw.Restart();


        // PART 1
        ulong sum = 0;

        foreach (var baseSecret in baseSecrets)
        {
            sum += baseSecret.CalculateSecret(2000);
        }


        Console.WriteLine($"Part 1 sum: {sum}");
        Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
        sw.Restart();

        // PART 2

        List<(ulong baseSecret, List<int> prices, List<int> differences)> buyers = [];

        // Calculate the prices and differences for each buyer
        foreach (var baseSecret in baseSecrets)
        {
            ulong currentSecret = baseSecret;
            List<int> prices = [currentSecret.CalculatePrice()];
            List<int> differences = [];

            for (int i = 0; i < 2000; i++)
            {
                currentSecret = currentSecret.CalculateSecret();
                differences.Add(currentSecret.CalculatePrice() - prices.Last());
                prices.Add(currentSecret.CalculatePrice());
            }

            buyers.Add((baseSecret, prices, differences));
        }

        // Get all possible diferent sequences of 4 differences
        HashSet<(int, int, int, int)> sequences = [];

        foreach (var (_, _, differences) in buyers)
        {
            for (int i = 0; i < differences.Count - 4; i++)
            {
                var current = differences.GetRange(i, 4).ToSequence();
                sequences.Add(current);
            }
        }

        // Find the sequence that gives the highest sum of prices
        sum = 0;

        foreach (var (sequence, i) in sequences.Select((value, i) => (value, i)))
        {
            var current = buyers.AggregatePricesByDifferences(sequence.ToList());
            if (current > sum) sum = current;
        }

        Console.WriteLine($"Part 2 sum: {sum}");
        Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
        sw.Stop();
    }

    private static ulong AggregatePricesByDifferences(this List<(ulong baseSecret, List<int> prices, List<int> differences)> buyers, List<int> differences)
    {
        ulong sum = 0;

        buyers.AsParallel().ForAll(buyer =>
        {
            int price = buyer.FindAppearenceInDifferences(differences);
            sum += (ulong)price;
        });

        return sum;
    }

    public static int FindAppearenceInDifferences(this (ulong baseSecret, List<int> prices, List<int> differences) buyer, List<int> sequence)
    {
        for (int i = 0; i < buyer.differences.Count - sequence.Count; i++)
        {
            bool found = true;
            for (int j = 0; j < sequence.Count; j++)
            {
                if (buyer.differences[i + j] != sequence[j])
                {
                    found = false;
                    break;
                }
            }
            if (found) return buyer.prices[i + sequence.Count];
        }

        return 0;
    }


    public static ulong CalculateSecret(this ulong baseSecret, int steps = 1)
    {
        ulong current = baseSecret;

        for (int i = 0; i < steps; i++)
        {
            current = current.Mix(current * 64).Prune();
            current = current.Mix(current / 32).Prune();
            current = current.Mix(current * 2048).Prune();
        }

        return current;
    }

    public static int CalculatePrice(this ulong secret) => (int)(secret % 10);

    public static ulong Mix(this ulong value, ulong other)
    {
        return value ^ other;
    }

    public static ulong Prune(this ulong value)
    {
        return value % 16777216;
    }

    public static List<int> ToList(this (int, int, int, int) sequence) => [sequence.Item1, sequence.Item2, sequence.Item3, sequence.Item4];

    public static (int, int, int, int) ToSequence(this List<int> list) => (list[0], list[1], list[2], list[3]);
}