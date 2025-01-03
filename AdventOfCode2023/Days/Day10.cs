using System.Collections.Generic;
using System.Linq;
using Common.Helpers;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day10 : Day
{
    private const int Start = 'S';

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
        var path = GetPipePath(out _);
        return (path.Count / 2).ToString();
    }

    public override string Part2()
    {
        var path = GetPipePath(out var corners);

        var area = Calculator.PolygonArea(corners);
        var interior = Calculator.PolygonInteriorPoints(area, path.Count);

        return interior.ToString();
    }

    private HashSet<GridPos2d> GetPipePath(out List<GridPos2d> corners)
    {
        var start = _grid.Flatten().First(item => item.Value == Start);
        var seen = new HashSet<GridPos2d> { start.Pos };

        var connected = ConnectedPipes(start.Pos, start.Value).ToList();
        var direction = connected.First();
        var toVisit = new Queue<GridPos2d>();
        corners = [];
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

        return seen;
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