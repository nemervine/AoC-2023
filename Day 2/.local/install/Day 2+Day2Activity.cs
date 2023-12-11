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
using Day2.ObjectRepository;
using UiPath.Core;
using UiPath.Core.Activities.Storage;

namespace Day2
{
    public class Day2Activity : System.Activities.Activity
    {
        public InArgument<bool> debug { get; set; }

        public Day2Activity()
        {
            this.Implementation = () =>
            {
                return new Day2ActivityChild()
                {debug = (this.debug == null ? (InArgument<bool>)Argument.CreateReference((Argument)new InArgument<bool>(), "debug") : (InArgument<bool>)Argument.CreateReference((Argument)this.debug, "debug")), };
            };
        }
    }

    internal class Day2ActivityChild : CodeActivity
    {
        public InArgument<bool> debug { get; set; }

        public Day2ActivityChild()
        {
            DisplayName = "Day2";
        }

        protected override void Execute(CodeActivityContext context)
        {
            var codedWorkflow = new global::Day2.Day2();
            CodedWorkflowHelper.Initialize(codedWorkflow, context);
            CodedWorkflowHelper.RunWithExceptionHandling(() =>
            {
                if (codedWorkflow is IBeforeAfterRun codedWorkflowWithBeforeAfter)
                {
                    codedWorkflowWithBeforeAfter.Before(new BeforeRunContext()
                    {RelativeFilePath = "Day 2.cs"});
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
                    {RelativeFilePath = "Day 2.cs", Exception = exception});
                }
            });
        }
    }
}