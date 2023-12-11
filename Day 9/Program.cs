using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

var debug = false;
//debug = true;
string path = debug ? "Test.txt" : "Input.txt";
var input = System.IO.File.ReadAllLines(path).ToList();
var history = input.Select(x => x.Split(" ").Select(y => long.Parse(y.Trim())).ToList()).ToList();

for (int i = 0; i < history.Count; i++)
{
    var sequences = new List<List<long>> { history[i] };

    for (int j = 0; j < sequences.Count; j++)
    {
        sequences.Add(sequences[j].Zip(sequences[j].Skip(1), (x, y) => y - x).ToList());
        if (sequences.Last().All(x => x == 0))
            break;
    }

    sequences.Last().Add(0);
    sequences.Last().Insert(0, 0);

    for (int j = sequences.Count - 2; j >= 0; j--)
    {
        sequences[j].Add(sequences[j].Last() + sequences[j + 1].Last());
        sequences[j].Insert(0, sequences[j].First() - sequences[j + 1].First());
    }

    history[i] = sequences[0];
}

Console.WriteLine("Part 1: " + history.Sum(x => x.Last()));
Console.WriteLine("Part 2: " + history.Sum(x => x.First()));