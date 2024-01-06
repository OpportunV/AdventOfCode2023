namespace AdventOfCode2023.Models.Day7;

public class Hand
{
    public string Cards { get; }

    public int Bid { get; }

    public CombinationType CombinationType { get; }

    public CombinationType AdvancedCombinationType { get; }

    public Hand(string cards, int bid)
    {
        Cards = cards;
        Bid = bid;
        CombinationType = GetCardsType();
        AdvancedCombinationType = GetCardsAdvancedType();
    }
    
    private CombinationType GetCardsType()
    {
        var noDuplicates = new HashSet<char>(Cards);
        if (noDuplicates.Count == 5)
        {
            return CombinationType.HighCard;
        }

        if (noDuplicates.Count == 4)
        {
            return CombinationType.Pair;
        }

        var sameCardInHand = noDuplicates.Select(card => Cards.Count(chr => chr == card)).Max();
        if (noDuplicates.Count == 3)
        {
            return sameCardInHand == 2 ? CombinationType.TwiPair : CombinationType.Set;
        }

        if (noDuplicates.Count == 2)
        {
            return sameCardInHand == 3 ? CombinationType.FullHouse : CombinationType.Four;
        }

        return CombinationType.Five;
    }

    private CombinationType GetCardsAdvancedType()
    {
        var jokersCount = Cards.Count(chr => chr == 'J');
        if (jokersCount == 0)
        {
            return GetCardsType();
        }

        if (jokersCount == 5)
        {
            return CombinationType.Five;
        }

        var newCards = Cards.Replace("J", string.Empty);
        var noDuplicates = new HashSet<char>(newCards);
        if (noDuplicates.Count == 4)
        {
            return CombinationType.Pair;
        }

        if (noDuplicates.Count == 3)
        {
            return CombinationType.Set;
        }

        var sameCardInHand = noDuplicates.Select(card => newCards.Count(chr => chr == card)).Max();
        if (noDuplicates.Count == 2)
        {
            return jokersCount switch
            {
                1 => sameCardInHand == 2 ? CombinationType.FullHouse : CombinationType.Four,
                2 => CombinationType.Four,
                3 => CombinationType.Four,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        if (noDuplicates.Count == 1)
        {
            return CombinationType.Five;
        }

        return CombinationType.Five;
    }
}