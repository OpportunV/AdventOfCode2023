using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2023.Models.Day20;

public record Conjunction(string Name) : Module(Name)
{
    private readonly Dictionary<string, bool> _memory = new();

    public void InitMemory(string input)
    {
        _memory[input] = false;
    }

    public override void Receive(Module source)
    {
        _memory[source.Name] = source.LastSignalHigh;
    }

    public override bool Send()
    {
        LastSignalHigh = !_memory.Values.All(val => val);
        return base.Send();
    }
}