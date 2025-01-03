namespace AdventOfCode2023.Models.Day19;

public abstract record Rule(string Destination)
{
    public abstract bool IsApplicableTo(Part part);
}