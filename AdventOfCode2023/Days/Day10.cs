using System;
using System.Collections.Generic;
using System.Linq;
using Common.Helpers;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day10 : Day
{
    private readonly Dictionary<char, HashSet<GridPos2d>> _pipes = new()
    {
        { '|', [GridPos2d.Down, GridPos2d.Up] },
        { '-', [GridPos2d.Left, GridPos2d.Right] },
        { 'L', [GridPos2d.Up, GridPos2d.Right] },
        { 'J', [GridPos2d.Up, GridPos2d.Left] },
        { '7', [GridPos2d.Down, GridPos2d.Left] },
        { 'F', [GridPos2d.Down, GridPos2d.Right] },
        { '.', [] },
        { 'S', [..Directions2d.Side] }
    };

    private readonly HashSet<char> _cornerPipes = ['L', 'J', '7', 'F'];

    private readonly Grid<char> _grid;

    public Day10()
    {
        var grid = GetInput().Select(line => line.ToCharArray()).ToArray();
        _grid = new Grid<char>(grid);
    }

    public override string Part1()
    {
        var start = _grid.Flatten().First(item => item.Value == 'S').Pos;
        var toVisit = new Queue<(GridPos2d pos, int dist)>();
        var seen = new Dictionary<GridPos2d, int>();
        toVisit.Enqueue((start, 0));

        while (toVisit.TryDequeue(out var cur))
        {
            var (pos, dist) = cur;
            var pipe = _grid[pos];
            seen.TryAdd(pos, int.MaxValue);
            seen[pos] = Math.Min(seen[pos], dist);

            foreach (var next in ConnectedPipes(pos, pipe))
            {
                if (!seen.ContainsKey(next))
                {
                    toVisit.Enqueue((next, dist + 1));
                }
            }
        }

        return seen.Values.Max().ToString();
    }

    public override string Part2()
    {
        var start = _grid.Flatten().First(item => item.Value == 'S');
        var seen = new HashSet<GridPos2d> { start.Pos };

        var connected = ConnectedPipes(start.Pos, start.Value).ToList();
        var direction = connected.First();
        var toVisit = new Queue<GridPos2d>();
        var corners = new List<GridPos2d>();
        toVisit.Enqueue(direction);

        if (connected[0] != -connected[1])
        {
            corners.Add(start.Pos);
        }

        while (toVisit.TryDequeue(out var pos))
        {
            var pipe = _grid[pos];
            if (!seen.Add(pos))
            {
                continue;
            }

            if (_cornerPipes.Contains(_grid[pos]))
            {
                corners.Add(pos);
            }

            foreach (var next in ConnectedPipes(pos, pipe))
            {
                if (!seen.Contains(next))
                {
                    toVisit.Enqueue(next);
                }
            }
        }

        // Fancy theorems go here.
        // https://11011110.github.io/blog/2021/04/17/picks-shoelaces.html
        var area = 0;
        foreach (var (prev, cur) in corners.Zip(corners[1..].Append(corners[0])))
        {
            area += (cur.Row - prev.Row) * (cur.Col + prev.Col) / 2;
        }

        var interior = area - seen.Count / 2 + 1;

        return interior.ToString();
    }

    private IEnumerable<GridPos2d> ConnectedPipes(GridPos2d pos, char pipe)
    {
        foreach (var dir in Directions2d.Side)
        {
            var nextPos = pos + dir;
            if (!_grid.Contains(nextPos))
            {
                continue;
            }

            if (_pipes[pipe].Contains(dir) && _pipes[_grid[nextPos]].Contains(-dir))
            {
                yield return nextPos;
            }
        }
    }
}