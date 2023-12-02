using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using UiPath.CodedWorkflows;
using UiPath.Core;
using UiPath.Core.Activities.Storage;
using UiPath.Orchestrator.Client.Models;
using UiPath.Testing;
using UiPath.Testing.Activities.TestData;
using UiPath.Testing.Activities.TestDataQueues.Enums;
using UiPath.Testing.Enums;

namespace Day1
{
    public class Day_1 : CodedWorkflow
    {
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
            var replaceText = new Dictionary<string, string>()
            {
                {"one","1"},
                {"two","2"},
                {"three","3"},
                {"four","4"},
                {"five","5"},
                {"six","6"},
                {"seven","7"},
                {"eight","8"},
                {"nine","9"}
            };
            var text = System.IO.File.ReadAllLines(path);
            var cleanText = new List<string>();
            var calVals = new List<int>();
            
            foreach (var n in text)
            {
                string cleanLine = "";
                int i = 0;
                while (i < n.Length)
                {
                    int tempNum;
                    if (int.TryParse(n.Substring(i, 1), out tempNum))
                    {
                        cleanLine = cleanLine + tempNum.ToString();
                        break;
                    }
                    else if ((from r in replaceText where n.Substring(i).StartsWith(r.Key) select r.Key).Any())
                    {
                        cleanLine = cleanLine + int.Parse((from r in replaceText where n.Substring(i).StartsWith(r.Key) select r.Value).First());
                        break;
                    }
                    i++;
                }
                
                i=1;
                while (i <= n.Length)
                {
                    int tempNum;
                    if (int.TryParse(n.Substring(n.Length-i, 1), out tempNum))
                    {
                        cleanLine = cleanLine + tempNum.ToString();
                        break;
                    }
                    else if ((from r in replaceText where n.Substring(n.Length-i).StartsWith(r.Key) select r.Key).Any())
                    {
                        cleanLine = cleanLine + int.Parse((from r in replaceText where n.Substring(n.Length-i).StartsWith(r.Key) select r.Value).First());
                        break;
                    }
                    i++;
                }
                
                cleanText.Add(cleanLine);
                //Log(String.Format("{0} -> {1}", n, cleanText.Last()));
            }
            
            //Log(String.Join(Environment.NewLine, cleanText));

            foreach (var line in cleanText)
            {
                int tempInt = 0;
                var firstVal = (from l in line where int.TryParse(l.ToString(), out tempInt) select l).First().ToString();
                var lastVal = (from l in line where int.TryParse(l.ToString(), out tempInt) select l).Last().ToString();
                calVals.Add(int.Parse(firstVal + lastVal));
            }
            Log($"{calVals.Sum()}");
            
            var tmp = cleanText.Select(str => String.Join("",Regex.Matches(str.Trim(),"\\d",RegexOptions.IgnoreCase).Select(x => x.Value))).Aggregate(0, (int sum,string x) => sum + int.Parse(x.Substring(0,1)+x.Substring(x.Length-1,1)));
            Log(tmp.ToString());
        }
    }
}