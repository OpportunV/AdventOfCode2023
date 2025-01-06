using System.Collections.Generic;
using System.Linq;
using AdventOfCode2023.Models.Day22;
using Common.Extensions;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day22 : Day
{
    private readonly List<Brick> _bricks;

    public Day22()
    {
        _bricks = GetInput()
            .Select(line => line.GetNumbers<int>())
            .Select(vals => new Brick(vals[..3], vals[3..]))
            .OrderBy(brick => brick.Z.Start)
            .ToList();
    }

    public override string Part1()
    {
        _bricks.ForEach(MoveBrick);
        var res = _bricks.Count(brick => brick.Supports.All(other => other.SupportedBy.Count > 1));
        return res.ToString();
    }

    public override string Part2()
    {
        if (_bricks.Any(brick => !brick.Fixed))
        {
            _bricks.ForEach(MoveBrick);
        }

        var res = 0;
        foreach (var brick in _bricks)
        {
            var removed = new HashSet<Brick> { brick };
            var toVisit = new Queue<Brick>();
            toVisit.Enqueue(brick);
            while (toVisit.TryDequeue(out var cur))
            {
                foreach (var next in cur.Supports)
                {
                    if (removed.Contains(next))
                    {
                        continue;
                    }

                    if (next.SupportedBy.All(support => removed.Contains(support)))
                    {
                        removed.Add(next);
                    }

                    toVisit.Enqueue(next);
                }
            }

            res += removed.Count - 1;
        }

        return res.ToString();
    }

    private void MoveBrick(Brick brick)
    {
        while (true)
        {
            if (brick.Z.Start == 1)
            {
                break;
            }

            var dz = _bricks.Select(other => brick.Z.Start - other.Z.End + 1).Where(val => val > 0).Min();
            brick.Z = new Range<int>(brick.Z.Start - dz, brick.Z.End - dz);
            var found = false;
            foreach (var other in _bricks)
            {
                if (!other.Fixed)
                {
                    continue;
                }

                if (other.Z.End - 1 != brick.Z.Start)
                {
                    continue;
                }

                if (other.X.Intersects(brick.X) && other.Y.Intersects(brick.Y))
                {
                    other.Supports.Add(brick);
                    brick.SupportedBy.Add(other);
                    found = true;
                }
            }

            if (found)
            {
                brick.Z = new Range<int>(brick.Z.Start + 1, brick.Z.End + 1);
                break;
            }
        }

        brick.Fixed = true;
    }
}