using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day14 : Day
{
    private readonly Grid<char> _grid;
    private const char Empty = '.';
    private const char RoundRock = 'O';
    private const int CyclesAmount = 1_000_000_000;

    public Day14()
    {
        var grid = GetInput()
            .Select(line => line.ToCharArray())
            .ToArray();

        _grid = new Grid<char>(grid);
    }

    public override string Part1()
    {
        var grid = new Grid<char>(_grid);
        Tilt(grid, GridPos2d.Up);
        return CalculateLoad(grid).ToString();
    }

    public override string Part2()
    {
        var grid = new Grid<char>(_grid);
        List<GridPos2d> cycle = [GridPos2d.Up, GridPos2d.Left, GridPos2d.Down, GridPos2d.Right];
        var curCycle = 0;
        var seen = new Dictionary<string, int> { { grid.ToString(), curCycle } };
        while (true)
        {
            cycle.ForEach(dir => Tilt(grid, dir));
            curCycle++;
            var gridStr = grid.ToString();
            if (!seen.TryAdd(gridStr, curCycle))
            {
                break;
            }
        }

        var firstCycle = seen[grid.ToString()];
        var cycleLength = curCycle - firstCycle;
        var targetCycle = (CyclesAmount - firstCycle) % cycleLength + firstCycle;
        var targetGridStr = seen.First(pair => pair.Value == targetCycle).Key;
        var gridArray = targetGridStr
            .Trim()
            .Split("\r\n")
            .Select(line => line.ToCharArray())
            .ToArray();
        var targetGrid = new Grid<char>(gridArray);

        return CalculateLoad(targetGrid).ToString();
    }

    private static int CalculateLoad(Grid<char> grid)
    {
        return grid.Flatten().Where(item => item.Value == RoundRock).Sum(item => grid.Rows - item.Pos.Row);
    }

    private static void Tilt(Grid<char> grid, GridPos2d dir)
    {
        foreach (var (value, pos) in TraverseByDirection(grid, dir))
        {
            if (value != RoundRock)
            {
                continue;
            }

            var nextPos = pos + dir;
            while (grid.Contains(nextPos) && grid[nextPos] == Empty)
            {
                nextPos += dir;
            }

            grid[pos] = Empty;
            grid[nextPos - dir] = RoundRock;
        }
    }

    private static IEnumerable<GridItem<char>> TraverseByDirection(Grid<char> grid, GridPos2d dir)
    {
        if (dir == GridPos2d.Up)
        {
            return grid.Flatten();
        }

        if (dir == GridPos2d.Left)
        {
            return grid.VerticalFlatten();
        }

        if (dir == GridPos2d.Down)
        {
            return grid.Flatten().Reverse();
        }

        if (dir == GridPos2d.Right)
        {
            return grid.VerticalFlatten().Reverse();
        }

        throw new ArgumentOutOfRangeException($"Unsupproted direction {dir}");
    }
}