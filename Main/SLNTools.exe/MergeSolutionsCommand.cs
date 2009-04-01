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

        public override void Run(string[] args)
        {
            Arguments parsedArguments = new Arguments();
            if (Parser.ParseArgumentsWithUsage(args, parsedArguments))
            {
                if (parsedArguments.Solutions.Length < 4)
                    throw new Exception("TODO");

                SolutionFile latestSolutionInSourceBranch = SolutionFile.FromFile(parsedArguments.Solutions[0]);
                SolutionFile latestSolutionInDestinationBranch = SolutionFile.FromFile(parsedArguments.Solutions[1]);
                SolutionFile rootSolution = SolutionFile.FromFile(parsedArguments.Solutions[2]);
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
