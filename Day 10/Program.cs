using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

var sw = Stopwatch.StartNew();
var debug = false;
//debug = true;
string path = debug ? "Test.txt" : "Input.txt";
var input = System.IO.File.ReadAllLines(path).ToList();
Console.WriteLine($"Init ({sw.ElapsedMilliseconds}ms)");
//sw.Restart();

var pipes = new Tile[input.Count, input[0].Length];
Tile tempAnimal = new Tile(0, 0, '.');

for (int i = 0; i < input.Count; i++)
    for (int j = 0; j < input[i].Length; j++)
    {
        pipes[j, i] = new Tile(j, i, input[i][j]);
        if (input[i][j] == 'S')
            tempAnimal = pipes[j, i];
    }

tempAnimal.North = tempAnimal.Y > 0 ? pipes[tempAnimal.X, tempAnimal.Y - 1].South : false;
tempAnimal.South = tempAnimal.Y < input.Count ? pipes[tempAnimal.X, tempAnimal.Y + 1].North : false;
tempAnimal.East = tempAnimal.X < input[0].Length ? pipes[tempAnimal.X + 1, tempAnimal.Y].West : false;
tempAnimal.West = tempAnimal.X > 0 ? pipes[tempAnimal.X - 1, tempAnimal.Y].East : false;

if (tempAnimal.North)
{
    if (tempAnimal.South)
        tempAnimal.Shape = '│';
    else if (tempAnimal.West)
        tempAnimal.Shape = '┘';
    else if (tempAnimal.East)
        tempAnimal.Shape = '└';
}
else if (tempAnimal.South)
{
    if (tempAnimal.West)
        tempAnimal.Shape = '┐';
    else if (tempAnimal.East)
        tempAnimal.Shape = '┌';
}
else if (tempAnimal.West && tempAnimal.East)
    tempAnimal.Shape = '─';
pipes[tempAnimal.X, tempAnimal.Y] = tempAnimal;
Console.WriteLine($"Parse ({sw.ElapsedMilliseconds}ms)");

var loop = new List<Tile> { pipes[tempAnimal.X, tempAnimal.Y] };
for (int i = 0; i < loop.Count; i++)
{
    if (loop[i].East && pipes[loop[i].X + 1, loop[i].Y].InLoop == false)
    {
        pipes[loop[i].X + 1, loop[i].Y].InLoop = true;
        loop.Add(pipes[loop[i].X + 1, loop[i].Y]);
    }
    else if (loop[i].South && pipes[loop[i].X, loop[i].Y + 1].InLoop == false)
    {
        pipes[loop[i].X, loop[i].Y + 1].InLoop = true;
        loop.Add(pipes[loop[i].X, loop[i].Y + 1]);
    }
    else if (loop[i].West && pipes[loop[i].X - 1, loop[i].Y].InLoop == false)
    {
        pipes[loop[i].X - 1, loop[i].Y].InLoop = true;
        loop.Add(pipes[loop[i].X - 1, loop[i].Y]);
    }
    else if (loop[i].North && pipes[loop[i].X, loop[i].Y - 1].InLoop == false)
    {
        pipes[loop[i].X, loop[i].Y - 1].InLoop = true;
        loop.Add(pipes[loop[i].X, loop[i].Y - 1]);
    }
}

Console.WriteLine($"Part 1 ({sw.ElapsedMilliseconds}ms): {loop.Count / 2}");

var inside = new List<Tile>();

for (int i = 1; i < loop.Count; i++)
{
    switch (loop[i].Shape)
    {
        case '│':
            if (loop[i].Y < loop[i - 1].Y)
                if (pipes[loop[i].X + 1, loop[i].Y].InLoop == false)
                    inside.Add(pipes[loop[i].X + 1, loop[i].Y]);
            if (loop[i].Y > loop[i - 1].Y)
                if (pipes[loop[i].X - 1, loop[i].Y].InLoop == false)
                    inside.Add(pipes[loop[i].X - 1, loop[i].Y]);
            break;
        case '─':
            if (loop[i].X < loop[i - 1].X)
                if (pipes[loop[i].X, loop[i].Y - 1].InLoop == false)
                    inside.Add(pipes[loop[i].X, loop[i].Y - 1]);
            if (loop[i].X > loop[i - 1].X)
                if (pipes[loop[i].X, loop[i].Y + 1].InLoop == false)
                    inside.Add(pipes[loop[i].X, loop[i].Y + 1]);
            break;
        case '└':
            if (loop[i].X < loop[i - 1].X && loop[i].Y == loop[i - 1].Y)
                if (pipes[loop[i].X + 1, loop[i].Y - 1].InLoop == false)
                    inside.Add(pipes[loop[i].X + 1, loop[i].Y - 1]);
            if (loop[i].Y > loop[i - 1].Y && loop[i].X == loop[i - 1].X)
            {
                if (pipes[loop[i].X - 1, loop[i].Y].InLoop == false)
                    inside.Add(pipes[loop[i].X - 1, loop[i].Y]);
                if (pipes[loop[i].X, loop[i].Y + 1].InLoop == false)
                    inside.Add(pipes[loop[i].X, loop[i].Y + 1]);
                if (pipes[loop[i].X - 1, loop[i].Y + 1].InLoop == false)
                    inside.Add(pipes[loop[i].X - 1, loop[i].Y + 1]);
            }
            break;
        case '┘':
            if (loop[i].Y > loop[i - 1].Y && loop[i].X == loop[i - 1].X)
                if (pipes[loop[i].X - 1, loop[i].Y - 1].InLoop == false)
                    inside.Add(pipes[loop[i].X - 1, loop[i].Y - 1]);
            if (loop[i].X > loop[i - 1].X && loop[i].Y == loop[i - 1].Y)
            {
                if (pipes[loop[i].X + 1, loop[i].Y].InLoop == false)
                    inside.Add(pipes[loop[i].X + 1, loop[i].Y]);
                if (pipes[loop[i].X, loop[i].Y + 1].InLoop == false)
                    inside.Add(pipes[loop[i].X, loop[i].Y + 1]);
                if (pipes[loop[i].X + 1, loop[i].Y + 1].InLoop == false)
                    inside.Add(pipes[loop[i].X + 1, loop[i].Y + 1]);
            }
            break;
        case '┐':
            if (loop[i].X > loop[i - 1].X && loop[i].Y == loop[i - 1].Y)
                if (pipes[loop[i].X - 1, loop[i].Y + 1].InLoop == false)
                    inside.Add(pipes[loop[i].X - 1, loop[i].Y + 1]);
            if (loop[i].Y < loop[i - 1].Y && loop[i].X == loop[i - 1].X)
            {
                if (pipes[loop[i].X + 1, loop[i].Y].InLoop == false)
                    inside.Add(pipes[loop[i].X + 1, loop[i].Y]);
                if (pipes[loop[i].X, loop[i].Y - 1].InLoop == false)
                    inside.Add(pipes[loop[i].X, loop[i].Y - 1]);
                if (pipes[loop[i].X + 1, loop[i].Y - 1].InLoop == false)
                    inside.Add(pipes[loop[i].X + 1, loop[i].Y - 1]);
            }
            break;
        case '┌':
            if (loop[i].Y > loop[i - 1].Y && loop[i].X == loop[i - 1].X)
                if (pipes[loop[i].X + 1, loop[i].Y + 1].InLoop == false)
                    inside.Add(pipes[loop[i].X + 1, loop[i].Y + 1]);
            if (loop[i].X < loop[i - 1].X && loop[i].Y == loop[i - 1].Y)
            {
                if (pipes[loop[i].X - 1, loop[i].Y].InLoop == false)
                    inside.Add(pipes[loop[i].X - 1, loop[i].Y]);
                if (pipes[loop[i].X, loop[i].Y - 1].InLoop == false)
                    inside.Add(pipes[loop[i].X, loop[i].Y - 1]);
                if (pipes[loop[i].X - 1, loop[i].Y - 1].InLoop == false)
                    inside.Add(pipes[loop[i].X - 1, loop[i].Y - 1]);
            }
            break;
    }
}

for (int i = 0; i < inside.Count; i++)
{
    if (pipes[inside[i].X + 1, inside[i].Y].InLoop == false && inside.Contains(pipes[inside[i].X + 1, inside[i].Y]) == false)
        inside.Add(pipes[inside[i].X + 1, inside[i].Y]);
    if (pipes[inside[i].X - 1, inside[i].Y].InLoop == false && inside.Contains(pipes[inside[i].X - 1, inside[i].Y]) == false)
        inside.Add(pipes[inside[i].X - 1, inside[i].Y]);
    if (pipes[inside[i].X, inside[i].Y - 1].InLoop == false && inside.Contains(pipes[inside[i].X, inside[i].Y - 1]) == false)
        inside.Add(pipes[inside[i].X, inside[i].Y - 1]);
    if (pipes[inside[i].X, inside[i].Y + 1].InLoop == false && inside.Contains(pipes[inside[i].X, inside[i].Y + 1]) == false)
        inside.Add(pipes[inside[i].X, inside[i].Y + 1]);
}

Console.WriteLine($"Part 2 ({sw.ElapsedMilliseconds}ms): {inside.AsParallel().Distinct().Count()}");

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool North { get; set; }
    public bool South { get; set; }
    public bool West { get; set; }
    public bool East { get; set; }
    public bool InLoop { get; set; }
    public char Shape { get; set; }

    public Tile(int x, int y, char shape)
    {
        X = x;
        Y = y;
        switch (shape)
        {
            case '|':
                North = true;
                South = true;
                Shape = '│';
                break;
            case '-':
                East = true;
                West = true;
                Shape = '─';
                break;
            case 'L':
                North = true;
                East = true;
                Shape = '└';
                break;
            case 'J':
                North = true;
                West = true;
                Shape = '┘';
                break;
            case '7':
                South = true;
                West = true;
                Shape = '┐';
                break;
            case 'F':
                South = true;
                East = true;
                Shape = '┌';
                break;
            case 'S':
                InLoop = true;
                break;
            case '.':
                Shape = '.';
                break;
        }
    }
}
