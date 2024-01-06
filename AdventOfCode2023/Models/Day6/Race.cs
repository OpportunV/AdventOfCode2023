namespace AdventOfCode2023.Models.Day6;

public struct Race
{
    public long Time { get; }

    public long Distance { get; }

    public Race(long time, long distance)
    {
        Time = time;
        Distance = distance;
    }
}