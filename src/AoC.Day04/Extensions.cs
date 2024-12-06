using System.Text.RegularExpressions;

namespace AoC.Day04;

internal static class Extensions
{
    public static long CountXMAS(this List<List<char>> chars)
    {
        long sum = 0;
        foreach (var row in chars)
        {
            Regex regex = new Regex("(?=(XMAS|SAMX))");
            sum += regex.Count(new string([.. row])!);
        }

        Console.WriteLine(sum);
        return sum;
    }

    public static List<List<T>> Transpose<T>(this List<List<T>> input)
    {
        List<List<T>> output = [];
        for (int i = 0; i < input[0].Count; i++)
        {
            List<T> row = [];
            for (int j = 0; j < input.Count; j++)
            {
                row.Add(input[j][i]);
            }
            output.Add(row);
        }
        return output;
    }

    public static List<List<T>> MirrorVertical<T>(this List<List<T>> input)
    {
        List<List<T>> output = [];
        for (int i = 0; i < input.Count; i++)
        {
            List<T> row = [];
            for (int j = input[0].Count - 1; j >= 0; j--)
            {
                row.Add(input[i][j]);
            }
            output.Add(row);
        }
        return output;
    }

    public static List<List<T>> RotateMatrixBy45Degrees<T>(this List<List<T>> input)
    {
        List<List<T>> output = [];

        // Rows
        for (int j = 0; j < input[0].Count; j++)
        {
            List<T> row = [];
            try
            {
                int a = 0;
                int b = j;

                while (true)
                {
                    row.Add(input[a][b]);

                    a++;
                    b--;
                }
            }
            catch (ArgumentOutOfRangeException) { }

            output.Add(row);
        }

        // Columns
        for (int i = 1; i < input.Count; i++)
        {
            List<T> row = [];
            try
            {
                int a = i;
                int b = input[0].Count - 1;
                while (true)
                {
                    row.Add(input[a][b]);
                    a++;
                    b--;
                }
            }
            catch (ArgumentOutOfRangeException) { }
            output.Add(row);
        }

        return output;
    }


    public static bool CheckX_MAS(this List<List<char>> map, (int i, int j) position)
    {
        try
        {
            // Check if the middle is A
            if (map[position.i][position.j] != 'A') return false;

            // Get diagonals, is in the try catch to avoid out of range
            var (i, j) = position;
            List<char> surroungingDiagonals = [
                map[i - 1][j - 1],
                map[i - 1][j + 1],
                map[i + 1][j - 1],
                map[i + 1][j + 1]];

            // Remove the case
            // M   S
            //   A
            // S   M
            if (surroungingDiagonals[0] == surroungingDiagonals[3]) return false;

            // Find if there is the same amount of M and S
            int sum = 0;
            foreach (var c in surroungingDiagonals)
            {
                sum += c switch
                {
                    'M' => +1,
                    'S' => -1,
                    _ => -100
                };
            }
            if (sum == 0) return true;

            return false;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }
}
