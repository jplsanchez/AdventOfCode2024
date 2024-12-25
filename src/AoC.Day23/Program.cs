using System.Diagnostics;
using Computer = string;

namespace AoC.Day23;

internal static class Program
{
    private static void Main(string[] args)
    {
        Stopwatch sw = new();
        sw.Start();

        // Read the file
#if (DEBUG)
        const bool IS_TEST = true;
#else
        const bool IS_TEST = false;
#endif

        StreamReader stream = IS_TEST switch
        {
            true => new("..\\..\\..\\res\\sample01.txt"),
            _ => new("..\\..\\..\\res\\input01.txt"),
        };




        Connections connections = stream.ReadToEnd()
                                        .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                                        .Aggregate(
                                            new List<Connection>(),
                                            (list, item) =>
                                            {
                                                list.Add((Connection)item);
                                                return list;
                                            });


        Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
        sw.Restart();


        // PART 1
        var sets = connections.FindSetsOf3Computers();


        long sum = 0;

        Console.WriteLine($"Part 1 sum: {sum}");
        Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
        sw.Restart();

        // PART 2
        sum = 0;


        Console.WriteLine($"Part 2 sum: {sum}");
        Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
        sw.Stop();
    }
}

public class Connections
{
    private readonly HashSet<Computer> _computers = [];
    private readonly HashSet<Connection> _connections = [];
    private HashSet<(Computer, Computer, Computer)> _setsOf3Computers = [];

    public Connections(List<Connection> connections)
    {
        foreach (var connection in connections)
        {
            // Add the firstComputer
            _computers.Add(connection.A);
            _computers.Add(connection.B);

            // Add the firstConnection
            _connections.Add(connection);
        }
    }

    public HashSet<(Computer, Computer, Computer)> FindSetsOf3Computers()
    {
        _setsOf3Computers = [];
        HashSet<(Connection, Connection)> setsOf3ComputersConns = [];

        HashSet<Connection> notVisited = [.. _connections];
        foreach (Computer firstComputer in _computers)
        {
            foreach (Connection firstConnection in notVisited.Where(conn => conn.Contains(firstComputer)))
            {
                notVisited.Remove(firstConnection);
                Computer secondComputer = firstConnection.Other(firstComputer);

                foreach (Connection secondConnection in notVisited.Where(conn => conn.Contains(secondComputer)))
                {
                    Computer thirdComputer = secondConnection.Other(secondComputer);
                    if (_connections.Contains((Connection)(firstComputer, thirdComputer)) && !setsOf3ComputersConns.Contains((firstConnection, secondConnection)))
                    {
                        Console.WriteLine($"    Found set: {firstComputer} {secondComputer} {thirdComputer}");
                        _setsOf3Computers.Add((firstComputer, secondComputer, thirdComputer));
                        setsOf3ComputersConns.Add((firstConnection, secondConnection));
                        setsOf3ComputersConns.Add((secondConnection, firstConnection));
                    }
                }
            }
        }

        return _setsOf3Computers;
    }

    public static implicit operator Connections(List<Connection> connections) => new(connections);
}

public struct Connection(Computer A, Computer B)
{
    public Computer A { get; set; } = A;
    public Computer B { get; set; } = B;

    public static explicit operator Connection(string connection) => new(connection.Split("-")[0], connection.Split("-")[1]);
    public static implicit operator Connection((Computer, Computer) connection) => new(connection.Item1, connection.Item2);
    public static bool operator ==(Connection C1, Connection C2) => (C1.A == C2.A && C1.B == C2.B) || (C1.A == C2.B && C1.B == C2.A);
    public static bool operator !=(Connection C1, Connection C2) => !(C1 == C2);
    public override readonly bool Equals(object? obj) => obj is Connection connection && connection == this;
    public readonly override int GetHashCode() => (A + B).ToCharArray().Select(c => (int)c).Sum();
    public readonly bool Contains(Computer computer) => A == computer || B == computer;
    public readonly Computer Other(Computer computer) => A == computer ? B : A;
    public readonly override string ToString() => $"({A}, {B})";
    public readonly (Computer, Computer) ToTuple() => (A, B);
    public readonly (Computer, Computer) ToTupleReversed() => (B, A);
}