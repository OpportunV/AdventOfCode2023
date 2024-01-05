using System.Text.RegularExpressions;
using AdventOfCode2023.Models.Day5;
using Helpers.Data;


namespace AdventOfCode2023.Days;

public class Day5 : Day
{
    private IEnumerable<long> _seeds = Enumerable.Empty<long>();
    private readonly Dictionary<int, List<Converter>> _converters;

    public Day5()
    {
        var rawInput = GetInputRaw();
        _converters = ParseConverters(rawInput);
    }

    public override string Part1()
    {
        var valuesToConvert = _seeds.ToList();
        foreach (var converters in _converters.Values)
        {
            valuesToConvert = valuesToConvert.Select(valueToConvert =>
                                                         converters.FirstOrDefault(
                                                                 converter =>
                                                                     converter.IsApplicableToNumber(valueToConvert))
                                                             ?.Convert(valueToConvert)
                                                         ?? valueToConvert).ToList();
        }

        return valuesToConvert.Min().ToString();
    }

    public override string Part2()
    {
        var valuesToConvert = _seeds.ToList();
        var result = long.MaxValue;
        for (var i = 0; i < valuesToConvert.Count; i += 2)
        {
            var startingRange = new Range<long>(valuesToConvert[i], valuesToConvert[i] + valuesToConvert[i + 1] - 1);
            var ranges = new Queue<Range<long>>(new[] { startingRange });
            foreach (var converters in _converters.Values)
            {
                var newRanges = new HashSet<Range<long>>();
                while (ranges.TryDequeue(out var range))
                {
                    var converted = false;
                    foreach (var converter in converters)
                    {
                        if (!converter.TryConvertRange(range, out var res, out var extras))
                        {
                            continue;
                        }

                        converted = true;
                        newRanges.Add(res);
                        extras.ForEach(ranges.Enqueue);
                        break;
                    }

                    if (!converted)
                    {
                        newRanges.Add(range);
                    }
                }

                ranges = new Queue<Range<long>>(newRanges);
            }

            result = Math.Min(result, ranges.MinBy(range => range.Start).Start);
        }

        return result.ToString();
    }

    private Dictionary<int, List<Converter>> ParseConverters(string input)
    {
        var regex = new Regex(@"\d+");
        const StringSplitOptions SplitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
        var entries = input.Split(new[] { "\r\n\r\n", "\n\n", "\r\r" }, SplitOptions);

        _seeds = regex.Matches(entries[0]).Select(match => long.Parse(match.Value));

        var converters = entries[1..]
            .Select((data, i) => (data: data.Split(new[] { "\r\n", "\n", "\r" }, SplitOptions)[1..], index: i))
            .ToDictionary(pair => pair.index,
                          pair => pair.data.Select(converterText =>
                                                       new Converter(
                                                           regex.Matches(converterText)
                                                               .Select(item => long.Parse(item.Value)).ToArray()))
                              .ToList());

        return converters;
    }
}