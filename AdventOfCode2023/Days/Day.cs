namespace AdventOfCode2023.Days;

public abstract class Day
{
    private string InputPath => Path.Combine("Input", $"{GetType().Name}.txt");

    public abstract string Part1();

    public abstract string Part2();

    protected string[] GetInput()
    {
        return File.ReadAllLines(InputPath);
    }
}