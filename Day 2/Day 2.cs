using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Day2.ObjectRepository;
using UiPath.CodedWorkflows;
using UiPath.Core;
using UiPath.Core.Activities.Storage;

namespace Day2
{
    public class Day2 : CodedWorkflow
    {
        public bool RoundPossible(List<int> roundInput, List<int> maxCubes)
        {
            return roundInput.All(x => (roundInput[roundInput.IndexOf(x)] <= maxCubes[roundInput.IndexOf(x)]));
        }

        public bool GamePossible(List<List<int>> gameInput, List<int> maxCubes)
        {
            return gameInput.All(x => RoundPossible(x, maxCubes));
        }

        public int GetCubes(string color, string input)
        {
            return (Regex.Match(input.ToLower(), "\\d+\\s" + color).Success ? int.Parse(Regex.Match(input.ToLower(), "\\d+\\s" + color).Value.Split(" ")[0].Trim()) : 0);
        }

        [Workflow]
        public void Execute(bool debug)
        {
            string path;
            //debug = true;
            if (debug.Equals(true))
            {
                path = "Test.txt";
            }
            else
            {
                path = "Input.txt";
            }

            var maxCubes = new List<int> { 12, 13, 14 };

            var input = System.IO.File.ReadAllLines(path);

            var gameList = input.Select(x => x.Split(": ")[1].Split(';').Select(y => new List<int> { GetCubes("red", y), GetCubes("green", y), GetCubes("blue", y) }).ToList()).ToList();

            //Part 1
            int idSum = gameList.Where(x => GamePossible(x, maxCubes)).Sum(x => gameList.IndexOf(x) + 1);
            Log(idSum.ToString());

            //Part 2
            int powerSum = gameList.Select(x => x.Max(y => y[0]) * x.Max(y => y[1]) * x.Max(y => y[2])).Sum(x => x);
            Log(powerSum.ToString());
        }
    }
}