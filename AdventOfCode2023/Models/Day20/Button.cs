namespace AdventOfCode2023.Models.Day20;

public record Button(string Name) : Module(Name)
{
    public override void Receive(Module source)
    {
    }
}