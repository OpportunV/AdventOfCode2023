using System.Collections.Generic;
using System.Linq;
using AdventOfCode2023.Models.Day20;
using Common.Extensions;
using Common.Helpers;

namespace AdventOfCode2023.Days;

public class Day20 : Day
{
    private Button _button;
    private Dictionary<string, Module> _modules;
    private const string TargetModuleName = "rx";

    public override string Part1()
    {
        ParseModules();
        for (var i = 0; i < 1000; i++)
        {
            PressButton();
        }

        return Module.SignalCounter.Values.Product().ToString();
    }

    public override string Part2()
    {
        ParseModules();
        var res = 0;
        var target = _modules.Values
            .SelectMany(module => module.Destinations)
            .First(module => module.Name == TargetModuleName);
        var writesToTarget = _modules.Values
            .First(module => module.Destinations.Contains(target));
        var writesToWriter = _modules.Values
            .Where(module => module.Destinations.Contains(writesToTarget))
            .ToList();

        var cycles = Enumerable.Repeat(0L, writesToWriter.Count).ToArray();
        for (var i = 0; i < writesToWriter.Count; i++)
        {
            var cur = i;
            writesToWriter[i].Sent += module =>
            {
                if (cycles[cur] == 0 && module.LastSignalHigh)
                {
                    // ReSharper disable once AccessToModifiedClosure
                    cycles[cur] = res;
                }
            };
        }

        while (cycles.Any(cycle => cycle == 0))
        {
            res++;
            PressButton();
        }

        return cycles.Aggregate(cycles[0], Calculator.Lcm).ToString();
    }

    private void PressButton()
    {
        var toUse = new Queue<Module>();
        toUse.Enqueue(_button);

        while (toUse.TryDequeue(out var cur))
        {
            if (!cur.Send())
            {
                continue;
            }

            cur.Destinations.ForEach(dest => toUse.Enqueue(dest));
        }
    }

    private void ParseModules()
    {
        _modules = GetInput().Select(ModuleFactory.FromString)
            .ToDictionary(module => module.Name, module => module);
        ModuleFactory.FillDestinations(_modules);

        _button = new Button(nameof(Button))
        {
            Destinations = _modules.Values.Where(module => module is Broadcaster).ToList()
        };
    }
}