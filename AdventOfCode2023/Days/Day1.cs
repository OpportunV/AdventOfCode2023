namespace AdventOfCode2023.Days;

public class Day1 : Day
{
    private readonly Dictionary<string, string> _digits = new()
    {
        { "one", "1" },
        { "two", "2" },
        { "three", "3" },
        { "four", "4" },
        { "five", "5" },
        { "six", "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine", "9" },
    };

    public override string Part1()
    {
        var input = GetInput();
        return input
               .Select(line => GetFirstDigit(line) + GetLastDigit(line))
               .Select(int.Parse)
               .Sum()
               .ToString();
    }

    public override string Part2()
    {
        var input = GetInput();
        return input
               .Select(line => GetFirstDigitWithWords(line) + GetLastDigitWithWords(line))
               .Select(int.Parse)
               .Sum()
               .ToString();
    }

    private string GetFirstDigit(string line)
    {
        return line
               .Select(chr => chr.ToString())
               .First(item => int.TryParse(item, out _));
    }

    private string GetLastDigit(string line)
    {
        return line
               .Select(chr => chr.ToString())
               .Last(item => int.TryParse(item, out _));
    }

    private string GetFirstDigitWithWords(string line)
    {
        var indexWordPairs = _digits
                             .Keys.Select(digitWord => (
                                              index: line.IndexOf(
                                                  digitWord, StringComparison.InvariantCultureIgnoreCase), digitWord))
                             .Where(pair => pair.Item1 >= 0)
                             .ToList();

        var indexDigitPairs = _digits
                              .Values.Select(digit =>
                                                 (index: line.IndexOf(digit, StringComparison.InvariantCultureIgnoreCase),
                                                 digit))
                              .Where(pair => pair.index >= 0)
                              .ToList();

        if (indexWordPairs.Any() && indexDigitPairs.Any())
        {
            var indexWordPair = indexWordPairs.MinBy(pair => pair.index);
            var indexDigitPair = indexDigitPairs.MinBy(pair => pair.index);
            return indexWordPair.index < indexDigitPair.index
                ? _digits[indexWordPair.digitWord]
                : indexDigitPair.digit;
        }

        if (indexWordPairs.Any())
        {
            var indexWordPair = indexWordPairs.MinBy(pair => pair.index);
            return _digits[indexWordPair.digitWord];
        }

        if (indexDigitPairs.Any())
        {
            var indexDigitPair = indexDigitPairs.MinBy(pair => pair.index);
            return indexDigitPair.digit;
        }

        throw new ArgumentOutOfRangeException($"cannot find first digit in line {line}");
    }

    private string GetLastDigitWithWords(string line)
    {
        var indexWordPairs = _digits
                             .Keys.Select(digitWord => (
                                              index: line.LastIndexOf(
                                                  digitWord, StringComparison.InvariantCultureIgnoreCase), digitWord))
                             .Where(pair => pair.Item1 >= 0)
                             .ToList();

        var indexDigitPairs = _digits
                              .Values.Select(
                                  digit => (index: line.LastIndexOf(digit, StringComparison.InvariantCultureIgnoreCase),
                                      digit))
                              .Where(pair => pair.index >= 0)
                              .ToList();

        if (indexWordPairs.Any() && indexDigitPairs.Any())
        {
            var indexWordPair = indexWordPairs.MaxBy(pair => pair.index);
            var indexDigitPair = indexDigitPairs.MaxBy(pair => pair.index);
            return indexWordPair.index > indexDigitPair.index
                ? _digits[indexWordPair.digitWord]
                : indexDigitPair.digit;
        }

        if (indexWordPairs.Any())
        {
            var indexWordPair = indexWordPairs.MaxBy(pair => pair.index);
            return _digits[indexWordPair.digitWord];
        }

        if (indexDigitPairs.Any())
        {
            var indexDigitPair = indexDigitPairs.MaxBy(pair => pair.index);
            return indexDigitPair.digit;
        }

        throw new ArgumentOutOfRangeException($"cannot find last digit in line {line}");
    }
}