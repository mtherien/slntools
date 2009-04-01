using System;
using System.Collections.Generic;

namespace CWDev.SLNTools
{
    using CommandLine;
    using Core;
    using Core.Merge;
    using UIKit;

    internal class MergeSolutionsCommand : Command
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
                if (parsedArguments.Solutions.Length < 4)
                    throw new Exception("TODO");

                SolutionFile rootSolution = SolutionFile.FromFile(parsedArguments.Solutions[0]);
                SolutionFile latestSolutionInSourceBranch = SolutionFile.FromFile(parsedArguments.Solutions[1]);
                SolutionFile latestSolutionInDestinationBranch = SolutionFile.FromFile(parsedArguments.Solutions[2]);
                string mergedSolutionName = parsedArguments.Solutions[3];

                NodeDifference differenceInSourceBranch;
                NodeDifference differenceInDestinationBranch;
                NodeConflict conflict = Conflict.Merge(
                                rootSolution.GetElement(), 
                                latestSolutionInSourceBranch.GetElement(), 
                                latestSolutionInDestinationBranch.GetElement(),
                                out differenceInSourceBranch,
                                out differenceInDestinationBranch);

                using (MergeSolutionsForm form = new MergeSolutionsForm(
                            differenceInSourceBranch, 
                            differenceInDestinationBranch, 
                            conflict,
                            delegate(ConflictContext context, Difference differenceTypeInSourceBranch, Difference differenceTypeInDestinationBranch)
                            {
                                TypeDifferenceConflictResolverForm resolverForm = new TypeDifferenceConflictResolverForm(
                                            context,
                                            differenceTypeInSourceBranch,
                                            differenceTypeInDestinationBranch);
                                resolverForm.ShowDialog();
                                return resolverForm.Result;
                            },
                            delegate(ConflictContext context, string latestValueInSourceBranch, string latestValueInDestinationBranch)
                            {
                                ValueConflictResolverForm resolverForm = new ValueConflictResolverForm(
                                            context,
                                            latestValueInSourceBranch,
                                            latestValueInDestinationBranch);
                                resolverForm.ShowDialog();
                                return resolverForm.Result;
                            }))
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SolutionFile mergedSolution = new SolutionFile(rootSolution, form.Result);
                        mergedSolution.SaveAs(mergedSolutionName);
                    }
                }
            }
        }
    }
}
