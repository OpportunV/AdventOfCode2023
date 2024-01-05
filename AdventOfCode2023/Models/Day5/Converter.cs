using Helpers.Data;


namespace AdventOfCode2023.Models.Day5;

public class Converter
{
    private readonly Range<long> _sourceRange;
    private readonly Range<long> _destinationRange;

    public Converter(params long[] values) : this(values[0], values[1], values[2])
    {
    }

    private Converter(long destinationRangeStart, long sourceRangeStart, long rangeLength)
    {
        _sourceRange = new Range<long>(sourceRangeStart, sourceRangeStart + rangeLength);
        _destinationRange = new Range<long>(destinationRangeStart, destinationRangeStart + rangeLength);
    }

    public bool IsApplicableToNumber(long number)
    {
        return number >= _sourceRange.Start && number <= _sourceRange.End;
    }
    
    public long Convert(long number)
    {
        return _destinationRange.Start + number - _sourceRange.Start;
    }

    public bool TryConvertRange(Range<long> range, out Range<long> result, out List<Range<long>> extras)
    {
        extras = new List<Range<long>>();
        var overlap = range.Intersection(_sourceRange);
        if (!overlap.Ascending)
        {
            result = default;
            return false;
        }

        if (range.StartsBefore(overlap))
        {
            extras.Add(new Range<long>(range.Start, overlap.Start));
        }

        if (overlap.EndsBefore(range))
        {
            extras.Add(new Range<long>(overlap.End, range.End));
        }

        result = new Range<long>(Convert(overlap.Start), Convert(overlap.End));
        return true;
    }
}