namespace AoC.Day15;

public class Map
{
    public Position Robot { get; private set; }
    public HashSet<Position> Walls { get; private set; } = [];
    public HashSet<Position> Boxes { get; private set; } = [];

    private readonly int _width;
    private readonly int _height;

    public Map(List<List<char>> grid)
    {
        _height = grid.Count;
        _width = grid[0].Count;

        for (int y = 0; y < grid.Count; y++)
        {
            for (int x = 0; x < grid[y].Count; x++)
            {
                Tile current = (Tile)grid[y][x];

                if (current == Tile.Empty) continue;
                if (current == Tile.Wall) Walls.Add(new Position(x, y));
                if (current == Tile.Box) Boxes.Add(new Position(x, y));
                if (current == Tile.Robot) Robot = new Position(x, y);
            }
        };
    }

    public bool TryMove(Direction direction) => TryMoveRecursive(Robot, direction);

    private bool TryMoveRecursive(Position position, Direction direction)
    {
        Tile tile = position switch
        {
            Position p when p == Robot => Tile.Robot,
            Position p when Walls.Contains(p) => Tile.Wall,
            Position p when Boxes.Contains(p) => Tile.Box,
            _ => Tile.Empty
        };

        Position next = position + (Position)direction;

        // Walls doesn't move
        if (tile == Tile.Wall || Walls.Contains(next)) return false;

        // If the next position is empty, move
        if (next != Robot && !Boxes.Contains(next) && !Walls.Contains(next))
        {
            Move(position, next);
            return true;
        }

        // If the next position is a box, try to move the box
        if (Boxes.Contains(next))
        {
            if (TryMoveRecursive(next, direction))
            {
                Move(position, next);
                return true;
            }
        }

        return false;
    }

    private void Move(Position position, Position next)
    {
        if (position == Robot) Robot = next;

        if (Boxes.Contains(position))
        {
            Boxes.Remove(position);
            Boxes.Add(next);
        }
    }

    public void Print()
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Position p = new(x, y);
                if (p == Robot) Console.Write((char)Tile.Robot);
                else if (Walls.Contains(p)) Console.Write((char)Tile.Wall);
                else if (Boxes.Contains(p)) Console.Write((char)Tile.Box);
                else Console.Write((char)Tile.Empty);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}