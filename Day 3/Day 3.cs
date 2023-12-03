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
            public int SymbolX { get; set; }
            public int SymbolY { get; set; }

            public Part(int number, string symbol, int symbolx, int symboly)
            {
                Number = number;
                Symbol = symbol;
                SymbolX = symbolx;
                SymbolY = symboly;
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
            var input = System.IO.File.ReadAllLines(path).ToList();
            var symbols = Regex.Matches(System.IO.File.ReadAllText(path), "[^.1234567890\\n\\r]").Select(x => x.Value).Distinct().ToList();
            var partNums = new List<int> { };
            var partList = new List<Part> { };

            foreach (var line in input)
            {
                var matches = Regex.Matches(line, "\\d+");
                foreach (Match m in matches)
                {
                    var touchSymbol = false;
                    for (int i = 0; i < m.Length; i++)
                    {
                        try
                        {
                            if (symbols.Contains(line.Substring(m.Index + i - 1, 1)))  //check left
                            {
                                touchSymbol = true;
                                partList.Add(new Part(int.Parse(m.Value), line.Substring(m.Index + i - 1, 1), m.Index + i - 1, input.IndexOf(line)));
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        try
                        {
                            if (symbols.Contains(line.Substring(m.Index + i + 1, 1)))  //check right
                            {
                                touchSymbol = true;
                                partList.Add(new Part(int.Parse(m.Value), line.Substring(m.Index + i + 1, 1), m.Index + i + 1, input.IndexOf(line)));
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        try
                        {
                            if (symbols.Contains(input[input.IndexOf(line) - 1].Substring(m.Index + i, 1)))  //check up
                            {
                                touchSymbol = true;
                                partList.Add(new Part(int.Parse(m.Value), input[input.IndexOf(line) - 1].Substring(m.Index + i, 1), m.Index + i, input.IndexOf(line) - 1));
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        try
                        {
                            if (symbols.Contains(input[input.IndexOf(line) - 1].Substring(m.Index + i - 1, 1)))  //check up left
                            {
                                touchSymbol = true;
                                partList.Add(new Part(int.Parse(m.Value), input[input.IndexOf(line) - 1].Substring(m.Index + i - 1, 1), m.Index + i - 1, input.IndexOf(line) - 1));
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        try
                        {
                            if (symbols.Contains(input[input.IndexOf(line) - 1].Substring(m.Index + i + 1, 1)))  //check up right
                            {
                                touchSymbol = true;
                                partList.Add(new Part(int.Parse(m.Value), input[input.IndexOf(line) - 1].Substring(m.Index + i + 1, 1), m.Index + i + 1, input.IndexOf(line) - 1));
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        try
                        {
                            if (symbols.Contains(input[input.IndexOf(line) + 1].Substring(m.Index + i, 1)))  //check down
                            {
                                touchSymbol = true;
                                partList.Add(new Part(int.Parse(m.Value), input[input.IndexOf(line) + 1].Substring(m.Index + i, 1), m.Index + i, input.IndexOf(line) + 1));
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        try
                        {
                            if (symbols.Contains(input[input.IndexOf(line) + 1].Substring(m.Index + i - 1, 1)))  //check down left
                            {
                                touchSymbol = true;
                                partList.Add(new Part(int.Parse(m.Value), input[input.IndexOf(line) + 1].Substring(m.Index + i - 1, 1), m.Index + i - 1, input.IndexOf(line) + 1));
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        try
                        {
                            if (symbols.Contains(input[input.IndexOf(line) + 1].Substring(m.Index + i + 1, 1)))  //check down right
                            {
                                touchSymbol = true;
                                partList.Add(new Part(int.Parse(m.Value), input[input.IndexOf(line) + 1].Substring(m.Index + i + 1, 1), m.Index + i + 1, input.IndexOf(line) + 1));
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    if (touchSymbol)
                        partNums.Add(int.Parse(m.Value));
                }
            }
            Log(partNums.Sum().ToString());

            var gearList = new List<Gear> { };
            foreach (var part in partList.Where(x => x.Symbol.Equals("*")))
            {
                gearList.Add(new Gear(part.SymbolX, part.SymbolY));
            }
            var gearSum = 0;
            foreach (var gear in gearList.Distinct())
            {
                if (partList.Where(p => p.SymbolX.Equals(gear.X) && p.SymbolY.Equals(gear.Y)).Count().Equals(2))
                    gearSum = gearSum + (partList.Where(p => p.SymbolX.Equals(gear.X) && p.SymbolY.Equals(gear.Y)).Select(p => p.Number).First() * partList.Where(p => p.SymbolX.Equals(gear.X) && p.SymbolY.Equals(gear.Y)).Select(p => p.Number).Last());
            }
            Log(gearSum.ToString());
        }
    }
}