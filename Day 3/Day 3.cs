using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UiPath.CodedWorkflows;
using UiPath.Core;
using UiPath.Core.Activities.Storage;
using UiPath.Orchestrator.Client.Models;
using UiPath.Testing;
using UiPath.Testing.Activities.TestData;
using UiPath.Testing.Activities.TestDataQueues.Enums;
using UiPath.Testing.Enums;

namespace Day3
{
    public class Day3 : CodedWorkflow
    {
        record struct Part
        {
            public int Number { get; set; }
            public string Symbol { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public Part(int number, string symbol, int x, int y)
            {
                Number = number;
                Symbol = symbol;
                X = x;
                Y = y;
            }
        }
        record struct Gear
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Gear(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public bool CheckSymbol(List<string> symbols, List<string> input, int x, int y)
        {
            try
            {
                if (symbols.Contains(input[y].Substring(x, 1)))
                    return true;
            }
            catch (Exception e)
            { }
            return false;
        }

        [Workflow]
        public void Execute(bool debug)
        {
            string path;
            //debug = true;
            if (debug.Equals(true))
                path = "Test.txt";
            else
                path = "Input.txt";

            var input = System.IO.File.ReadAllLines(path).ToList();
            var symbols = Regex.Matches(System.IO.File.ReadAllText(path), "[^.1234567890\\n\\r]").Select(x => x.Value).Distinct().ToList();
            var checkPosition = new List<int[]> {
                new int[] {-1,-1}, new int[] {-1,0}, new int[] {-1,1},
                new int[] {0,-1},                    new int[] {0,1},
                new int[] {1,-1},  new int[] {1,0},  new int[] {1,1}
            };
            var partList = new List<Part> { };

            foreach (var line in input)
            {
                foreach (Match m in Regex.Matches(line, "\\d+"))
                {
                    var touchSymbol = false;
                    for (int i = 0; i < m.Length; i++)
                    {
                        foreach (var pos in checkPosition)
                        {
                            if (CheckSymbol(symbols, input, m.Index + i + pos[0], input.IndexOf(line) + pos[1]))
                            {
                                touchSymbol = true;
                                partList.Add(new Part(int.Parse(m.Value), input[input.IndexOf(line) + pos[1]].Substring(m.Index + i + pos[0], 1), m.Index + i + pos[0], input.IndexOf(line) + pos[1]));
                                break;
                            }
                        }
                        if (touchSymbol)
                            break;
                    }
                }
            }

            Log(partList.Sum(p => p.Number).ToString()); //Part 1

            var gearList = partList.Where(p => p.Symbol.Equals("*")).Select(g => new Gear(g.X, g.Y)).Distinct().Where(g => partList.Where(p => p.X.Equals(g.X) && p.Y.Equals(g.Y)).Count().Equals(2)).ToList();
            var gearSum = gearList.Sum(g => partList.Where(p => p.X.Equals(g.X) && p.Y.Equals(g.Y)).Select(n => n.Number).First() * partList.Where(p => p.X.Equals(g.X) && p.Y.Equals(g.Y)).Select(n => n.Number).Last());

            Log(gearSum.ToString()); //Part 2
        }
    }
}