// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.Serialization;

long Map(long seed, List<List<AlmanacRange>> maps)
{
    var temp = seed;
    for (int j = 0; j < maps.Count(); j++)
    {
        for (int k = 0; k < maps[j].Count(); k++)
        {
            if ((maps[j][k].Destination <= temp) && (temp <= (maps[j][k].Destination + maps[j][k].Length - 1)))
            {
                temp = (temp - maps[j][k].Destination) + maps[j][k].Source;
                break;
            }
        }
    }
    return temp;
}
long UnMap(long seed, List<List<AlmanacRange>> maps)
{
    var temp = seed;
    for (int j = maps.Count - 1; j >= 0; j--)
    {
        for (int k = 0; k < maps[j].Count(); k++)
        {
            if ((maps[j][k].Source <= temp) && (temp <= (maps[j][k].Source + maps[j][k].Length - 1)))
            {
                temp = (temp - maps[j][k].Source) + maps[j][k].Destination;
                break;
            }
        }
    }
    return temp;
}

var debug = false;
debug = true;
string path = debug ? "Test.txt" : "Input.txt";
var input = System.IO.File.ReadAllText(path);
var mappings = new List<string>
            {
                "seed-to-soil map:",
                "soil-to-fertilizer map:",
                "fertilizer-to-water map:",
                "water-to-light map:",
                "light-to-temperature map:",
                "temperature-to-humidity map:",
                "humidity-to-location map:"
            };

var seeds = Regex.Matches(Regex.Match(input, "seeds\\:\\s(\\d+\\W*)*").Value, "\\d+\\s\\d+\\s").Select(x => Regex.Matches(x.Value, "\\d+").Select(y => long.Parse(y.Value)).ToArray()).OrderBy(x => x[0]).ToList();

var maps = new List<List<AlmanacRange>>(7);
for (var j = 0; j < mappings.Count(); j++)
{
    maps.Add(new List<AlmanacRange> { });
    foreach (var s in Regex.Matches(Regex.Match(input, "(" + mappings[j] + "\\s)(((\\d*\\s*){3})+)").Value, "(\\s\\d+){3}").Select(x => Regex.Matches(x.Value, "\\d+").Select(y => long.Parse(y.Value)).ToArray()).ToList())
    {
        maps[j].Add(new AlmanacRange(s[1], s[0], s[2]));
    }
    maps[j] = maps[j].OrderBy(x => x.Source).ToList();
}


var seedsToCheck = new List<long> { };
for (var i = 0; i < seeds.Count; i++)
{
    seedsToCheck.Add(seeds[i][0]);
    seedsToCheck.Add(seeds[i][0] + seeds[i][1]);
}

for (var i = 0; i < maps.Count; i++)
{
    for (var j = 0; j < maps[i].Count; j++)
    {
        for (var k = 0; k < seeds.Count; k++)
        {
            //if (seeds[k])
            //seedsToCheck.AddRange(Enumerable.Range(0, Convert.ToInt32(maps[i][j].Length)).Select(x => maps[i][j].Destination + x).Where(x => seeds.Where(y => (y[0] <= UnMap(x, maps)) && (UnMap(x, maps) < y[0] + y[1])).Any()).Select(x => x));
        }
    }
}

Console.WriteLine($"{seedsToCheck.Count} seeds to check");

var locationList = new List<long>(seedsToCheck.Count);
var start = DateTime.Now;
for (var i = 0; i < seedsToCheck.Count; i++)
{
    Debug.WriteLine($"Checking {seedsToCheck[i]}");
    Debug.WriteLine($"Location {Map(seedsToCheck[i], maps)}");
    locationList.Add(Map(seedsToCheck[i], maps));
}
Console.WriteLine(locationList.Where(x => seeds.Where(y => (y[0] <= UnMap(x, maps)) && (UnMap(x, maps) < y[0] + y[1])).Any()).Min());
var end = DateTime.Now;

Console.WriteLine((end - start).TotalMilliseconds);

public struct AlmanacRange
{
    public long Source { get; set; }
    public long Destination { get; set; }
    public long Length { get; set; }

    public AlmanacRange(long source, long destination, long length)
    {
        Source = source;
        Destination = destination;
        Length = length;
    }
}