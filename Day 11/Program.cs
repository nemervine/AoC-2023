using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;


var sw = Stopwatch.StartNew();
var debug = false;
//debug = true;
string path = debug ? "Test.txt" : "Input.txt";
var input = System.IO.File.ReadAllLines(path).ToList();
var expansionRate = 1;


//PrintMap(input);

var insertColumns = new List<int>();
for (int i = 0; i < input[0].Count(); i++)
    if (input.Select(x => x[i]).All(x => x.Equals('.')))
        insertColumns.Add(i);

var insertRows = new List<int>();
for (int i = 0; i < input.Count; i++)
    if (input[i].All(x => x.Equals('.')))
        insertRows.Add(i);

Console.WriteLine($"Init ({sw.ElapsedMilliseconds}ms)");
//PrintMap(input);
sw.Restart();
var galaxyList = new List<Galaxy>();
for (int i = 0; i < input.Count; i++)
    for (int j = 0; j < input[i].Length; j++)
        if (input[i][j] == '#')
            galaxyList.Add(new Galaxy(j + insertColumns.Where(x => x < j).Count() * expansionRate, i + insertRows.Where(x => x < i).Count() * expansionRate, galaxyList.Count + 1));


var galaxyPairs = new List<GalaxyPair>();
for (int i = 0; i < galaxyList.Count; i++)
{
    for (int j = i; j < galaxyList.Count; j++)
    {
        galaxyPairs.Add(new GalaxyPair(galaxyList[i], galaxyList[j]));
        //Console.WriteLine($"Between galaxy {galaxyList[i].Num} and galaxy {galaxyList[j].Num}: {galaxyPairs.Last().Distance}");
    }
}

Console.WriteLine($"Part 1 ({sw.ElapsedMilliseconds}ms): {galaxyPairs.Distinct().Sum(x => x.Distance)}");
sw.Restart();

expansionRate = 1000000 - 1;

galaxyList = new List<Galaxy>();
for (int i = 0; i < input.Count; i++)
    for (int j = 0; j < input[i].Length; j++)
        if (input[i][j] == '#')
            galaxyList.Add(new Galaxy(j + insertColumns.Where(x => x < j).Count() * expansionRate, i + insertRows.Where(x => x < i).Count() * expansionRate, galaxyList.Count + 1));


galaxyPairs = new List<GalaxyPair>();
for (int i = 0; i < galaxyList.Count; i++)
{
    for (int j = i; j < galaxyList.Count; j++)
    {
        galaxyPairs.Add(new GalaxyPair(galaxyList[i], galaxyList[j]));
        //Console.WriteLine($"Between galaxy {galaxyList[i].Num} and galaxy {galaxyList[j].Num}: {galaxyPairs.Last().Distance}");
    }
}

Console.WriteLine($"Part 2 ({sw.ElapsedMilliseconds}ms): {galaxyPairs.Distinct().Sum(x => x.Distance)}");

public class GalaxyPair
{
    public Galaxy First { get; set; }
    public Galaxy Second { get; set; }
    public long Distance { get; set; }

    public GalaxyPair(Galaxy first, Galaxy second)
    {
        First = first;
        Second = second;
        Distance = (first.X == second.X ? 0 : first.X > second.X ? first.X - second.X : second.X - first.X) + (first.Y == second.Y ? 0 : first.Y > second.Y ? first.Y - second.Y : second.Y - first.Y);
    }
}
public class Galaxy
{
    public long X { get; set; }
    public long Y { get; set; }
    public long Num { get; set; }
    public Galaxy(long X, long Y, long num)
    {
        this.X = X;
        this.Y = Y;
        this.Num = num;
    }
}