using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace AdventOfCode2023.Models.Day4;

public class Card
{
    public HashSet<int> WinningNumbers { get; }

    public int Copies { get; set; } = 1;

    public int Points { get; }

    public Card(string cardData)
    {
        var cardDataSplit = cardData.Split(": ");
        var allNumbers = cardDataSplit[1].Split("|");

        var winingNumbers = new HashSet<int>(allNumbers[0].GetNumbers<int>());
        var numbers = new HashSet<int>(allNumbers[1].GetNumbers<int>());
        WinningNumbers = numbers.Intersect(winingNumbers).ToHashSet();
        Points = (int)Math.Pow(2, WinningNumbers.Count - 1);
    }
}