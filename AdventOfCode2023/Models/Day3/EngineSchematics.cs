using System.Text.RegularExpressions;
using Helpers.Table;


namespace AdventOfCode2023.Models.Day3;

public class EngineSchematics
{
    public IEnumerable<int> PartNumbers => _engineNumbers
                                           .Where(number => number.IsPartNumber)
                                           .Select(number => number.Value);

    public IEnumerable<int> GearValues => _engineGears
                                          .Values.Where(engineNumbers => engineNumbers.Count == 2)
                                          .Select(engineNumbers => engineNumbers[0].Value * engineNumbers[1].Value);

    private readonly string[] _schematics;
    private readonly List<EngineNumber> _engineNumbers = new();
    private readonly Regex _regexNum = new(@"\d+", RegexOptions.IgnoreCase);
    private readonly int _schematicsWidth;
    private readonly int _schematicsHeight;
    private readonly Dictionary<Position2d, List<EngineNumber>> _engineGears = new();

    public EngineSchematics(string[] schematics)
    {
        _schematics = schematics;
        _schematicsWidth = _schematics[0].Length;
        _schematicsHeight = _schematics.Length;
        GetEngineNumbers();
        VerifyPartNumbers();
    }

    private void GetEngineNumbers()
    {
        for (var column = 0; column < _schematicsHeight; column++)
        {
            var schemaLine = _schematics[column];
            var matches = _regexNum.Matches(schemaLine);
            foreach (Match match in matches)
            {
                var engineNumber = new EngineNumber(int.Parse(match.Value), column, match.Index, match.Length);
                _engineNumbers.Add(engineNumber);
            }
        }
    }

    private void VerifyPartNumbers()
    {
        foreach (var engineNumber in _engineNumbers)
        {
            engineNumber.IsPartNumber = !(TopRowEmpty(engineNumber)
                                          && BottomRowEmpty(engineNumber)
                                          && LeftEmpty(engineNumber)
                                          && RightEmpty(engineNumber));
        }
    }

    private bool TopRowEmpty(EngineNumber engineNumber)
    {
        if (engineNumber.Row <= 0)
        {
            return true;
        }

        var row = engineNumber.Row - 1;
        var startIndex = Math.Max(engineNumber.Index - 1, 0);
        var endIndex = Math.Min(engineNumber.EndIndex + 2, _schematicsWidth - 1);
        var flag = true;
        for (var col = startIndex; col < endIndex; col++)
        {
            var chr = _schematics[row][col];
            CheckGear(chr, col, row, engineNumber);

            if (chr != '.')
            {
                flag = false;
            }
        }

        return flag;
    }

    private bool BottomRowEmpty(EngineNumber engineNumber)
    {
        if (engineNumber.Row >= _schematicsHeight - 1)
        {
            return true;
        }

        var row = engineNumber.Row + 1;
        var startIndex = Math.Max(engineNumber.Index - 1, 0);
        var endIndex = Math.Min(engineNumber.EndIndex + 2, _schematicsWidth - 1);
        var flag = true;
        for (var col = startIndex; col < endIndex; col++)
        {
            var chr = _schematics[row][col];
            CheckGear(chr, col, row, engineNumber);

            if (chr != '.')
            {
                flag = false;
            }
        }

        return flag;
    }

    private bool LeftEmpty(EngineNumber engineNumber)
    {
        if (engineNumber.Index == 0)
        {
            return true;
        }

        var chr = _schematics[engineNumber.Row][engineNumber.Index - 1];
        CheckGear(chr, engineNumber.Index - 1, engineNumber.Row, engineNumber);

        return chr == '.';
    }

    private bool RightEmpty(EngineNumber engineNumber)
    {
        if (engineNumber.EndIndex == _schematicsWidth - 1)
        {
            return true;
        }

        var chr = _schematics[engineNumber.Row][engineNumber.EndIndex + 1];
        CheckGear(chr, engineNumber.EndIndex + 1, engineNumber.Row, engineNumber);

        return chr == '.';
    }

    private void CheckGear(char chr, int col, int row, EngineNumber engineNumber)
    {
        if (chr != '*')
        {
            return;
        }

        var gearPos = new Position2d(col, row);
        if (_engineGears.TryGetValue(gearPos, out var engineGear))
        {
            engineGear.Add(engineNumber);
        }
        else
        {
            _engineGears[gearPos] = new List<EngineNumber> { engineNumber };
        }
    }
}