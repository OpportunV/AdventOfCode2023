using System.Text.RegularExpressions;


namespace AdventOfCode2023.Models.Day4;

public class Card
{
    public HashSet<int> WinningNumbers { get; }

    public int Copies { get; set; } = 1;

    public int Id { get; init; }

    public int Points { get; init; }

    public Card(string cardData)
    {
        var cardDataSplit = cardData.Split(": ");
        Id = int.Parse(cardDataSplit[0].Split(" ")[^1]);

        var allNumbers = cardDataSplit[1].Split("|");

        var winingNumbers =
            new HashSet<int>(Regex.Matches(allNumbers[0], @"\d+").Select(match => int.Parse(match.Value)));
        var numbers = new HashSet<int>(Regex.Matches(allNumbers[1], @"\d+").Select(match => int.Parse(match.Value)));
        WinningNumbers = numbers.Intersect(winingNumbers).ToHashSet();
        Points = (int) Math.Pow(2, WinningNumbers.Count - 1);
    }
}