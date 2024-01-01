namespace AdventOfCode2023.Models.Day2;

public record CubeSet(int Red, int Green, int Blue)
{
    public int Power => Red * Green * Blue;

    public static bool operator <=(CubeSet first, CubeSet second)
    {
        return first.Red <= second.Red && first.Green <= second.Green && first.Blue <= second.Blue;
    }

    public static bool operator >=(CubeSet first, CubeSet second)
    {
        return first.Red >= second.Red && first.Green >= second.Green && first.Blue >= second.Blue;
    }
}