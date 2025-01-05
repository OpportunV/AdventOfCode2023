using System;
using System.Collections.Generic;

namespace AdventOfCode2023.Models.Day20;

public abstract record Module(string Name)
{
    public List<Module> Destinations { get; set; }

    public event Action<Module> Sent;

    public bool LastSignalHigh { get; set; }

    public static readonly Dictionary<bool, int> SignalCounter = new() { { true, 0 }, { false, 0 } };

    public abstract void Receive(Module source);

    public virtual bool Send()
    {
        foreach (var module in Destinations)
        {
            SignalCounter[LastSignalHigh]++;
            module.Receive(this);
        }

        Sent?.Invoke(this);
        return true;
    }
}