namespace AdventOfCode2023.Models.Day19;

public record DefaultRule(string Destination) : Rule(Destination)
{
    public override bool IsApplicableTo(Part part)
    {
        return true;
    }
}