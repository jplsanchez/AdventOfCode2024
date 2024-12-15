namespace AoC.Day15;

public struct Position(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;


    public static explicit operator Position(Direction direction) => direction switch
    {
        Direction.Up => new Position(0, -1),
        Direction.Down => new Position(0, 1),
        Direction.Left => new Position(-1, 0),
        Direction.Right => new Position(1, 0),
        _ => new Position(0, 0)
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
    Up = '^',
    Down = 'v',
    Left = '<',
    Right = '>'
}

public enum Tile
{
    Wall = '#',
    Box = 'O',
    Robot = '@',
    Empty = '.',
    BoxA = '[', // Part 2
    BoxB = ']' // Part 2

}


