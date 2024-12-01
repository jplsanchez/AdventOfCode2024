record ProjectInfo
{
    public string SolutionFolderPath { get; init; }
    public string SolutionPath => Path.Combine(SolutionFolderPath, "advent_of_code_2024.sln");
    public string SourceFolderPath { get; init; }
    public string Day { get; init; }
    public string ProjectName => "AoC.Day" + Day;
    public string ProjectFolderPath { get; init; }
    public string ProjectPath => Path.Combine(ProjectFolderPath, $"{ProjectName}.csproj");

    public string ResourcesFolderPath => Path.Combine(ProjectFolderPath, "res");

    public string SamplePath => Path.Combine(ResourcesFolderPath, "sample01.txt");
    public string InputPath => Path.Combine(ResourcesFolderPath, "input01.txt");


    public ProjectInfo()
    {
        SolutionFolderPath = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.Parent!.FullName;
        SourceFolderPath = Path.Combine(SolutionFolderPath, "src");
        Day = ReadLineCurrentDay();
        ProjectFolderPath = Path.Combine(SourceFolderPath, ProjectName);
    }

    static string ReadLineCurrentDay()
    {
        int day;
        while (true)
        {
            Console.WriteLine("Enter your current advent of code day (1-25):");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out day) && day >= 1 && day <= 25) break;

            Console.WriteLine("Invalid input. Please enter a number between 1 and 25.\n");

        }

        return day.ToString("D2");
    }
}
