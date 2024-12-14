public class Robot
{
    public Position Position { get; set; }
    public Speed Speed { get; set; }
    public Robot(Position position, Speed speed)
    {
        Position = position;
        Speed = speed;
    }
    public void Move(long xLenght, long yLength, long times = 1)
    {
        long x = (Position.X + (times * Speed.X) + xLenght * times) % xLenght;
        long y = (Position.Y + (times * Speed.Y) + yLength * times) % yLength;

        Position = new(x, y);
    }
}
