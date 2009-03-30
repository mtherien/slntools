using System;
using System.IO;
using System.Threading;

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
                    Console.WriteLine("TODO " + ex.ToString());
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
