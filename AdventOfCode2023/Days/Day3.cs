using AdventOfCode2023.Models.Day3;


namespace AdventOfCode2023.Days;

public class Day3 : Day
{
    private readonly EngineSchematics _engine;

    public Day3()
    {
        var input = GetInput();
        _engine = new EngineSchematics(input);
    }

    public override string Part1()
    {
        return _engine.PartNumbers.Sum().ToString();
    }

    public override string Part2()
    {
        return _engine.GearValues.Sum().ToString();
    }
}