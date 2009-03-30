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

    internal class OpenFilterFileArguments
    {
        [DefaultArgument(ArgumentType.Required | ArgumentType.AtMostOnce)]
        public string FilterFile = null;

        [Argument(ArgumentType.AtMostOnce)]
        public bool Wait = false;
    }

    internal class OpenFilterFileCommand : Command
    {
        public override void Run(string[] args)
        {
            OpenFilterFileArguments parsedArguments = new OpenFilterFileArguments();
            if (Parser.ParseArgumentsWithUsage(args, parsedArguments))
            {
                FilterFile filterFile = FilterFile.FromFile(parsedArguments.FilterFile);

                // Save the filtered solution. We also add a link to the original solution file in the filtered solution.
                // If we have to checkout the original solution file later on, its easier with that link.
                SolutionFile filteredSolution = filterFile.Apply();
                Project originalSolutionProject = CreateOriginalSolutionProject(filterFile.SourceSolutionFullPath);
                filteredSolution.AddOrUpdateProject(originalSolutionProject);
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
                        new ProjectSection[]
                                {
                                    new ProjectSection(
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
