using System.Linq;
using System.Diagnostics;

var cards = new List<char> { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };
cards.Reverse();
var handTypes = new Dictionary<string, int> { { "5", 0 }, { "4", 1 }, { "F", 2 }, { "3", 3 }, { "2P", 4 }, { "1P", 5 }, { "H", 6 } };

int GetHandTypes(string hand)
{
    if (hand.Contains("J") && (hand.Distinct().Count() > 1))
    {
        var r = hand.Replace("J", "").OrderByDescending(x => hand.Count(y => x == y)).ThenByDescending(x => cards.IndexOf(x)).First();
        hand = hand.Replace('J', r);
    }
    switch (hand.Distinct().Count())
    {
        case 1:
            return handTypes["5"];
        case 2:
            if (hand.Distinct().Any(x => hand.Where(y => x.Equals(y)).Count().Equals(4)))
                return handTypes["4"];
            else
                return handTypes["F"];
        case 3:
            if (hand.Distinct().Any(x => hand.Where(y => x.Equals(y)).Count().Equals(3)))
                return handTypes["3"];
            else
                return handTypes["2P"];
        case 4:
            return handTypes["1P"];
        case 5:
            return handTypes["H"];
        default:
            return 0;
    }
}

var debug = false;
//debug = true;
string path = debug ? "Test.txt" : "Input.txt";
var input = System.IO.File.ReadAllLines(path);


var hands = input.AsParallel().Select(x => new Hand(x.Split(" ")[0].Trim(), int.Parse(x.Split(" ")[1].Trim()))).AsParallel()
    .Select(x => new Hand(x.Cards, x.Bid, GetHandTypes(x.Cards)))
    .OrderByDescending(hand => hand.HandType)
    .ThenBy(hand => cards.IndexOf(hand.Cards[0]))
    .ThenBy(hand => cards.IndexOf(hand.Cards[1]))
    .ThenBy(hand => cards.IndexOf(hand.Cards[2]))
    .ThenBy(hand => cards.IndexOf(hand.Cards[3]))
    .ThenBy(hand => cards.IndexOf(hand.Cards[4]))
    .ToList();

Console.WriteLine(hands.Sum(x => x.Bid * (1 + hands.IndexOf(x))));

public record Hand
{
    public string Cards { get; set; }
    public int Bid { get; set; }
    public int? HandType { get; set; }

    public Hand(string cards, int bid)
    {
        Cards = cards;
        Bid = bid;
    }
    public Hand(string cards, int bid, int handType)
    {
        Cards = cards;
        Bid = bid;
        HandType = handType;
    }
}