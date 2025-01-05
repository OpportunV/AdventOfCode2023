using System;

namespace AdventOfCode2023.Models.Day20;

public record Dummy(string Name) : Module(Name)
{
    public override void Receive(Module source)
    {
    }

    public override bool Send()
    {
        return false;
    }
}