using System.Collections.Generic;
using System.Linq;
using AdventOfCode2023.Models.Day19;
using Common.Extensions;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day19 : Day
{
    private readonly List<Part> _parts;
    private readonly Dictionary<string, Workflow> _workflows;
    private readonly Range<int> _defaultRange = new(1, 4000);

    private const string Start = "in";
    private const string Target = "A";
    private const string Reject = "R";

    public Day19()
    {
        var split = GetInputRaw().Split("\n\n");

        _workflows = split[0].Split("\n").Select(Workflow.Parse)
            .ToDictionary(workflow => workflow.Name, workflow => workflow);
        _parts = split[1].Split("\n").Select(Part.Parse).ToList();
    }

    public override string Part1()
    {
        var res = 0;
        foreach (var part in _parts)
        {
            var cur = Start;
            while (true)
            {
                var workflow = _workflows[cur];
                cur = workflow.GetDestination(part);

                if (cur == Target)
                {
                    res += part.Total;
                    break;
                }

                if (cur == Reject)
                {
                    break;
                }
            }
        }

        return res.ToString();
    }

    public override string Part2()
    {
        var ranges = _parts[0].Data.Keys.ToDictionary(key => key, _ => _defaultRange);
        return GetAcceptedCombinations(Start, ranges).ToString();
    }

    private long GetAcceptedCombinations(string cur, Dictionary<string, Range<int>> ranges)
    {
        switch (cur)
        {
            case Reject:
                return 0;
            case Target:
                return ranges.Values.Select(range => (long)(range.Length + 1)).Product();
        }

        var res = 0L;
        var workflow = _workflows[cur];
        foreach (var rule in workflow.Rules.OfType<ConditionalRule>())
        {
            var curRange = ranges[rule.Target];
            var (good, bad) = rule.SplitRange(curRange);

            var newRanges = ranges.ToDictionary();
            newRanges[rule.Target] = good;
            res += GetAcceptedCombinations(rule.Destination, newRanges);

            ranges[rule.Target] = bad;
        }

        res += GetAcceptedCombinations(workflow.Rules[^1].Destination, ranges);
        return res;
    }
}