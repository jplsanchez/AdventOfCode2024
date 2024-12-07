
//// PART 2
//sum = 0;


//Console.WriteLine($"Part 2 sum: {sum}");
//Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
//sw.Stop();

internal class Calculator(long Result, List<long> Numbers)
{
    public bool PossibleSolutionExists() => CheckRecursively(0, 0, Operator.Add) || CheckRecursively(0, 1, Operator.Multiply);

    private bool CheckRecursively(int i, long acumulated, Operator operation)
    {
        if (i == Numbers.Count) return acumulated == Result;

        acumulated = operation switch
        {
            Operator.Add => acumulated + Numbers[i],
            Operator.Multiply => acumulated * Numbers[i],
            _ => throw new NotImplementedException()
        };

        return CheckRecursively(i + 1, acumulated, Operator.Add) ||
               CheckRecursively(i + 1, acumulated, Operator.Multiply);
    }

    private enum Operator
    {
        Add,
        Multiply
    }
}

internal class CalculatorExpanded(long Result, List<long> Numbers)
{
    public bool PossibleSolutionExists() => CheckRecursively(0, 0, Operator.Add) || CheckRecursively(0, 1, Operator.Multiply);

    private bool CheckRecursively(int i, long acumulated, Operator operation)
    {
        if (i == Numbers.Count) return acumulated == Result;

        acumulated = operation switch
        {
            Operator.Add => acumulated + Numbers[i],
            Operator.Multiply => acumulated * Numbers[i],
            Operator.Concatenate => long.Parse(acumulated.ToString() + Numbers[i].ToString()),
            _ => throw new NotImplementedException()
        };

        return CheckRecursively(i + 1, acumulated, Operator.Add) ||
               CheckRecursively(i + 1, acumulated, Operator.Multiply) ||
               CheckRecursively(i + 1, acumulated, Operator.Concatenate);
    }

    private enum Operator
    {
        Add,
        Multiply,
        Concatenate
    }
}