namespace AoC.Day15;

public class WideMap
{
    public Position Robot { get; private set; }
    public HashSet<Position> Walls { get; private set; } = [];
    public HashSet<(Position, Position)> Boxes { get; private set; } = [];

    private readonly int _width;
    private readonly int _height;

    public WideMap(List<List<char>> grid)
    {
        List<List<char>> wideGrid = [];

        for (int y = 0; y < grid.Count; y++)
        {
            List<char> row = [];
            for (int x = 0; x < grid[y].Count; x++)
            {
                Tile current = (Tile)grid[y][x];

                if (current == Tile.Empty) row.AddRange(['.', '.']);
                if (current == Tile.Wall) row.AddRange(['#', '#']);
                if (current == Tile.Box) row.AddRange(['[', ']']);
                if (current == Tile.Robot) row.AddRange(['@', '.']);
            }
            wideGrid.Add(row);
        };


        _height = wideGrid.Count;
        _width = wideGrid[0].Count;

        for (int y = 0; y < wideGrid.Count; y++)
        {
            for (int x = 0; x < wideGrid[y].Count; x++)
            {
                Tile current = (Tile)wideGrid[y][x];

                if (current == Tile.Empty || current == Tile.BoxB) continue;
                if (current == Tile.Wall) Walls.Add(new Position(x, y));
                if (current == Tile.BoxA) Boxes.Add((new Position(x, y), new Position(x + 1, y)));
                if (current == Tile.Robot) Robot = new Position(x, y);
            }
        };


    }

    public bool CanMoveRobot(Direction direction)
    {
        Position next = Robot + (Position)direction;

        // Walls doesn't move
        if (Walls.Contains(next)) return false;

        // If the next position is empty, move
        if (!Boxes.Contains(next)) return true;

        // If the next position is a box, try to move the box
        if (Boxes.Contains(next))
        {
            return CanMoveBoxRecursive(next, direction);
        }

        return false;
    }

    private bool CanMoveBoxRecursive(Position position, Direction direction)
    {
        (Position, Position) box = Boxes.First(b => b.Item1 == position || b.Item2 == position);

        List<Position> next = direction switch
        {
            Direction.Up or Direction.Down => [box.Item1 + (Position)direction, box.Item2 + (Position)direction],
            Direction.Left => [box.Item1 + (Position)direction],
            Direction.Right => [box.Item2 + (Position)direction],
            _ => []
        };

        // Walls doesn't move
        if (next.Any(Walls.Contains)) return false;

        // If the next position is empty, move
        if (next.All(n => !Boxes.Contains(n))) return true;

        // If the next position is a box or multiple boxes, try to move the boxes

        bool canMove = true;
        foreach (var n in next)
        {
            // Ignore empty spaces
            if (!Boxes.Contains(n)) continue;

            if (!CanMoveBoxRecursive(n, direction))
            {
                canMove = false;
            }

        }
        return canMove;
    }

    public void MoveRobot(Direction direction)
    {
        Position next = Robot + (Position)direction;

        // If the next position is a box, move the box
        if (Boxes.Contains(next)) MoveBoxRecursive(next, direction);

        // If the next position is empty, move
        Robot = next;
    }

    private void MoveBoxRecursive(Position position, Direction direction)
    {
        (Position, Position) box = Boxes.First(b => b.Item1 == position || b.Item2 == position);
        (Position, Position) possibleNext = (box.Item1 + (Position)direction, box.Item2 + (Position)direction);

        List<Position> next = direction switch
        {
            Direction.Up or Direction.Down => Boxes.FirstOrDefault(b => b.Item1 == position) != Boxes.FirstOrDefault(b => b.Item2 == position)
                                                            ? [possibleNext.Item1, possibleNext.Item2]
                                                            : [possibleNext.Item1],
            Direction.Left => [possibleNext.Item1],
            Direction.Right => [possibleNext.Item2],
            _ => []
        };

        // If the next position is empty, move
        if (next.All(n => !Boxes.Contains(n)))
        {
            MoveBox(position, direction);
            return;
        }

        // If the next position is a box or multiple boxes, move the boxes
        foreach (var b in next)
        {
            if (Boxes.Contains(b)) MoveBoxRecursive(b, direction);
        }

        MoveBox(position, direction);
    }


    private void MoveBox(Position position, Direction direction)
    {
        var box = Boxes.FirstOrDefault(b => b.Item1 == position || b.Item2 == position);
        var next = (box.Item1 + (Position)direction, box.Item2 + (Position)direction);

        if (Boxes.Contains(position))
        {
            Boxes.Remove(box);
            Boxes.Add(next);
        }
    }

    public void Print(char icon = (char)Tile.Robot)
    {
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Position p = new(x, y);
                if (p == Robot) Console.Write(icon);
                else if (Walls.Contains(p)) Console.Write((char)Tile.Wall);
                else if (Boxes.Contains(p))
                {
                    var box = Boxes.First(b => b.Item1 == p || b.Item2 == p);
                    if (box.Item1 == p) Console.Write((char)Tile.BoxA);
                    else Console.Write((char)Tile.BoxB);
                }
                else Console.Write((char)Tile.Empty);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}