using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day23 : Day
{
    private readonly Grid<char> _grid;
    private readonly GridPos2d _start;
    private readonly GridPos2d _end;

    private readonly Dictionary<char, GridPos2d> _slopes = new()
    {
        { '>', GridPos2d.Right },
        { 'v', GridPos2d.Down },
        { '<', GridPos2d.Left },
        { '^', GridPos2d.Up }
    };

    private const int Empty = '.';
    private const int Tree = '#';

    public Day23()
    {
        var grid = GetInput().Select(line => line.ToCharArray()).ToArray();
        _grid = new Grid<char>(grid);
        _start = _grid.Flatten().First(item => item.Value == Empty).Pos;
        _end = _grid.Flatten().Reverse().First(item => item.Value == Empty).Pos;
    }

    public override string Part1()
    {
        return GetLongestPath(true).ToString();
    }

    public override string Part2()
    {
        return GetLongestPath(false).ToString();
    }

    private int GetLongestPath(bool useSlope)
    {
        var graph = BuildGraph(useSlope);
        var toVisit = new PriorityQueue<(GridPos2d pos, HashSet<GridPos2d> path), int>();
        toVisit.Enqueue((_start, []), 0);
        var res = 0;
        while (toVisit.TryDequeue(out var cur, out var length))
        {
            var (pos, path) = cur;
            if (!path.Add(pos))
            {
                continue;
            }

            // some heuristics here to speed things up.
            if (-length * 4 < res)
            {
                continue;
            }

            if (pos == _end)
            {
                res = Math.Max(res, -length);
                continue;
            }

            foreach (var (nextPos, dist) in graph[pos])
            {
                toVisit.Enqueue((nextPos, [..path]), length - dist);
            }
        }

        return res;
    }

    private Dictionary<GridPos2d, List<(GridPos2d other, int dist)>> BuildGraph(bool useSlope)
    {
        var vertices = _grid.Flatten()
            .Where(item => _grid.AdjacentSide(item)
                .Count(second => second.Value != Tree) > 2)
            .Select(item => item.Pos)
            .ToHashSet();

        vertices.Add(_start);
        vertices.Add(_end);

        var edges = new Dictionary<GridPos2d, List<(GridPos2d other, int dist)>>();

        foreach (var vertex in vertices)
        {
            var toVisit = new PriorityQueue<GridPos2d, int>();
            toVisit.Enqueue(vertex, 0);
            var seen = new HashSet<GridPos2d>();
            while (toVisit.TryDequeue(out var pos, out var length))
            {
                if (!seen.Add(pos))
                {
                    continue;
                }

                if (pos != vertex && vertices.Contains(pos))
                {
                    edges.TryAdd(vertex, []);
                    edges[vertex].Add((pos, length));
                    continue;
                }

                if (useSlope && _slopes.TryGetValue(_grid[pos], out var dir))
                {
                    toVisit.Enqueue(pos + dir, length + 1);
                    continue;
                }

                foreach (var (nextVal, nextPos) in _grid.AdjacentSide(pos))
                {
                    if (nextVal != Tree)
                    {
                        toVisit.Enqueue(nextPos, length + 1);
                    }
                }
            }
        }

        return edges;
    }
}