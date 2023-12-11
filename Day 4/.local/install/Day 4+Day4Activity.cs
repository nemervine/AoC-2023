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

namespace Day4
{
    public class Day4Activity : System.Activities.Activity
    {
        public InArgument<bool> debug { get; set; }

        public Day4Activity()
        {
            this.Implementation = () =>
            {
                return new Day4ActivityChild()
                {debug = (this.debug == null ? (InArgument<bool>)Argument.CreateReference((Argument)new InArgument<bool>(), "debug") : (InArgument<bool>)Argument.CreateReference((Argument)this.debug, "debug")), };
            };
        }
    }

    internal class Day4ActivityChild : CodeActivity
    {
        public InArgument<bool> debug { get; set; }

        public Day4ActivityChild()
        {
            DisplayName = "Day4";
        }

        protected override void Execute(CodeActivityContext context)
        {
            var codedWorkflow = new global::Day4.Day4();
            CodedWorkflowHelper.Initialize(codedWorkflow, context);
            CodedWorkflowHelper.RunWithExceptionHandling(() =>
            {
                if (codedWorkflow is IBeforeAfterRun codedWorkflowWithBeforeAfter)
                {
                    codedWorkflowWithBeforeAfter.Before(new BeforeRunContext()
                    {RelativeFilePath = "Day 4.cs"});
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
                    {RelativeFilePath = "Day 4.cs", Exception = exception});
                }
            });
        }
    }
}