using AdventOfCode2023.Models.Day2;


namespace AdventOfCode2023.Days;

public class Day2 : Day
{
    private readonly CubeSet _limit = new(12, 13, 14);

    public override string Part1()
    {
        var games = GetGames();
        games.ForEach(game => game.CheckLimit(_limit));
        return games.Where(game => game.Possible).Sum(game => game.Id).ToString();
    }

    public override string Part2()
    {
        var games = GetGames();
        return games.Select(game => game.MinimumCubeSet.Power).Sum().ToString();
    }

    private List<CubeGame> GetGames()
    {
        var input = GetInput();
        var games = input.Select(inp => new CubeGame(inp)).ToList();
        return games;
    }
}