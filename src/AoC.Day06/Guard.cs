global using Position = (int x, int y);

internal enum Orientation
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}

internal class Guard(Position position, Orientation orientation)
{
    public Position Position { get; set; } = position;
    public Orientation Orientation { get; set; } = orientation;

    public static bool operator ==(Guard left, Guard right) => left.Position.x == right.Position.x && left.Position.y == right.Position.y && left.Orientation == right.Orientation;
    public static bool operator !=(Guard left, Guard right) => !(left == right);
    public override bool Equals(object? obj) => obj is Guard guard && this == guard;
    public override int GetHashCode() => base.GetHashCode();

    public Position NextStep() => Orientation switch
    {
        Orientation.UP => (Position.x, Position.y - 1),
        Orientation.RIGHT => (Position.x + 1, Position.y),
        Orientation.DOWN => (Position.x, Position.y + 1),
        Orientation.LEFT => (Position.x - 1, Position.y),
        _ => throw new ArgumentOutOfRangeException(),
    };

    public void TurnRight() => Orientation = Orientation switch
    {
        Orientation.UP => Orientation.RIGHT,
        Orientation.RIGHT => Orientation.DOWN,
        Orientation.DOWN => Orientation.LEFT,
        Orientation.LEFT => Orientation.UP,
        _ => throw new ArgumentOutOfRangeException(),
    };

    public void Move() => Position = NextStep();

    public Guard DeepCopy() => new(Position, Orientation);
}


internal readonly record struct PositionLog(Position Position, Orientation Orientation)
{
    public override int GetHashCode() => HashCode.Combine(Position.x, Position.y, Orientation);

    public static PositionLog LogFrom(Guard guard) => new(guard.Position, guard.Orientation);
};