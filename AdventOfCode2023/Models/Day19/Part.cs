using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AdventOfCode2023.Models.Day19;

public record Part(Dictionary<string, int> Data)
{
    public int Total => Data.Values.Sum();

    public static Part Parse(string input)
    {
        var numbers = input.GetNumbers<int>();
        var words = input.GetWords().Where((_, i) => i % 2 == 0);
        var values = words.Zip(numbers)
            .ToDictionary(pair => pair.First, pair => pair.Second);
        return new Part(values);
    }
}