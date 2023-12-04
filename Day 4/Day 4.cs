using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UiPath.CodedWorkflows;

namespace Day4
{
    public class Day4 : CodedWorkflow
    {
        public struct Card
        {
            public int cardNum { get; set; }
            public List<int> winningNums { get; set; }
            public List<int> haveNums { get; set; }
            public int matchCount { get; set; }

            public Card(string line)
            {
                cardNum = int.Parse(Regex.Matches(line, "\\d+").First().Value);
                winningNums = Regex.Matches(line.Split(": ")[1].Split("|")[0], "\\d+").Select(x => int.Parse(x.Value)).ToList();
                haveNums = Regex.Matches(line.Split(": ")[1].Split("|")[1], "\\d+").Select(x => int.Parse(x.Value)).ToList();
                matchCount = this.winningNums.Intersect(this.haveNums).Count();
            }
        }

        [Workflow]
        public void Execute(bool debug)
        {
            //debug = true;
            string path = debug ? "Test.txt" : "Input.txt";

            var input = System.IO.File.ReadAllLines(path).ToList();

            var cardsList = input.Select(x => new Card(x)).ToList();
            Log(cardsList.Where(x => x.matchCount > 0).Sum(x => Math.Pow(2, x.matchCount - 1)).ToString());

            var workingList = cardsList;
            int i = 0;
            while (i < workingList.Count())
            {
                if (workingList[i].matchCount > 0)
                {
                    for (int j = 1; j <= workingList[i].matchCount; j++)
                    {
                        workingList.Add(cardsList[workingList[i].cardNum + j - 1]);
                    }
                }
                i++;
            }
            Log(workingList.Count().ToString());
        }
    }
}