using Day1.ObjectRepository;
using System;
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
using UiPath.UIAutomationNext.API.Contracts;
using UiPath.UIAutomationNext.API.Models;
using UiPath.UIAutomationNext.Enums;

namespace Day1
{
    public class Day_1 : CodedWorkflow
    {
        [Workflow]
        public void Execute(bool debug)
        {            
            string path;
            //debug=true;
            if (debug.Equals(true))
            {
                path = "Test.txt";
            }
            else {
                path = "Input.txt";
            }
            var replaceText = new Dictionary<string,string>()
            {
                {"one","o1ne"},
                {"two","t2wo"},
                {"three","t3hree"},
                {"four","f4our"},
                {"five","f5ive"},
                {"six","s6ix"},
                {"seven","s7even"},
                {"eight","e8ight"},
                {"nine","n9ine"}
            };
            var text = System.IO.File.ReadAllText(path);
            var calVals = new List<int>();
            
            foreach (var n in replaceText)
            {
                text=text.Replace(n.Key,n.Value);
            }
            
            var splitText = text.Split(Environment.NewLine);
            
            foreach (var line in splitText)
            {
                int tempInt = 0;
                var firstVal = (from l in line where int.TryParse(l.ToString(),out tempInt) select l).First().ToString();
                var lastVal = (from l in line where int.TryParse(l.ToString(),out tempInt) select l).Last().ToString();
                calVals.Add(int.Parse(firstVal+lastVal));
            }
            Log($"{calVals.Sum()}");
        }
    }
}