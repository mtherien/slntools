#region License

// SLNTools
// Copyright (c) 2009 
// by Christian Warren
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

#endregion

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

        public override void Run(string[] args, MessageBoxErrorReporter reporter)
        {
            Arguments parsedArguments = new Arguments();
            reporter.CommandUsage = Parser.ArgumentsUsage(parsedArguments.GetType());

            if (Parser.ParseArguments(args, parsedArguments, reporter.Handler))
            {
                if (parsedArguments.Solutions.Length < 4)
                {
                    reporter.Handler("Four solution files should be provided, in order:\n   SourceBranch.sln\n   DestinationBranch.sln\n   CommonAncestror.sln\n   Result.sln");
                    return;
                }

                SolutionFile solutionInSourceBranch = SolutionFile.FromFile(parsedArguments.Solutions[0]);
                SolutionFile solutionInDestinationBranch = SolutionFile.FromFile(parsedArguments.Solutions[1]);
                SolutionFile commonAncestrorSolution = SolutionFile.FromFile(parsedArguments.Solutions[2]);
                string mergedSolutionName = parsedArguments.Solutions[3];

                NodeElement elementInSourceBranch = solutionInSourceBranch.ToElement();
                NodeElement elementInDestinationBranch = solutionInDestinationBranch.ToElement();
                NodeElement commonAncestrorElement = commonAncestrorSolution.ToElement();

                NodeDifference differenceInSourceBranch;
                NodeDifference differenceInDestinationBranch;
                NodeConflict conflict = Conflict.Merge(
                                commonAncestrorElement,
                                elementInSourceBranch,
                                elementInDestinationBranch,
                                out differenceInSourceBranch,
                                out differenceInDestinationBranch);

                using (MergeSolutionsForm form = new MergeSolutionsForm(
                            differenceInSourceBranch, 
                            differenceInDestinationBranch, 
                            conflict,
                            delegate(ConflictContext context, Difference differenceTypeInSourceBranch, Difference differenceTypeInDestinationBranch)
                            {
                                OperationTypeConflictResolverForm resolverForm = new OperationTypeConflictResolverForm(
                                            context,
                                            differenceTypeInSourceBranch,
                                            differenceTypeInDestinationBranch);
                                resolverForm.ShowDialog();
                                return resolverForm.Result;
                            },
                            delegate(ConflictContext context, string valueInSourceBranch, string valueInDestinationBranch)
                            {
                                ValueConflictResolverForm resolverForm = new ValueConflictResolverForm(
                                            context,
                                            valueInSourceBranch,
                                            valueInDestinationBranch);
                                resolverForm.ShowDialog();
                                return resolverForm.Result;
                            }))
                {
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        NodeElement mergedElement = (NodeElement)commonAncestrorElement.Apply(form.Result);
                        SolutionFile mergedSolution = SolutionFile.FromElement(mergedElement);
                        mergedSolution.SaveAs(mergedSolutionName);
                    }
                }
            }
        }
    }
}
