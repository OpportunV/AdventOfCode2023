using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AdventOfCode2023.Days;

public class Day15 : Day
{
    private readonly string[] _sequence;

    public Day15()
    {
        _sequence = GetInputRaw().Split(",");
    }

    public override string Part1()
    {
        return _sequence.Sum(GetHash).ToString();
    }

    public override string Part2()
    {
        var boxes = new Dictionary<int, OrderedDictionary<string, int>>();
        foreach (var step in _sequence)
        {
            var label = step.GetWords()[0];
            var boxId = GetHash(label);
            boxes.TryAdd(boxId, new OrderedDictionary<string, int>());
            var operation = step[label.Length];
            switch (operation)
            {
                case '-':
                    boxes[boxId].Remove(label);
                    break;
                case '=':
                    var power = step[^1] - '0';
                    boxes[boxId][label] = power;
                    break;
            }
        }

        var ans = 0;
        foreach (var (box, lenses) in boxes)
        {
            foreach (var (ind, power) in lenses.Values.Index())
            {
                ans += (box + 1) * (ind + 1) * power;
            }
        }

        return ans.ToString();
    }

    private static int GetHash(string data)
    {
        var hash = 0;
        foreach (var chr in data)
        {
            hash += chr;
            hash *= 17;
            hash %= 256;
        }

        return hash;
    }
}