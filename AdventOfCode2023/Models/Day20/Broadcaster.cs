using System.Collections.Generic;

namespace AdventOfCode2023.Models.Day20;

public record Broadcaster(string Name) : Module(Name)
{
    public override void Receive(Module source)
    {
        LastSignalHigh = source.LastSignalHigh;
    }
}