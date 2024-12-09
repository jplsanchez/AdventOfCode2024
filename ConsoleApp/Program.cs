using Microsoft.Extensions.Configuration;
using System.Diagnostics;

Console.WriteLine("###############################################");
Console.WriteLine("### Advent of Code 2024  - Folder Generator ###");
Console.WriteLine("###############################################\n");

ProjectInfo info = new();

// Create src folder
if (!Directory.Exists(info.SourceFolderPath))
{
    Directory.CreateDirectory(info.SourceFolderPath);
    Console.WriteLine($"Directory Created Successfully: {info.SourceFolderPath}");
}
else Console.WriteLine($"Directory already exists: {info.SourceFolderPath}");


// Create project and add to solution
if (!Directory.Exists(info.ProjectFolderPath))
{
    Helper.RunCmd($"dotnet new console -n {info.ProjectName}", info.SourceFolderPath);
    Console.WriteLine($"Project created: {info.ProjectName}");

    Helper.RunCmd($"dotnet sln {info.SolutionPath} add {info.ProjectPath}", info.ProjectFolderPath);
    Console.WriteLine($"Project added to solution: {info.ProjectName}");

    // Change Program.cs to the scaffold
    File.WriteAllText(Path.Combine(info.ProjectFolderPath, "Program.cs"), File.ReadAllText(Path.Combine(info.ConsoleProjectFolderPath, "scaffoldProgram.cs.txt")));
}
else Console.WriteLine($"Project already exists: {info.ProjectName}");

// add input files to project folder
if (!Directory.Exists(info.ResourcesFolderPath))
{
    Directory.CreateDirectory(info.ResourcesFolderPath);
    Console.WriteLine($"Directory Created Successfully: {info.ResourcesFolderPath}");
}
else Console.WriteLine($"Directory already exists: {info.ResourcesFolderPath}");

// add sample file to project folder
if (!File.Exists(info.SamplePath))
{
    File.Create(info.SamplePath).Close();
    Console.WriteLine($"File Created Successfully: {info.SamplePath}");
}
else Console.WriteLine($"File already exists: {info.SamplePath}");

// add input file to project folder
if (!File.Exists(info.InputPath))
{
    File.Create(info.InputPath).Close();
    Console.WriteLine($"File Created Successfully: {info.InputPath}");

    using HttpClient client = new()
    {
        BaseAddress = new($"https://adventofcode.com/2024/day/{int.Parse(info.Day)}/input"),
    };

    // Add session cookie from user secrets
    var conf = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
    client.DefaultRequestHeaders.Add("Cookie", $"session={conf["Session"]}");

    using HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
    if (!response.IsSuccessStatusCode) Helper.Finish("Failed to load input file");

    using Stream stream = await response.Content.ReadAsStreamAsync();
    string input = await new StreamReader(stream).ReadToEndAsync();
    await File.WriteAllTextAsync(info.InputPath, input);
    Console.WriteLine("Input file loaded successfully");
}
else Console.WriteLine($"File already exists: {info.InputPath}");



Helper.Finish("Program finished successfully");



public static class Helper
{
    public static void RunCmd(string cmd, string path)
    {
        Process p = new();

        // Redirect the output stream of the child process.
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.WorkingDirectory = path;
        p.StartInfo.FileName = "cmd";
        p.StartInfo.Arguments = "/c " + cmd;
        p.Start();
        // Do not wait for the child process to exit before
        // reading to the end of its redirected stream.
        // p.WaitForExit();
        // Read the output stream first and then wait.
        string output = p.StandardOutput.ReadToEnd();
        Console.WriteLine(output);
    }

    public static void Finish(string? message = null)
    {
        if (message is not null) Console.WriteLine(message);
        Environment.Exit(1);
    }

}

