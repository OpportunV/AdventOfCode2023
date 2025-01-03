using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day16 : Day
{
    private readonly Grid<char> _grid;
    private readonly GridPos2d _startPos = GridPos2d.Zero;
    private readonly GridPos2d _startDir = GridPos2d.Right;

    private readonly Dictionary<char, Dictionary<GridPos2d, GridPos2d>> _mirroring = new()
    {
        [RMirror] = new Dictionary<GridPos2d, GridPos2d>
        {
            { GridPos2d.Right, GridPos2d.Up },
            { GridPos2d.Down, GridPos2d.Left },
            { GridPos2d.Left, GridPos2d.Down },
            { GridPos2d.Up, GridPos2d.Right }
        },
        [LMirror] = new Dictionary<GridPos2d, GridPos2d>
        {
            { GridPos2d.Right, GridPos2d.Down },
            { GridPos2d.Down, GridPos2d.Right },
            { GridPos2d.Left, GridPos2d.Up },
            { GridPos2d.Up, GridPos2d.Left }
        }
    };

    private const char Empty = '.';
    private const char HSplitter = '-';
    private const char VSplitter = '|';
    private const char RMirror = '/';
    private const char LMirror = '\\';

    public Day16()
    {
        var grid = GetInput().Select(line => line.ToCharArray()).ToArray();
        _grid = new Grid<char>(grid);
    }

    public override string Part1()
    {
        return CalculateEnergized(_startPos, _startDir).ToString();
    }

    public override string Part2()
    {
        var ans = 0;
        for (var i = 0; i < _grid.Cols; i++)
        {
            ans = Math.Max(ans, CalculateEnergized(new GridPos2d(0, i), GridPos2d.Down));
            ans = Math.Max(ans, CalculateEnergized(new GridPos2d(_grid.Rows - 1, i), GridPos2d.Up));
        }

        for (var i = 0; i < _grid.Rows; i++)
        {
            ans = Math.Max(ans, CalculateEnergized(new GridPos2d(i, 0), GridPos2d.Right));
            ans = Math.Max(ans, CalculateEnergized(new GridPos2d(i, _grid.Cols - 1), GridPos2d.Left));
        }

        return ans.ToString();
    }

    private int CalculateEnergized(GridPos2d startPos, GridPos2d startDir)
    {
        var toVisit = new Queue<(GridPos2d pos, GridPos2d dir)>();
        var seen = new HashSet<(GridPos2d pos, GridPos2d dir)>();
        toVisit.Enqueue((startPos, startDir));
        while (toVisit.TryDequeue(out var cur))
        {
            var (pos, dir) = cur;
            if (!_grid.Contains(pos))
            {
                continue;
            }

            if (!seen.Add((pos, dir)))
            {
                continue;
            }

            var value = _grid[pos];
            switch (value)
            {
                case Empty:
                case HSplitter when dir == GridPos2d.Right || dir == GridPos2d.Left:
                case VSplitter when dir == GridPos2d.Up || dir == GridPos2d.Down:
                    toVisit.Enqueue((pos + dir, dir));
                    break;
                case HSplitter:
                    toVisit.Enqueue((pos + GridPos2d.Left, GridPos2d.Left));
                    toVisit.Enqueue((pos + GridPos2d.Right, GridPos2d.Right));
                    break;
                case VSplitter:
                    toVisit.Enqueue((pos + GridPos2d.Up, GridPos2d.Up));
                    toVisit.Enqueue((pos + GridPos2d.Down, GridPos2d.Down));
                    break;
                default:
                    toVisit.Enqueue((pos + _mirroring[value][dir], _mirroring[value][dir]));
                    break;
            }
        }

        return seen.Select(pair => pair.pos).ToHashSet().Count;
    }
}