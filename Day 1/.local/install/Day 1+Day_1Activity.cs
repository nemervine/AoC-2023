using System.Activities;
using UiPath.CodedWorkflows;
using UiPath.CodedWorkflows.Utils;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using UiPath.Core;
using UiPath.Core.Activities.Storage;
using UiPath.Orchestrator.Client.Models;
using UiPath.Testing;
using UiPath.Testing.Activities.TestData;
using UiPath.Testing.Activities.TestDataQueues.Enums;
using UiPath.Testing.Enums;

namespace Day1
{
    public class Day_1Activity : System.Activities.Activity
    {
        public InArgument<bool> debug { get; set; }

        public Day_1Activity()
        {
            this.Implementation = () =>
            {
                return new Day_1ActivityChild()
                {debug = (this.debug == null ? (InArgument<bool>)Argument.CreateReference((Argument)new InArgument<bool>(), "debug") : (InArgument<bool>)Argument.CreateReference((Argument)this.debug, "debug")), };
            };
        }
    }

    internal class Day_1ActivityChild : CodeActivity
    {
        public InArgument<bool> debug { get; set; }

        public Day_1ActivityChild()
        {
            DisplayName = "Day_1";
        }

        protected override void Execute(CodeActivityContext context)
        {
            var codedWorkflow = new global::Day1.Day_1();
            CodedWorkflowHelper.Initialize(codedWorkflow, context);
            CodedWorkflowHelper.RunWithExceptionHandling(() =>
            {
                if (codedWorkflow is IBeforeAfterRun codedWorkflowWithBeforeAfter)
                {
                    codedWorkflowWithBeforeAfter.Before(new BeforeRunContext()
                    {RelativeFilePath = "Day 1.cs"});
                }
            }, () =>
            {
                codedWorkflow.Execute(debug.Get(context));
                return null;
            }, (exception, outArgs) =>
            {
                if (codedWorkflow is IBeforeAfterRun codedWorkflowWithBeforeAfter)
                {
                    codedWorkflowWithBeforeAfter.After(new AfterRunContext()
                    {RelativeFilePath = "Day 1.cs", Exception = exception});
                }
            });
        }
    }
}