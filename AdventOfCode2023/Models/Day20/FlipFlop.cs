using System;
using System.Collections.Generic;

namespace AdventOfCode2023.Models.Day20;

public record FlipFlop(string Name) : Module(Name)
{
    private bool _enabled;
    private readonly List<Action> _toSend = [];

    public override void Receive(Module source)
    {
        if (source.LastSignalHigh)
        {
            return;
        }

        _toSend.Add(() =>
        {
            LastSignalHigh = !_enabled;
            _enabled = !_enabled;
            base.Send();
        });
    }

    public override bool Send()
    {
        _toSend.ForEach(action => action.Invoke());
        var res = _toSend.Count > 0;
        _toSend.Clear();
        return res;
    }
}