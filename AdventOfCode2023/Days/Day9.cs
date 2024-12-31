using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AdventOfCode2023.Days;

public class Day9 : Day
{
    private readonly List<List<int>> _histories;

    public Day9()
    {
        _histories = GetInput().Select(line => line.GetNumbers<int>()).ToList();
    }

    public override string Part1()
    {
        return _histories.Select(Predict).Sum().ToString();
    }

    public override string Part2()
    {
        return _histories.Select(PredictBackwards).Sum().ToString();
    }

    private static int Predict(List<int> history)
    {
        var deltas = new List<int> { history[^1] };
        while (history.Any(item => item != 0))
        {
            history = GetNext(history);
            deltas.Add(history[^1]);
        }

        return deltas.Sum();
    }

    private static int PredictBackwards(List<int> history)
    {
        var deltas = new List<int> { history[0] };
        while (history.Any(item => item != 0))
        {
            history = GetNext(history);
            deltas.Add(history[0]);
        }

        var ans = 0;
        for (var i = deltas.Count - 2; i >= 0; i--)
        {
            ans = deltas[i] - ans;
        }

        return ans;
    }

    private static List<int> GetNext(List<int> history)
    {
        var next = new List<int>();
        for (var i = 1; i < history.Count; i++)
        {
            next.Add(history[i] - history[i - 1]);
        }

        return next;
    }
}