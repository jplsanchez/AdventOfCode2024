namespace AoC.Day02.Types;

internal record IsSafeResult(bool Success, int? PositionNotSafe = null)
{
    static public implicit operator bool(IsSafeResult isSafe) => isSafe.Success;
}