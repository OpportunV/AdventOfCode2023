using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AdventOfCode2023.Days;

public class Day12 : Day
{
    private readonly List<string> _springs = [];
    private readonly List<List<int>> _sequences = [];
    private readonly Dictionary<(string spring, string sequence), long> _cache = new();
    private const char Operational = '.';
    private const char Broken = '#';
    private const char Unknown = '?';
    private const int Repeats = 5;

    public Day12()
    {
        foreach (var line in GetInput())
        {
            var split = line.Split(' ');
            _springs.Add(split[0]);
            _sequences.Add(split[1].GetNumbers<int>());
        }
    }

    public override string Part1()
    {
        return _springs.Zip(_sequences).Sum(pair => GetCombinations(pair.First, pair.Second)).ToString();
    }

    public override string Part2()
    {
        var ans = 0L;
        foreach (var (spring, sequence) in _springs.Zip(_sequences))
        {
            var newSpring = string.Join(Unknown, Enumerable.Repeat(spring, Repeats));
            var newSequence = Enumerable.Range(0, Repeats).SelectMany(_ => sequence).ToList();
            ans += GetCombinations(newSpring, newSequence);
        }

        return ans.ToString();
    }

    private long GetCombinations(string spring, List<int> sequence)
    {
        var strSeq = string.Join(",", sequence);
        if (_cache.ContainsKey((spring, strSeq)))
        {
            return _cache[(spring, strSeq)];
        }

        if (sequence.Count == 0)
        {
            return spring.Any(chr => chr == Broken) ? 0 : 1;
        }

        if (spring.Length == 0)
        {
            return sequence.Count == 0 ? 1 : 0;
        }

        var cur = sequence[0];
        var ans = 0L;

        if (spring[0] != Broken)
        {
            ans += GetCombinations(spring[1..], sequence);
        }

        if (spring[0] != Operational)
        {
            if (cur <= spring.Length && spring[..cur].All(chr => chr != Operational))
            {
                if (cur == spring.Length)
                {
                    ans += GetCombinations(string.Empty, sequence[1..]);
                }
                else if (spring[cur] != Broken)
                {
                    ans += GetCombinations(spring[(cur + 1)..], sequence[1..]);
                }
            }
        }

        _cache[(spring, strSeq)] = ans;
        return ans;
    }
}