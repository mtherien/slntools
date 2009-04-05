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
    using Core.Merge;

    internal class FilteredSolutionWatcher
    {
        public FilteredSolutionWatcher(
                    AcceptDifferencesHandler handler,
                    FilterFile filterFile,
                    SolutionFile filteredSolution)
        {
            m_acceptDifferencesHandler = handler;
            m_filterFile = filterFile;
            m_filteredSolution = filteredSolution;

            m_watcher = new FileSystemWatcher();
            m_watcher.NotifyFilter = NotifyFilters.LastWrite;
            m_watcher.Path = Path.GetDirectoryName(m_filteredSolution.SolutionFullPath);
            m_watcher.Filter = Path.GetFileName(m_filteredSolution.SolutionFullPath);
            m_watcher.Changed += OnChanged;
        }

        private AcceptDifferencesHandler m_acceptDifferencesHandler;
        private FilterFile m_filterFile;
        private SolutionFile m_filteredSolution;
        private FileSystemWatcher m_watcher;

        public void Start()
        {
            lock (m_watcher)
            {
                m_watcher.EnableRaisingEvents = true;
            }
        }

        public void Stop()
        {
            lock (m_watcher)
            {
                m_watcher.EnableRaisingEvents = false;
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            lock (m_watcher)
            {
                try
                {
                    WaitForFileToBeReleased(e.FullPath);

                    SolutionFile newFilteredSolution = SolutionFile.FromFile(m_filteredSolution.SolutionFullPath);
                    NodeDifference difference = newFilteredSolution.CompareTo(m_filteredSolution);
                    if (difference != null)
                    {
                        difference.Remove(delegate(Difference diff)
                        {
                            return diff.Identifier.Name.Contains("SccProjectTopLevelParentUniqueName");
                        });
                        if (difference.Subdifferences.Count > 0)
                        {
                            if (m_acceptDifferencesHandler(difference.Subdifferences))
                            {
                                SolutionFile newOriginalSolution = new SolutionFile(m_filterFile.SourceSolution, difference.Subdifferences);
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

            DateTime start = DateTime.Now;
            do
            {
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
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
