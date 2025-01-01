using System;
using System.Collections.Generic;
using System.Linq;
using Common.Helpers;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day11 : Day
{
    private readonly List<GridPos2d> _galaxies;
    private readonly HashSet<int> _emptyRows;
    private readonly HashSet<int> _emptyCols;
    private const int Galaxy = '#';
    private const int Empty = '.';

    public Day11()
    {
        var inp = GetInput().Select(line => line.ToCharArray()).ToArray();
        var grid = new Grid<char>(inp);
        _galaxies = grid
            .Flatten()
            .Where(item => item.Value == Galaxy).Select(item => item.Pos)
            .ToList();
        _emptyRows = Enumerable.Range(0, grid.Rows)
            .Where(row => Enumerable.Range(0, grid.Cols)
                .All(col => grid[row, col] == Empty))
            .ToHashSet();

        _emptyCols = Enumerable.Range(0, grid.Cols)
            .Where(col => Enumerable.Range(0, grid.Rows)
                .All(row => grid[row, col] == Empty))
            .ToHashSet();
    }

    public override string Part1()
    {
        return CalculateDistancesSum(2).ToString();
    }

    public override string Part2()
    {
        return CalculateDistancesSum(1_000_000).ToString();
    }

    private long CalculateDistancesSum(int expansionFactor)
    {
        var ans = 0L;
        foreach (var (first, second) in Combinations.GenerateAllPairs(_galaxies))
        {
            for (var i = Math.Min(first.Row, second.Row); i < Math.Max(first.Row, second.Row); i++)
            {
                ans += _emptyRows.Contains(i) ? expansionFactor : 1;
            }

            for (var i = Math.Min(first.Col, second.Col); i < Math.Max(first.Col, second.Col); i++)
            {
                ans += _emptyCols.Contains(i) ? expansionFactor : 1;
            }
        }

        return ans;
    }
}