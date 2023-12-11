using System.Text.RegularExpressions;
using System.Diagnostics;

var debug = false;
//debug = true;
string path = debug ? "Test.txt" : "Input.txt";
var input = System.IO.File.ReadAllLines(path).Select(x => Regex.Matches(x, "\\d+").Select(y => Convert.ToInt32(y.Value)).ToList()).ToList();

var startingSpeed = 0;
var buttonPower = 1;

var winWays = 1;
var possibilities = new List<int>();
for (int i = 0; i < input[0].Count; i++)
{
    possibilities = Enumerable.Range(0, input[0][i]).Where(x => (startingSpeed + (buttonPower * x) * (input[0][i] - x)) > input[1][i]).ToList();
    winWays = winWays * possibilities.Count;
}
Console.WriteLine(winWays);

var input2 = System.IO.File.ReadAllLines(path).Select(x => Regex.Matches(x, "\\d+").Select(y => y.Value).ToList()).Select(x => long.Parse(string.Join("", x))).ToArray();

var possibilities2 = Enumerable.Range(0, Convert.ToInt32(input2[0])).Where(x => (startingSpeed + (buttonPower * x) * (input2[0] - x)) > input2[1]).ToList();
Console.WriteLine(possibilities2.Count);