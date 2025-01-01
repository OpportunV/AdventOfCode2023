using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AdventOfCode2023.Days;

public class Day25 : Day
{
    private readonly Dictionary<string, HashSet<string>> _graph = new();
    private readonly HashSet<List<string>> _edges = [];

    public Day25()
    {
        var lines = GetInput();
        foreach (var line in lines)
        {
            var wires = line.GetWords().ToArray();
            var cur = wires[0];
            _graph.TryAdd(cur, []);
            foreach (var wire in wires[1..])
            {
                _graph.TryAdd(wire, []);
                _graph[cur].Add(wire);
                _graph[wire].Add(cur);
                _edges.Add(new[] { cur, wire }.Order().ToList());
            }
        }
    }

    public override string Part1()
    {
        int result;
        string start;
        var nodes = _graph.Keys.ToList();
        do
        {
            start = nodes[Random.Shared.Next(nodes.Count)];
        } while (GetCuts(start, out result) != 3);

        return result.ToString();
    }

    public override string Part2()
    {
        return "2";
    }

    private int GetCuts(string start, out int result)
    {
        var cuts = 0;
        var first = GetFurthestNode(start);
        var second = GetFurthestNode(first);

        var toVisit = new Queue<string>();
        var seen = new HashSet<string>();
        toVisit.Enqueue(first);
        toVisit.Enqueue(second);
        var distances = new Dictionary<string, double>
        {
            [first] = 0,
            [second] = 1
        };

        while (toVisit.TryDequeue(out var cur))
        {
            if (!seen.Add(cur))
            {
                continue;
            }

            if (cur != first && cur != second)
            {
                distances[cur] = _graph[cur].Average(node => distances.GetValueOrDefault(node, 0.5));
            }

            foreach (var nextNode in _graph[cur])
            {
                if (seen.Contains(nextNode))
                {
                    continue;
                }

                toVisit.Enqueue(nextNode);
            }
        }

        foreach (var edge in _edges)
        {
            if (Math.Sign(distances[edge[0]] - 0.5) != Math.Sign(distances[edge[1]] - 0.5))
            {
                cuts++;
            }
        }

        result = distances.Values.Count(value => value > 0.5) * distances.Values.Count(value => value < 0.5);

        return cuts;
    }

    private string GetFurthestNode(string node)
    {
        var toVisit = new Queue<(string node, int dist)>();
        var seen = new HashSet<string>();
        var distances = new Dictionary<string, int>();
        toVisit.Enqueue((node, 0));

        while (toVisit.TryDequeue(out var cur))
        {
            var (curNode, dist) = cur;
            if (!seen.Add(curNode))
            {
                continue;
            }

            distances.TryAdd(curNode, 0);
            distances[curNode] = Math.Max(distances[curNode], dist);
            foreach (var nextNode in _graph[curNode])
            {
                if (seen.Contains(nextNode))
                {
                    continue;
                }

                toVisit.Enqueue((nextNode, dist + 1));
            }
        }

        return distances.MaxBy(pair => pair.Value).Key;
    }
}