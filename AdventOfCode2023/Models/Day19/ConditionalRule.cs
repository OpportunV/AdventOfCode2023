using Common.Models;

namespace AdventOfCode2023.Models.Day19;

public record ConditionalRule : Rule
{
    public bool Greater { get; }

    public string Target { get; }

    public int Value { get; }

    public ConditionalRule(string target, char op, int value, string destination) : base(destination)
    {
        Greater = op == '>';
        Target = target;
        Value = value;
    }

    public (Range<int> good, Range<int> bad) SplitRange(Range<int> range)
    {
        return Greater
            ? (new Range<int>(Value + 1, range.End), new Range<int>(range.Start, Value))
            : (new Range<int>(range.Start, Value - 1), new Range<int>(Value, range.End));
    }

    public override bool IsApplicableTo(Part part)
    {
        return Greater ? part.Data[Target] > Value : part.Data[Target] < Value;
    }
}