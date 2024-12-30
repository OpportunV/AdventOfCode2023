using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Helpers;

namespace AdventOfCode2023.Days;

public class Day8 : Day
{
    private readonly string _instructions;
    private readonly Dictionary<string, (string left, string right)> _nodes = new();

    private const string Start = "AAA";
    private const string Target = "ZZZ";
    private const int Left = 'L';

    public Day8()
    {
        var split = GetInputRaw().Split("\n\n");
        _instructions = split[0];
        foreach (var line in split[1].Split("\n"))
        {
            var words = Regex.Matches(line, @"(\w+)").Select(match => match.Value).ToList();
            _nodes[words[0]] = (words[1], words[2]);
        }
    }

    public override string Part1()
    {
        var curStep = 0;
        var curNode = Start;
        while (curNode != Target)
        {
            curNode = _instructions[curStep % _instructions.Length] == Left
                ? _nodes[curNode].left
                : _nodes[curNode].right;
            curStep++;
        }

        return curStep.ToString();
    }

    public override string Part2()
    {
        var curStep = 0;
        var curNodes = _nodes.Keys.Where(node => node.EndsWith(Start[0])).ToArray();
        var cycles = Enumerable.Repeat(-1L, curNodes.Length).ToArray();
        while (!cycles.All(cycle => cycle > -1))
        {
            for (var i = 0; i < curNodes.Length; i++)
            {
                curNodes[i] = _instructions[curStep % _instructions.Length] == Left
                    ? _nodes[curNodes[i]].left
                    : _nodes[curNodes[i]].right;

                if (curNodes[i].EndsWith(Target[0]) && cycles[i] == -1)
                {
                    cycles[i] = curStep + 1;
                }
            }

            curStep++;
        }

        return cycles.Aggregate(cycles[0], Calculator.Lcm).ToString();
    }
}