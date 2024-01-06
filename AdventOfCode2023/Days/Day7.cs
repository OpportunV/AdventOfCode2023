using AdventOfCode2023.Models.Day7;


namespace AdventOfCode2023.Days;

public class Day7 : Day
{
    public override string Part1()
    {
        var hands = GetHands();
        var sortedHands = Enum.GetValues<CombinationType>()
            .Where(value => hands.Any(hand => hand.CombinationType == value)).ToDictionary(
                value => value,
                value => hands.Where(hand => hand.CombinationType == value).OrderBy(
                    hand => string.Join("", hand.Cards.Select(card => "23456789TJQKA".IndexOf(card).ToString("X")))))
            .Values.SelectMany(handsByType => handsByType);
        return sortedHands.Select((hand, i) => hand.Bid * (i + 1)).Sum().ToString();
    }

    public override string Part2()
    {
        var hands = GetHands();
        var sortedHands = Enum.GetValues<CombinationType>()
            .Where(value => hands.Any(hand => hand.AdvancedCombinationType == value)).ToDictionary(
                value => value,
                value => hands.Where(hand => hand.AdvancedCombinationType == value).OrderBy(
                    hand => string.Join("", hand.Cards.Select(card => "J23456789TQKA".IndexOf(card).ToString("X")))))
            .Values.SelectMany(handsByType => handsByType);
        return sortedHands.Select((hand, i) => hand.Bid * (i + 1)).Sum().ToString();
    }

    private List<Hand> GetHands()
    {
        var input = GetInput();
        return input.Select(inp => inp.Split(" ")).Select(split => new Hand(split[0], int.Parse(split[1]))).ToList();
    }
}