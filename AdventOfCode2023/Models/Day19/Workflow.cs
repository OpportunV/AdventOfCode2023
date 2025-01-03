using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023.Models.Day19;

public record Workflow(string Name, List<Rule> Rules)
{
    public string GetDestination(Part part)
    {
        return Rules.First(rule => rule.IsApplicableTo(part)).Destination;
    }

    public static Workflow Parse(string input)
    {
        var name = input[..input.IndexOf("{", StringComparison.InvariantCulture)];
        var rulesStr = input[(name.Length + 1)..^1];
        var split = rulesStr.Split(",");
        var rules = split.Select(RuleFactory.FromString).ToList();

        return new Workflow(name, rules);
    }
}