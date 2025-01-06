using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Common.Extensions;
using Common.Helpers;
using Common.Models;

namespace AdventOfCode2023.Days;

public class Day24 : Day
{
    private readonly List<(Vector3<decimal> pos, Vector3<decimal> vel)> _hailstones;
    private const decimal Min = 200_000_000_000_000M;
    private const decimal Max = 400_000_000_000_000M;
    private const decimal CheckLimit = 400m;

    public Day24()
    {
        _hailstones = GetInput()
            .Select(line => line.GetNumbers<decimal>())
            .Select(nums =>
                (new Vector3<decimal>(nums[0], nums[1], nums[2]), new Vector3<decimal>(nums[3], nums[4], nums[5])))
            .ToList();
    }

    public override string Part1()
    {
        var res = 0;
        foreach (var (first, second) in Combinations.GenerateAllPairs(_hailstones))
        {
            if (TryGetIntersectionXy(first, second, out var intersection))
            {
                if (intersection.X is <= Max and >= Min && intersection.Y is <= Max and >= Min)
                {
                    res++;
                }
            }
        }

        return res.ToString();
    }

    public override string Part2()
    {
        var pos1 = GetRockXy(_hailstones);
        var hailstones = _hailstones.Select(SwapCoords).ToList();
        var pos2 = GetRockXy(hailstones);
        return (pos1.X + pos1.Y + pos2.Y).ToString(CultureInfo.InvariantCulture);
    }

    private Vector3<decimal> GetRockXy(List<(Vector3<decimal> pos, Vector3<decimal> vel)> hailstones)
    {
        var (h0, h1) = (hailstones[0], hailstones[1]);
        for (var vx = -CheckLimit; vx < CheckLimit; vx++)
        {
            for (var vy = -CheckLimit; vy < CheckLimit; vy++)
            {
                var vel = new Vector3<decimal>(vx, vy, 0);
                var nh0 = ModifyVel(h0, vel);
                var nh1 = ModifyVel(h1, vel);

                if (TryGetIntersectionXy(nh0, nh1, out var intersection))
                {
                    if (hailstones.All(hailstone => GoesThroughXy(ModifyVel(hailstone, vel), intersection)))
                    {
                        return intersection;
                    }
                }
            }
        }

        throw new UnreachableException();

        bool GoesThroughXy((Vector3<decimal> pos, Vector3<decimal> vel) hailstone, Vector3<decimal> pos)
        {
            var diff = (pos.X - hailstone.pos.X) * hailstone.vel.Y - (pos.Y - hailstone.pos.Y) * hailstone.vel.X;
            return Math.Abs(diff) < 0.001m;
        }
    }

    private static (Vector3<decimal> pos, Vector3<decimal> vel) ModifyVel(
        (Vector3<decimal> pos, Vector3<decimal> vel) hailstone, Vector3<decimal> vel)
    {
        return hailstone with { vel = hailstone.vel + vel };
    }

    private static (Vector3<decimal> pos, Vector3<decimal> vel) SwapCoords(
        (Vector3<decimal> pos, Vector3<decimal> vel) hailstone)
    {
        return (new Vector3<decimal>(hailstone.pos.X, hailstone.pos.Z, hailstone.pos.Y),
            new Vector3<decimal>(hailstone.vel.X, hailstone.vel.Z, hailstone.vel.Y));
    }

    private static bool TryGetIntersectionXy((Vector3<decimal> pos, Vector3<decimal> vel) first,
        (Vector3<decimal> pos, Vector3<decimal> vel) second, out Vector3<decimal> intersection)
    {
        intersection = default;
        if (first.vel.X == 0 || second.vel.X == 0)
        {
            return false;
        }

        var dv = first.vel.Y / first.vel.X;
        var fy = first.pos.Y - dv * first.pos.X;
        var sdv = second.vel.Y / second.vel.X;
        var sy = second.pos.Y - sdv * second.pos.X;

        if (dv == sdv)
        {
            return false;
        }

        var x = (sy - fy) / (dv - sdv);
        var t = (x - first.pos.X) / first.vel.X;
        var u = (x - second.pos.X) / second.vel.X;

        if (t < 0 || u < 0)
        {
            return false;
        }

        var y = dv * (x - first.pos.X) + first.pos.Y;
        intersection = new Vector3<decimal>(Math.Round(x, 0), Math.Round(y, 0), 0M);
        return true;
    }
}