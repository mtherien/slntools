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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CWDev.SLNTools
{
    using CommandLine;
    using Core;
    using Core.Filter;
    using Core.Merge;
    using UIKit;

    internal class OpenFilterFileCommand : Command
    {
        private class Arguments
        {
            [DefaultArgument(ArgumentType.Required | ArgumentType.AtMostOnce)]
            public string FilterFile = null;

            [Argument(ArgumentType.AtMostOnce)]
            public bool Wait = false;
        }

        public override void Run(string[] args, MessageBoxErrorReporter reporter)
        {
            Arguments parsedArguments = new Arguments();
            reporter.CommandUsage = Parser.ArgumentsUsage(parsedArguments.GetType());

            if (Parser.ParseArguments(args, parsedArguments, reporter.Handler))
            {
                FilterFile filterFile = FilterFile.FromFile(parsedArguments.FilterFile);

                // Save the filtered solution. We also add a link to the original solution file in the filtered solution.
                // If we have to checkout the original solution file later on, its easier with that link.
                SolutionFile filteredSolution = filterFile.Apply();
                Project originalSolutionProject = CreateOriginalSolutionProject(filterFile.SourceSolutionFullPath);
                filteredSolution.Projects.Add(originalSolutionProject);
                filteredSolution.Save();

                filterFile.StartFilteredSolutionWatcher(
                            filteredSolution,
                            delegate(ReadOnlyCollection<Difference> differences)
                            {
                                using (TopMostFormFix fix = new TopMostFormFix())
                                {
                                    using (UpdateOriginalSolutionForm form = new UpdateOriginalSolutionForm(differences, filterFile.SourceSolutionFullPath))
                                    {
                                        return (form.ShowDialog() == DialogResult.Yes);
                                    }
                                }
                            });

                DateTime startTime = DateTime.Now;
                Process process = Process.Start(filteredSolution.SolutionFullPath);

                if (parsedArguments.Wait || filterFile.WatchForChangesOnFilteredSolution)
                {
                    process.WaitForExit();

                    // If the process exited "too fast", we wait on the processes that were spawned by the process 
                    // we started. This allow us to handle the case where the '.sln' is associated to an application like 
                    // "VSLauncher.exe". That type of application only live for a short period of time because it's job 
                    // is to analyse the sln file, launch the right version of "devenv.exe" (i.e. VS2002, VS2005, VS2008) 
                    // and then exit. 
                    // This "trick" should not be needed with others IDE like SharpDevelop.
                    if (DateTime.Now - startTime < TimeSpan.FromMinutes(1))
                    {
                        foreach (Process processSpawned in ProcessEx.GetChildsOfProcess(process))
                        {
                            processSpawned.WaitForExit();
                        }
                    }
                }

                filterFile.StopFilteredSolutionWatcher();
            }
        }

        private static Project CreateOriginalSolutionProject(
                    string originalSolutionFullPath)
        {
            string originalSolutionName = Path.GetFileName(originalSolutionFullPath);
            Project project = new Project(
                        null,
                        "{3D86F2A1-6348-4351-9B53-2A75735A2AB4}", 
                        KnownProjectTypeGuid.SolutionFolder,
                        "-OriginalSolution-",
                        "-OriginalSolution-",
                        null, 
                        new Section[]
                                {
                                    new Section(
                                        "SolutionItems",
                                        "ProjectSection",
                                        "preProject",
                                        new PropertyLine[] { new PropertyLine(originalSolutionName, originalSolutionName) })
                                },
                        null,
                        null);
            return project;
        }
    }
}
