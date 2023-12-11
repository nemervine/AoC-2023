using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

long GCD(long big, long small)
{
    long temp = small;
    long temp2;
    do
    {
        temp2 = big - temp * (Math.DivRem(big, temp).Quotient);
        if ((temp2 > 0) && (temp2 < temp))
        {
            big = temp;
            temp = temp2;
        }
    } while (temp2 != 0 && temp2 > 0);
    return temp;
}

long LCM(long big, long small)
{
    var gcd = GCD(big, small);
    //Console.WriteLine($"GCD of {big} and {small} is {gcd}");
    return ((big * small) / gcd);
}

var debug = false;
//debug = true;
string path = debug ? "Test.txt" : "Input.txt";
var input = System.IO.File.ReadAllLines(path).ToList();

var moves = input[0].ToList();
var nodes = input.AsParallel().Where(x => Regex.Match(x, "^\\w\\w\\w\\W").Success).Select(x => new Node(Regex.Match(x, "^\\w\\w\\w").Value, input.IndexOf(x))).ToList();
var i = 0;

for (i = 0; i < nodes.Count; i++)
{
    //Debug.WriteLine(i);
    var nameL = Regex.Matches(input[nodes[i].Index], "\\w\\w\\w")[1].Value;
    var nameR = Regex.Matches(input[nodes[i].Index], "\\w\\w\\w")[2].Value;
    nodes[i].L = nodes.Where(y => y.Name.Equals(nameL)).Single();
    nodes[i].R = nodes.Where(y => y.Name.Equals(nameR)).Single();
}

var startNode = nodes.AsParallel().Where(x => x.Name.Equals("AAA")).Single();
var endNode = nodes.AsParallel().Where(x => x.Name.Equals("ZZZ")).Single();
var currentNode = startNode;

i = 0;
int step = 0;
while (currentNode != endNode)
{
    //Debug.WriteLine("Current Move: " + moves[i] + " Current Node: " + currentNode.Name);
    if (moves[i] == 'L')
        currentNode = currentNode.L;
    else if (moves[i] == 'R')
        currentNode = currentNode.R;
    step++;
    if (i == (moves.Count - 1))
        i = 0;
    else
        i++;
}

Console.WriteLine("Part 1: " + step);
Console.WriteLine();

var currentNodes = nodes.AsParallel().Where(x => x.Name.EndsWith('A')).ToList();
var stepsList = new List<int> { };

for (var j = 0; j < currentNodes.Count; j++)
{
    i = 0;
    step = 0;
    while (currentNodes[j].Name.EndsWith('Z') == false)
    {
        step++;
        if (moves[i] == 'L')
            currentNodes[j] = currentNodes[j].L;
        else if (moves[i] == 'R')
            currentNodes[j] = currentNodes[j].R;
        if (i == (moves.Count - 1))
            i = 0;
        else
            i++;
    }
    stepsList.Add(step);
    Console.WriteLine(currentNodes[j].Name + ": " + step);
}

long lcm = stepsList[0];
for (i = 1; i < stepsList.Count; i++)
{
    if (lcm > stepsList[i])
        lcm = LCM(lcm, stepsList[i]);
    else
        lcm = LCM(stepsList[i], lcm);
}

Console.WriteLine("Part 2: " + lcm);

public class Node
{
    public string Name { get; set; }
    public int Index { get; set; }
    public Node? L { get; set; }
    public Node? R { get; set; }

    public Node(string name, int index)
    {
        this.Name = name;
        this.Index = index;
    }

}