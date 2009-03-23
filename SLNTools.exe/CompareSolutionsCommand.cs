using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CWDev.VSSolutionTools
{
    using CommandLine;
    using Core;
    using Core.Merge;
    using UIKit;

    internal class CompareSolutionsCommand : Command
    {
        private class Arguments
        {
            [DefaultArgument(ArgumentType.MultipleUnique)]
            public string[] Solutions = null;
        }

        public override void Run(string[] args)
        {
            Arguments parsedArguments = new Arguments();
            if (Parser.ParseArgumentsWithUsage(args, parsedArguments))
            {
                if (parsedArguments.Solutions.Length < 2)
                    throw new Exception("TODO");

                SolutionFile rootSolution = SolutionFile.FromFile(parsedArguments.Solutions[0]);
                SolutionFile latestSolution = SolutionFile.FromFile(parsedArguments.Solutions[1]);
                Difference difference = latestSolution.CompareTo(rootSolution);
                if (difference == null)
                {
                    difference = new NodeDifference(new ElementIdentifier(""), OperationOnParent.Modified, new List<Difference>());
                }
                using (CompareSolutionsForm form = new CompareSolutionsForm(difference))
                {
                    form.ShowDialog();
                }
            }
        }
    }
}
