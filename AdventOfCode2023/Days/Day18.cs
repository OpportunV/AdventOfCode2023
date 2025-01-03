using System;
using System.Collections.Generic;
using System.Linq;
using Common.Helpers;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day18 : Day
{
    private readonly List<string> _directions = [];
    private readonly List<int> _lengths = [];
    private readonly List<string> _colors = [];

    public Day18()
    {
        foreach (var line in GetInput())
        {
            var split = line.Split(" ");
            _directions.Add(split[0]);
            _lengths.Add(int.Parse(split[1]));
            _colors.Add(split[2][1..^1]);
        }
    }

    public override string Part1()
    {
        var dirs = _directions.Select(dir => dir switch
        {
            "R" => GridPos2d.Right,
            "D" => GridPos2d.Down,
            "L" => GridPos2d.Left,
            "U" => GridPos2d.Up,
            _ => throw new ArgumentOutOfRangeException($"Unexpected direction {dir}")
        });

        var corners = GetCornersAndBoundary(dirs.Zip(_lengths), out var boundary);
        return Calculator.PolygonTotalPoints(corners, boundary).ToString();
    }

    public override string Part2()
    {
        var corners = GetCornersAndBoundary(_colors.Select(ConvertColor), out var boundary);
        return Calculator.PolygonTotalPoints(corners, boundary).ToString();
    }

    private static List<GridPos2d> GetCornersAndBoundary(IEnumerable<(GridPos2d dir, int length)> pairs,
        out long boundary)
    {
        var current = GridPos2d.Zero;
        var corners = new List<GridPos2d>();
        boundary = 0L;
        foreach (var (dir, length) in pairs)
        {
            current += dir * length;
            corners.Add(current);
            boundary += length;
        }

        return corners;
    }

    private static (GridPos2d dir, int length) ConvertColor(string color)
    {
        var length = Convert.ToInt32(color[1..^1], 16);
        var dirInd = color[^1] - '0';
        var dir = Directions2d.Side[dirInd];
        return (dir, length);
    }
}