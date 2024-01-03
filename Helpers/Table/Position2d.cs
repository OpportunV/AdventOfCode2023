namespace Helpers.Table;

public struct Position2d(int col, int row)
{
    public int Col { get; set; } = col;
    public int Row { get; set; } = row;
}