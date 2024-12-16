using System.Diagnostics;

Stopwatch sw = new();
sw.Start();

// Read the file
const bool IS_TEST = true;
int TEST_NUMBER = 1;

StreamReader stream = IS_TEST switch
{
    true when TEST_NUMBER == 2 => new("..\\..\\..\\res\\sample02.txt"),
    true => new("..\\..\\..\\res\\sample01.txt"),
    _ => new("..\\..\\..\\res\\input01.txt"),
};

string? line;

Map map = [];

while ((line = stream.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) continue;
    map.Add([.. line.ToCharArray()]);
}

map.Setup();

Console.WriteLine($"Map loaded in {sw.ElapsedMilliseconds}ms");
sw.Restart();


// PART 1
long sum = map.CalculateSmallestWeight();

Console.WriteLine($"Part 1 sum: {sum}");
Console.WriteLine($"Part 1 ran in {sw.ElapsedMilliseconds}ms");
sw.Restart();

// PART 2
sum = 0;
sum = map.SumAllPathsToEnd();
map.PrintWithPaths();

Console.WriteLine($"Part 2 sum: {sum}");
Console.WriteLine($"Part 2 ran in {sw.ElapsedMilliseconds}ms");
sw.Stop();


class Map : List<List<char>>
{
    public Position Start { get; private set; }
    public Position End { get; private set; }

    public const Direction START_DIR = Direction.East;

    Dictionary<Position, (long Weight, Position LastPosition, Direction LastDirection)> _weights = [];
    Dictionary<Position, List<Position>> _pathsHistory = [];
    HashSet<Position> _visited = [];
    HashSet<Position> _unvisited = [];

    public void Setup()
    {
        for (int i = 0; i < Count; i++)
        {
            for (int j = 0; j < this[i].Count; j++)
            {
                char c = this[i][j];

                if (c != '#') _unvisited.Add(new(j, i));

                _pathsHistory.Add(new(j, i), []); // add for pt2

                switch (c)
                {
                    case 'S':
                        Start = new(j, i);
                        _weights.Add(Start, (0, Start, START_DIR));
                        break;
                    case 'E':
                        End = new(j, i);
                        _weights.Add(End, (long.MaxValue, default, default));
                        break;
                    case '.':
                        _weights.Add(new(j, i), (long.MaxValue, default, default));
                        break;
                }
            }
        }
    }

    public long CalculateSmallestWeight()
    {
        Position current = Start;

        while (_unvisited.Count != 1/*current != End*/) //changed position pt2
        {
            _visited.Add(current);
            _unvisited.Remove(current);
            Direction currentDirection = _weights[current].LastDirection;

            foreach (Direction dir in Enum.GetValues<Direction>())
            {
                Position next = current + (Position)dir;

                if (!IsWithinBounds(next)) continue;
                if (this[next] == '#') continue;

                _pathsHistory[current].Add(next); // add for pt2

                long updatedWeight = _weights[current].Weight + CalculateWeight(current, next, currentDirection);
                if (updatedWeight < _weights[next].Weight) _weights[next] = (Weight: updatedWeight, LastPosition: current, LastDirection: (Direction)(next - current));
            }
            current = _weights.Where(x => !_visited.Contains(x.Key)).OrderBy(w => w.Value.Weight).FirstOrDefault().Key;
        }

        return _weights[End].Weight;
    }

    private long CalculateWeight(Position start, Position end, Direction direction)
    {
        return (end - start) switch
        {
            Position p when (Direction)p == direction => 1, // Same Direction
            Position p when (Direction)p == (Direction)((int)direction + 2 % 4) => 1 + 1000 + 1000, // Opposite Direction
            _ => 1 + 1000 // 90 degree turn
        };
    }


    public long SumAllPathsToEnd()
    {
        HashSet<Position> _onThePath = [];

        _onThePath.Add(End);
        AddOriginsToPath(End);

        return _onThePath.Count;

        void AddOriginsToPath(Position current)
        {
            if (current == Start) return;

            List<Position> origins = _pathsHistory[current];
            foreach (Position p in origins)
            {
                if (_onThePath.Contains(p)) continue;

                _onThePath.Add(p);
                AddOriginsToPath(p);
            }
        }
    }

    public HashSet<Position> AllPathsToEnd()
    {
        HashSet<Position> _onThePath = [];

        _onThePath.Add(End);
        AddOriginsToPath(End);

        return _onThePath;

        void AddOriginsToPath(Position current)
        {
            if (current == Start) return;

            List<Position> origins = _pathsHistory[current];
            foreach (Position p in origins)
            {
                if (_onThePath.Contains(p)) continue;

                _onThePath.Add(p);
                AddOriginsToPath(p);
            }
        }
    }

    bool IsWithinBounds(Position p) => p.X >= 0 && p.X < this[0].Count && p.Y >= 0 && p.Y < Count;

    public void PrintWithPaths()
    {
        HashSet<Position> paths = AllPathsToEnd();
        for (int i = 0; i < Count; i++)
        {
            for (int j = 0; j < this[i].Count; j++)
            {
                char c = this[i][j];
                Position p = new(j, i);

                if (paths.Contains(p)) Console.Write('O');
                else Console.Write(c);


            }
            Console.WriteLine();
        }
    }

    char this[Position p] => this[p.Y][p.X];
}

public struct Position(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);

    public static explicit operator Direction(Position p) => p switch
    {
        (0, -1) => Direction.North,
        (1, 0) => Direction.East,
        (0, 1) => Direction.South,
        (-1, 0) => Direction.West,
        _ => throw new InvalidCastException()
    };

    public static explicit operator Position(Direction d) => d switch
    {
        Direction.North => new(0, -1),
        Direction.East => new(1, 0),
        Direction.South => new(0, 1),
        Direction.West => new(-1, 0),
        _ => throw new InvalidCastException()
    };

    public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
    public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
    public static bool operator ==(Position a, Position b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Position a, Position b) => !(a == b);
    public override readonly bool Equals(object? obj) => obj is Position p && p == this;
    public override readonly int GetHashCode() => (X, Y).GetHashCode();

}

public enum Direction
{
    North,
    East,
    South,
    West
}