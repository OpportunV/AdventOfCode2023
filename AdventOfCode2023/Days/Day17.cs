using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.Helpers;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day17 : Day
{
    private readonly Grid<int> _grid;
    private readonly GridPos2d _target;

    private const int MaxMovesPart1 = 3;
    private const int MaxMovesPart2 = 10;

    public Day17()
    {
        var grid = GetInput()
            .Select(line => line.Select(c => c - '0').ToArray())
            .ToArray();

        _grid = new Grid<int>(grid);
        _target = new GridPos2d(_grid.Rows - 1, _grid.Cols - 1);
    }

    public override string Part1()
    {
        return FindPath(AddToVisitPart1).ToString();
    }

    public override string Part2()
    {
        return FindPath(AddToVisitPart2).ToString();
    }

    private int FindPath(Action<GridPos2d, int, PriorityQueue<(GridPos2d pos, int dirInd), int>, int> addFunc)
    {
        var toVisit = new PriorityQueue<(GridPos2d pos, int dirInd), int>();
        var seen = new HashSet<(GridPos2d pos, int dirInd)>();
        toVisit.Enqueue((GridPos2d.Zero, 0), 0);
        toVisit.Enqueue((GridPos2d.Zero, 1), 0);

        while (toVisit.TryDequeue(out var cur, out var cost))
        {
            var (pos, dirInd) = cur;

            if (!seen.Add(cur))
            {
                continue;
            }

            if (pos == _target)
            {
                return cost;
            }

            addFunc?.Invoke(pos, dirInd, toVisit, cost);
        }

        throw new ArgumentException($"Target pos {_target} has not been reached.");
    }

    private void AddToVisitPart1(GridPos2d pos, int dirInd, PriorityQueue<(GridPos2d pos, int dirInd), int> toVisit,
        int cost)
    {
        var stepCost = 0;
        for (var i = 1; i <= MaxMovesPart1; i++)
        {
            var newPos = pos + Directions2d.Side[dirInd] * i;
            if (!_grid.Contains(newPos))
            {
                break;
            }

            stepCost += _grid[newPos];
            toVisit.Enqueue((newPos, (dirInd + 1).Mod(Directions2d.Side.Count)), cost + stepCost);
            toVisit.Enqueue((newPos, (dirInd - 1).Mod(Directions2d.Side.Count)), cost + stepCost);
        }
    }

    private void AddToVisitPart2(GridPos2d pos, int dirInd, PriorityQueue<(GridPos2d pos, int dirInd), int> toVisit,
        int cost)
    {
        var stepCost = 0;
        for (var i = 1; i <= MaxMovesPart2; i++)
        {
            var newPos = pos + Directions2d.Side[dirInd] * i;
            if (!_grid.Contains(newPos))
            {
                break;
            }

            stepCost += _grid[newPos];

            if (i < 4)
            {
                continue;
            }

            toVisit.Enqueue((newPos, (dirInd + 1).Mod(Directions2d.Side.Count)), cost + stepCost);
            toVisit.Enqueue((newPos, (dirInd - 1).Mod(Directions2d.Side.Count)), cost + stepCost);
        }
    }
}