using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
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
        [Argument(ArgumentType.Multiple)]
        public string[] SolutionStarter = null;

        [DefaultArgument(ArgumentType.Required | ArgumentType.AtMostOnce)]
        public string FilterFile = null;
    }

    internal class OpenFilterFileCommand : Command
    {
        public override void Run(string[] args)
        {
            OpenFilterFileArguments parsedArguments = new OpenFilterFileArguments();
            if (Parser.ParseArgumentsWithUsage(args, parsedArguments))
            {
                m_filterFile = FilterFile.FromFile(parsedArguments.FilterFile);
                m_solutionStarter = ParseSolutionStarter(parsedArguments.SolutionStarter);

                m_filteredSolution = m_filterFile.SaveFilteredSolution();

                m_watcher = new FileSystemWatcher();
                m_watcher.NotifyFilter = NotifyFilters.LastWrite;
                m_watcher.Path = Path.GetDirectoryName(m_filterFile.FilterFullPath);
                m_watcher.Filter = "*.*";
                m_watcher.Changed += OnChanged;

                m_watcher.EnableRaisingEvents = true;

                SolutionStarter starter = null;
                foreach (string header in m_filteredSolution.Headers)
                {
                    if (m_solutionStarter.TryGetValue(header, out starter))
                        break;
                }

                Process process;
                if (starter == null)
                {
                    process = Process.Start(m_filterFile.DestinationSolutionFullPath);
                }
                else 
                {
                    process = starter.StartProcess(m_filterFile.DestinationSolutionFullPath);
                }
                process.WaitForExit();
            }
        }

        private static readonly string ms_patternStarter = "^(?<LINEINHEADER>[^|]*)\\|(?<STARTINGAPPLICATION>[^|]*)(\\|(?<ARGUMENTS>[^|]*))?$";
        private static readonly Regex ms_regexStarter = new Regex(ms_patternStarter);

        private class SolutionStarter
        {
            public SolutionStarter(string application, string arguments)
            {
                m_application = application;
                m_arguments = arguments;
            }

            private string m_application;
            private string m_arguments;

            public Process StartProcess(string solutionFullPath)
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = Environment.ExpandEnvironmentVariables(m_application);
                info.Arguments = CreateArguments(solutionFullPath);
                return Process.Start(info);
            }

            private string CreateArguments(string solutionFullPath)
            {
                if (m_arguments == null)
                {
                    return string.Format("\"{0}\"", solutionFullPath);
                }
                else
                {
                    string modifiedArguments = m_arguments.Replace("{SolutionFullPath}", solutionFullPath);
                    if (modifiedArguments == m_arguments)
                    {
                        throw new Exception("TODO");
                    }
                    return modifiedArguments;
                }
            }
        }

        private Dictionary<string, SolutionStarter> ParseSolutionStarter(string[] starters)
        {
            Dictionary<string, SolutionStarter> dict = new Dictionary<string, SolutionStarter>(StringComparer.OrdinalIgnoreCase);
            if (starters == null)
                return dict;

            foreach (string starter in starters)
            {
                Match match = ms_regexStarter.Match(starter);
                if (!match.Success)
                {
                    throw new IOException("TODO.");
                }
                string lineInHeader = match.Groups["LINEINHEADER"].Value;
                string application = match.Groups["STARTINGAPPLICATION"].Value;
                string arguments = (match.Groups["ARGUMENTS"].Success) ? match.Groups["ARGUMENTS"].Value : null;

                dict[lineInHeader] = new SolutionStarter(application, arguments);
            }

            // Add the default starters
            dict["Microsoft Visual Studio Solution File, Format Version 9.00"] = new SolutionStarter(
                            @"%ProgramFiles%\Microsoft Visual Studio 8\Common7\IDE\devenv.exe", 
                            null);
            dict["Microsoft Visual Studio Solution File, Format Version 10.00"] = new SolutionStarter(
                            @"%ProgramFiles%\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe",
                            null);
            return dict;
         }

        private FilterFile m_filterFile;
        private SolutionFile m_filteredSolution;
        private Dictionary<string, SolutionStarter> m_solutionStarter;
        private FileSystemWatcher m_watcher;

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);

            using (Form topmostForm = new Form())
            {
                // We do not want anyone to see this window so position it off the 
                // visible screen and make it as small as possible

                topmostForm.Size = new System.Drawing.Size(1, 1);
                topmostForm.StartPosition = FormStartPosition.Manual;
                topmostForm.ShowInTaskbar = false;
                System.Drawing.Rectangle rect = SystemInformation.VirtualScreen;
                topmostForm.Location = new Point(rect.Bottom + 10, rect.Right + 10);
                topmostForm.Show();
                // Make this form the active form and make it TopMost

                topmostForm.Focus();
                topmostForm.BringToFront();
                topmostForm.TopMost = true;
                // Finally show the MessageBox with the form just created as its owner

                //if (e.FullPath == m_filterFile.FilterFullPath)
                //{
                //    FilterFile newFilterFile = FilterFile.FromStream(e.FullPath, fs);
                //    if (!newFilterFile.Equals(m_filterFile))
                //    {
                //        if (MessageBox.Show("The filter file has been modified. Do you want to reapply it?", "SLNTools", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //        {
                //            m_filterFile = newFilterFile;
                //            m_filteredSolution = m_filterFile.SaveFilteredSolution();
                //        }
                //    }
                //}
                //else if (e.FullPath == m_filterFile.SourceSolutionFullPath)
                //{
                //    if (MessageBox.Show("The source solution file has been modified. Do you want to reapply the filter on the new version?", "SLNTools", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //    {
                //        m_filterFile.ReloadSourceSolution();
                //        m_filteredSolution = m_filterFile.SaveFilteredSolution();
                //    }
                //}
                //else 
                if (string.Compare(e.FullPath, m_filterFile.DestinationSolutionFullPath, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    using (FileStream fs = ForcedOpen(e.FullPath))
                    {
                        SolutionFile newSolutionFile = SolutionFile.FromStream(e.FullPath, fs);
                        m_filteredSolution.RemoveProjectByGuid(FilterFile.OriginalSolutionFolderGuid);
                        newSolutionFile.RemoveProjectByGuid(FilterFile.OriginalSolutionFolderGuid);

                        NodeDifference difference = newSolutionFile.CompareTo(m_filteredSolution);
                        difference.Remove(delegate(Difference diff)
                        {
                            return diff.Identifier.Name.Contains("SccProjectTopLevelParentUniqueName");
                        });

                        if ((difference != null) || (difference.Subdifferences.Count == 0))
                        {
                            using (UpdateOriginalSolutionForm form = new UpdateOriginalSolutionForm(difference.Subdifferences, m_filterFile.SourceSolutionFullPath))
                            {
                                if (form.ShowDialog() == DialogResult.Yes)
                                {
                                    SolutionFile newOriginalSolution = new SolutionFile(m_filterFile.SourceSolution, ((NodeDifference)difference).Subdifferences);
                                    newOriginalSolution.Save();
                                    m_filterFile.ReloadSourceSolution();
                                    m_filteredSolution = newSolutionFile;
                                }
                            }
                        }
                    }
                }
            }
        }

        // return FileStream when open was successful, otherwise null
        private static FileStream ForcedOpen(string path)
        {
            FileStream fs = null;
            int tried = 0;
            while (fs == null)
            {
                try
                {
                    Console.WriteLine("Try #{0} to open '{1}'...", ++tried, path);
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                    Console.WriteLine("FileOpened");
                }
                catch (Exception)
                {
                    // Ignore Error
                }
            }

            return fs;
        }
    }
}
