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
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace CWDev.SLNTools.Core.Filter
{
    using Core;
    using Merge;

    internal class FilteredSolutionWatcher
    {
        private readonly AcceptDifferencesHandler r_acceptDifferencesHandler;
        private readonly FilterFile r_filterFile;
        private SolutionFile m_filteredSolution;
        private readonly FileSystemWatcher r_watcher;

        public FilteredSolutionWatcher(
                    AcceptDifferencesHandler handler,
                    FilterFile filterFile,
                    SolutionFile filteredSolution)
        {
            r_acceptDifferencesHandler = handler;
            r_filterFile = filterFile;
            m_filteredSolution = filteredSolution;

            r_watcher = new FileSystemWatcher
                {
                    NotifyFilter = NotifyFilters.LastWrite,
                    Path = Path.GetDirectoryName(m_filteredSolution.SolutionFullPath),
                    Filter = Path.GetFileName(m_filteredSolution.SolutionFullPath)
                };
            r_watcher.Changed += OnChanged;
        }

        public void Start()
        {
            lock (r_watcher)
            {
                r_watcher.EnableRaisingEvents = true;
            }
        }

        public void Stop()
        {
            lock (r_watcher)
            {
                r_watcher.EnableRaisingEvents = false;
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            lock (r_watcher)
            {
                try
                {
                    WaitForFileToBeReleased(e.FullPath);

                    var newFilteredSolution = SolutionFile.FromFile(m_filteredSolution.SolutionFullPath);
                    var difference = newFilteredSolution.CompareTo(m_filteredSolution);
                    if (difference != null)
                    {
                        difference.Remove(diff => diff.Identifier.Name.Contains("SccProjectTopLevelParentUniqueName"));
                        if (difference.Subdifferences.Count > 0)
                        {
                            if (r_acceptDifferencesHandler(difference))
                            {
                                var newOriginalSolution = SolutionFile.FromElement((NodeElement)r_filterFile.SourceSolution.ToElement().Apply(difference));
                                newOriginalSolution.Save();
                                m_filteredSolution = newFilteredSolution;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO Better handling of error, this assembly shouldn't display UI by itself
                    MessageBox.Show(ex.ToString(), "OnChanged handler");
                }
            }
        }

        private static void WaitForFileToBeReleased(string path)
        {
            if (!File.Exists(path))
                return;

            var start = DateTime.Now;
            do
            {
                try
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        return;
                    }
                }
                catch (IOException)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            } while (DateTime.Now - start < TimeSpan.FromSeconds(20));
        }
    }
}
