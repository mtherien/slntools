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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using CWDev.SLNTools.CommandErrorReporters;
using CWDev.SLNTools.CommandLine;
using CWDev.SLNTools.Core;
using CWDev.SLNTools.Core.Filter;
using CWDev.SLNTools.Core.Merge;
using CWDev.SLNTools.UIKit;

namespace CWDev.SLNTools.Commands
{
    internal class OpenFilterFileCommand : Command
    {
        private class Arguments
        {
            [DefaultArgument(ArgumentType.Required | ArgumentType.AtMostOnce)]
            public string FilterFile = null;

            [Argument(ArgumentType.AtMostOnce)]
            public bool Wait = false;

            [Argument(ArgumentType.AtMostOnce)]
            public bool CreateOnly = false;
        }

        public override void Run(string[] args, ICommandUsageReporter commandUsageReporter)
        {
            var parsedArguments = new Arguments();
            commandUsageReporter.CommandUsage = Parser.ArgumentsUsage(parsedArguments.GetType());

            if (Parser.ParseArguments(args, parsedArguments, commandUsageReporter.ReportUsage))
            {
                var filterFile = FilterFile.FromFile(parsedArguments.FilterFile);

                // Save the filtered solution. We also add a link to the original solution file in the filtered solution.
                // If we have to checkout the original solution file later on, its easier with that link.
                var filteredSolution = filterFile.Apply();

                // Add OriginalSolutionFile to the filter solution (and a warnings file if needed)
                filteredSolution.Projects.Add(CreateOriginalSolutionProject(filterFile.SourceSolution));

                filteredSolution.Save();
                if (filterFile.CopyReSharperFiles)
                {
                    var resharperGlobalFileSettingsSource = filterFile.SourceSolutionFullPath + ".DotSettings";
                    if (File.Exists(resharperGlobalFileSettingsSource))
                    {
                        File.Copy(resharperGlobalFileSettingsSource, filteredSolution.SolutionFullPath + ".DotSettings", true);
                    }

                    var resharperUserFileSettingsSource = filterFile.SourceSolutionFullPath + ".DotSettings.user";
                    var resharperUserFileSettingsDestination = filteredSolution.SolutionFullPath + ".DotSettings.user";
                    if (File.Exists(resharperUserFileSettingsSource) && !File.Exists(resharperUserFileSettingsDestination))
                    {
                        File.Copy(resharperUserFileSettingsSource, resharperUserFileSettingsDestination);
                    }
                }

                if (!parsedArguments.CreateOnly)
                {
                    filterFile.StartFilteredSolutionWatcher(
                                filteredSolution,
                                delegate(NodeDifference difference)
                                {
                                    using (var fix = new TopMostFormFix())
                                    {
                                        using (var form = new UpdateOriginalSolutionForm(difference, filterFile.SourceSolutionFullPath))
                                        {
                                            return (form.ShowDialog() == DialogResult.Yes);
                                        }
                                    }
                                });

                    var startTime = DateTime.Now;
                    var process = Process.Start(filteredSolution.SolutionFullPath);

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
                            foreach (var processSpawned in ProcessEx.GetChildsOfProcess(process))
                            {
                                processSpawned.WaitForExit();
                            }
                        }
                    }

                    filterFile.StopFilteredSolutionWatcher();
                }
            }
        }

        public override string CommandLineUsage => Parser.ArgumentsUsage(typeof(Arguments));

        private static Project CreateOriginalSolutionProject(
                    SolutionFile sourceSolutionFile)
        {
            var originalSolutionName = Path.GetFileName(sourceSolutionFile.SolutionFullPath);

            var propertyLines = new List<PropertyLine>();
            propertyLines.Add(new PropertyLine(originalSolutionName, originalSolutionName));
            if (sourceSolutionFile.Warnings.Count > 0)
            {
                var warningFileName = originalSolutionName + ".warnings.txt";
                var warningFullPath = Path.Combine(Path.GetDirectoryName(sourceSolutionFile.SolutionFullPath) ?? ".", warningFileName);
                using (var output = File.CreateText(warningFullPath))
                {
                    output.WriteLine("The following warnings were found while parsing the file '{0}': ", originalSolutionName);
                    output.WriteLine();
                    foreach (var warning in sourceSolutionFile.Warnings)
                    {
                        output.WriteLine(warning);
                    }
                }

                propertyLines.Add(new PropertyLine(warningFileName, warningFileName));
            }

            return new Project(
                        null,
                        "{3D86F2A1-6348-4351-9B53-2A75735A2AB4}",
                        KnownProjectTypeGuid.SolutionFolder,
                        "-OriginalSolution-",
                        "-OriginalSolution-",
                        null,
                        new[]
                                {
                                    new Section(
                                        "SolutionItems",
                                        "ProjectSection",
                                        "preProject",
                                        propertyLines)
                                },
                        null,
                        null);
        }
    }
}
