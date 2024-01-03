using AdventOfCode2023.Models.Day4;


namespace AdventOfCode2023.Days;

public class Day4 : Day
{
    private readonly IEnumerable<Card> _cards;

    public Day4()
    {
        _cards = GetInput().Select(inp => new Card(inp));
    }
    
    public override string Part1()
    {
        return _cards.Sum(card => card.Points).ToString();
    }

    public override string Part2()
    {
        var cards = _cards.ToArray();
        for (var i = 0; i < cards.Length; i++)
        {
            var card = cards[i];
            for (var k = 0; k < card.Copies; k++)
            {
                for (var j = i + 1; j < i + 1 + card.WinningNumbers.Count; j++)
                {
                    cards[j].Copies += 1;
                }
            }
        }

        return cards.Sum(card => card.Copies).ToString();
    }
}