using Common.Extensions;

namespace AdventOfCode2023.Models.Day19;

public static class RuleFactory
{
    public static Rule FromString(string input)
    {
        if (!input.Contains(":"))
        {
            return new DefaultRule(input);
        }

        var target = input[0];
        var op = input[1];
        var value = input.GetNumbers<int>()[0];
        var destination = input.Split(":")[1];
        return new ConditionalRule(target.ToString(), op, value, destination);
    }
}