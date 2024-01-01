using System.Text.RegularExpressions;


namespace AdventOfCode2023.Models.Day2;

public class CubeGame
{
    public int Id { get; private set; }

    public bool Possible { get; private set; }
    
    public CubeSet MinimumCubeSet => new(_rounds.MaxBy(set => set.Red)!.Red,
                                         _rounds.MaxBy(set => set.Green)!.Green,
                                         _rounds.MaxBy(set => set.Blue)!.Blue);

    private readonly List<CubeSet> _rounds = new();
    private readonly Regex _regexRed = new(@"(\d+) red", RegexOptions.IgnoreCase);
    private readonly Regex _regexGreen = new(@"(\d+) green", RegexOptions.IgnoreCase);
    private readonly Regex _regexBlue = new(@"(\d+) blue", RegexOptions.IgnoreCase);

    public CubeGame(string gameLog)
    {
        FillFromLog(gameLog);
    }

    public void CheckLimit(CubeSet limit)
    {
        Possible = _rounds.TrueForAll(round => round <= limit);
    }

    private void FillFromLog(string gameLog)
    {
        var gameLogSplit = gameLog.Split(": ");
        Id = int.Parse(gameLogSplit[0].Split(" ")[^1]);
        var rounds = gameLogSplit[1].Split(";");
        ParseRounds(rounds);
    }

    private void ParseRounds(IEnumerable<string> rounds)
    {
        foreach (var round in rounds)
        {
            _rounds.Add(new CubeSet(GetCubesByRegex(_regexRed, round), GetCubesByRegex(_regexGreen, round),
                                    GetCubesByRegex(_regexBlue, round)));
        }
    }

    private int GetCubesByRegex(Regex regex, string text)
    {
        var match = regex.Match(text);
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }

        return 0;
    }
}