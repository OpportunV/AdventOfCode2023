using System.Text.RegularExpressions;
using AdventOfCode2023.Models.Day6;
using Helpers.Extensions;


namespace AdventOfCode2023.Days;

public class Day6 : Day
{
    public override string Part1()
    {
        var races = GetRaces();
        var num = races.Select(GetPossibleSolutionsCount).ToList();
        return num.Product().ToString();
    }

    public override string Part2()
    {
        var race = GetSingleRace();
        return GetPossibleSolutionsCount(race).ToString();
    }

    private int GetPossibleSolutionsCount(Race race)
    {
        var counter = 0;
        for (long i = 1; i < race.Time; i++)
        {
            var distance = (race.Time - i) * i;
            counter += distance > race.Distance ? 1 : 0;
        }

        return counter;
    }

    private List<Race> GetRaces()
    {
        var input = GetInput();
        var races = input.Select(inp => Regex.Matches(inp, @"\d+").Select(match => int.Parse(match.Value))).ToList();
        return races[0].Zip(races[1]).Select(tuple => new Race(tuple.First, tuple.Second)).ToList();
    }

    private Race GetSingleRace()
    {
        var input = GetInput();
        var race = input.Select(
            inp => long.Parse(string.Join("", Regex.Matches(inp, @"\d+").Select(match => match.Value)))).ToList();
        return new Race(race[0], race[1]);
    }
}