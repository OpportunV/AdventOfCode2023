using System.Collections.Generic;
using Common.Models;

namespace AdventOfCode2023.Models.Day22;

public record Brick
{
    public Range<int> X { get; }

    public Range<int> Y { get; }

    public Range<int> Z { get; set; }

    public bool Fixed { get; set; }

    public readonly HashSet<Brick> Supports = [];
    public readonly HashSet<Brick> SupportedBy = [];

    public Brick(List<int> start, List<int> end)
    {
        X = new Range<int>(start[0], end[0] + 1);
        Y = new Range<int>(start[1], end[1] + 1);
        Z = new Range<int>(start[2], end[2] + 1);
    }
}