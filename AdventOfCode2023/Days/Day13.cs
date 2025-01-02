using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day13 : Day
{
    private readonly List<Grid<char>> _grids;
    private const int HorizontalModifier = 100;
    private const int Part1Target = 0;
    private const int Part2Target = 1;

    public Day13()
    {
        _grids = GetInputRaw()
            .Split("\n\n")
            .Select(grid => grid.Split("\n")
                .Select(line => line.ToCharArray())
                .ToArray())
            .Select(grid => new Grid<char>(grid))
            .ToList();
    }

    public override string Part1()
    {
        return _grids.Sum(grid => CalculateGrid(grid, Part1Target)).ToString();
    }

    public override string Part2()
    {
        return _grids.Sum(grid => CalculateGrid(grid, Part2Target)).ToString();
    }

    private static int CalculateGrid(Grid<char> grid, int target)
    {
        for (var i = 1; i < grid.Rows; i++)
        {
            if (ValidateRowMirror(grid, i, target))
            {
                return HorizontalModifier * i;
            }
        }

        for (var i = 1; i < grid.Cols; i++)
        {
            if (ValidateColMirror(grid, i, target))
            {
                return i;
            }
        }

        throw new ArgumentException($"No mirror found for {grid}");
    }

    private static bool ValidateRowMirror(Grid<char> grid, int row, int target)
    {
        var left = row - 1;
        var right = row;
        var total = 0;
        while (left >= 0 && right < grid.Rows)
        {
            total += Enumerable.Range(0, grid.Cols).Count(i => grid[left, i] != grid[right, i]);
            if (total > target)
            {
                return false;
            }

            left--;
            right++;
        }

        return total == target;
    }

    private static bool ValidateColMirror(Grid<char> grid, int col, int target)
    {
        var top = col - 1;
        var bot = col;
        var total = 0;
        while (top >= 0 && bot < grid.Cols)
        {
            total += Enumerable.Range(0, grid.Rows).Count(i => grid[i, top] != grid[i, bot]);
            if (total > target)
            {
                return false;
            }

            top--;
            bot++;
        }

        return total == target;
    }
}