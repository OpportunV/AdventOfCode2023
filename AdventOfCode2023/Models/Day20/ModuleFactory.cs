using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023.Models.Day20;

public static class ModuleFactory
{
    private static readonly Dictionary<string, string[]> _destinations = new();

    public static Module FromString(string input)
    {
        var split = input.Split(" -> ");
        var name = split[0];
        var destinations = split[1].Split(", ");
        var trueName = name.TrimStart('%').TrimStart('&');
        _destinations[trueName] = destinations;
        if (name.StartsWith("%"))
        {
            return new FlipFlop(trueName);
        }

        if (name.StartsWith("&"))
        {
            return new Conjunction(trueName);
        }

        return new Broadcaster(name);
    }

    public static void FillDestinations(Dictionary<string, Module> modules)
    {
        foreach (var module in modules.Values)
        {
            module.Destinations = _destinations[module.Name]
                .Select(dest => modules.GetValueOrDefault(dest, new Dummy(dest))).ToList();

            foreach (var conjunction in module.Destinations.OfType<Conjunction>())
            {
                conjunction.InitMemory(module.Name);
            }
        }
    }
}