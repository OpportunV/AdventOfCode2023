using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day21 : Day
{
    private readonly Grid<char> _grid;
    private readonly GridPos2d _start;

    private const char Start = 'S';
    private const char Rock = '#';
    private const int StepsPart1 = 64;
    private const int StepsPart2 = 26501365;

    public Day21()
    {
        var grid = GetInput().Select(line => line.ToCharArray()).ToArray();
        _grid = new Grid<char>(grid);
        _start = _grid.Flatten().First(item => item.Value == Start).Pos;
    }

    public override string Part1()
    {
        return CalculatePossiblePlots(_start, StepsPart1).ToString();
    }

    public override string Part2()
    {
        // yi = ax^2 + bx + c, yi = possible plots, x - amount of full grids possible to travel.
        var y = Enumerable.Range(0, 3)
            .Select(i => CalculatePossiblePlots(_start, _start.Col + i * _grid.Cols))
            .ToList();
        var c = y[0];
        var a = (y[2] - 2 * y[1] + y[0]) / 2;
        var b = y[1] - a - y[0];

        var nGrids = (long)(StepsPart2 - _grid.Cols / 2) / _grid.Cols;
        var res = a * nGrids * nGrids + b * nGrids + c;
        return res.ToString();
    }

    private int CalculatePossiblePlots(GridPos2d start, int targetSteps)
    {
        var res = 0;
        var toVisit = new Queue<(GridPos2d pos, int step)>();
        toVisit.Enqueue((start, 0));
        var seen = new HashSet<GridPos2d>();
        while (toVisit.TryDequeue(out var cur))
        {
            var (pos, step) = cur;
            if (!seen.Add(pos))
            {
                continue;
            }

            if (step == targetSteps)
            {
                res += 1;
                continue;
            }

            if (step % 2 == targetSteps % 2)
            {
                res += 1;
            }

            foreach (var nextPos in pos.AdjacentSide())
            {
                if (GetInfiniteGridValue(nextPos) == Rock)
                {
                    continue;
                }

                toVisit.Enqueue((nextPos, step + 1));
            }
        }

        return res;
    }

    private char GetInfiniteGridValue(GridPos2d pos)
    {
        return _grid[pos.Row.Mod(_grid.Rows), pos.Col.Mod(_grid.Cols)];
    }
}