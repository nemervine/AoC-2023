using System.Activities;
using UiPath.CodedWorkflows;
using UiPath.CodedWorkflows.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UiPath.Core;
using UiPath.Core.Activities.Storage;
using UiPath.Orchestrator.Client.Models;
using UiPath.Testing;
using UiPath.Testing.Activities.TestData;
using UiPath.Testing.Activities.TestDataQueues.Enums;
using UiPath.Testing.Enums;

namespace Day3
{
    public class Day3Activity : System.Activities.Activity
    {
        public InArgument<bool> debug { get; set; }

        public Day3Activity()
        {
            this.Implementation = () =>
            {
                return new Day3ActivityChild()
                {debug = (this.debug == null ? (InArgument<bool>)Argument.CreateReference((Argument)new InArgument<bool>(), "debug") : (InArgument<bool>)Argument.CreateReference((Argument)this.debug, "debug")), };
            };
        }
    }

    internal class Day3ActivityChild : CodeActivity
    {
        public InArgument<bool> debug { get; set; }

        public Day3ActivityChild()
        {
            DisplayName = "Day3";
        }

        protected override void Execute(CodeActivityContext context)
        {
            var codedWorkflow = new global::Day3.Day3();
            CodedWorkflowHelper.Initialize(codedWorkflow, context);
            CodedWorkflowHelper.RunWithExceptionHandling(() =>
            {
                if (codedWorkflow is IBeforeAfterRun codedWorkflowWithBeforeAfter)
                {
                    codedWorkflowWithBeforeAfter.Before(new BeforeRunContext()
                    {RelativeFilePath = "Day 3.cs"});
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
                    {RelativeFilePath = "Day 3.cs", Exception = exception});
                }
            });
        }
    }
}