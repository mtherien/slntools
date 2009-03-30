using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CWDev.SLNTools.Core.Filter
{
    public class FilterFile
    {
        public static FilterFile FromFile(string filterFullPath)
        {
            return FromStream(filterFullPath, new FileStream(filterFullPath, FileMode.Open));
        }

        public static FilterFile FromStream(string filterFullPath, Stream stream)
        {
            FilterFile filterFile = new FilterFile();
            filterFile.FilterFullPath = filterFullPath;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(stream);

            XmlNode configNode = xmldoc.SelectSingleNode("Config");

            XmlNode sourceSlnNode = configNode.SelectSingleNode("SourceSLN");
            filterFile.SourceSolutionFullPath = Path.Combine(
                        Path.GetDirectoryName(filterFullPath), 
                        Path.GetFileName(sourceSlnNode.InnerText));

            XmlNode watchForChangesNode = configNode.SelectSingleNode("WatchForChangesOnFilteredSolution");
            if (watchForChangesNode != null)
            {
                filterFile.WatchForChangesOnFilteredSolution = bool.Parse(watchForChangesNode.InnerText);
            }

            foreach (XmlNode node in configNode.SelectNodes("ProjectToKeep"))
            {
                filterFile.ProjectsToKeep.Add(node.InnerText);
            }

            return filterFile;
        }

        public FilterFile()
        {
            m_sourceSolutionFullPath = null;
            m_filterFullPath = null;
            m_projectsToKeep = new List<string>();
            m_watchForChangesOnFilteredSolution = false;
        }

        private string m_sourceSolutionFullPath;
        private string m_filterFullPath;
        private List<string> m_projectsToKeep;
        private bool m_watchForChangesOnFilteredSolution;

        public string SourceSolutionFullPath
        {
            get { return m_sourceSolutionFullPath; }
            set { m_sourceSolutionFullPath = value; }
        }

        public SolutionFile SourceSolution
        {
            get { return SolutionFile.FromFile(m_sourceSolutionFullPath); }
        }

        public string FilterFullPath
        {
            get { return m_filterFullPath; }
            set { m_filterFullPath = value; }
        }

        public string DestinationSolutionFullPath
        {
            get { return Path.ChangeExtension(this.FilterFullPath, ".sln"); }
        }

        public bool WatchForChangesOnFilteredSolution
        {
            get { return m_watchForChangesOnFilteredSolution; }
            set { m_watchForChangesOnFilteredSolution = value; }
        }

        public List<string> ProjectsToKeep
        {
            get { return m_projectsToKeep; }
        }

        public SolutionFile Apply()
        {
            return ApplyOn(this.SourceSolution);
        }

        public SolutionFile ApplyOn(SolutionFile original)
        {
            SolutionFile filteredSolutionFile = new SolutionFile(
                        this.DestinationSolutionFullPath, 
                        original.Headers, 
                        original.GlobalSections);

            List<Project> includedProjects = new List<Project>();
            foreach (string projectFullName in this.ProjectsToKeep)
            {
                Project projectToKeep = original.FindProjectByFullName(projectFullName);
                AddRecursiveDependenciesToList(includedProjects, projectToKeep);
                foreach (Project descendant in projectToKeep.AllDescendants)
                {
                    AddRecursiveDependenciesToList(includedProjects, descendant);
                }
            }
            foreach (Project project in includedProjects)
            {
                filteredSolutionFile.AddOrUpdateProject(project);
            }

            return filteredSolutionFile;
        }

        private void AddRecursiveDependenciesToList(List<Project> includedProjects, Project project)
        {
            if (includedProjects.Contains(project))
                return;

            includedProjects.Add(project);
            foreach (Project dependency in project.Dependencies)
            {
                AddRecursiveDependenciesToList(includedProjects, dependency);
            }
        }

        public void Save()
        {
            SaveAs(this.FilterFullPath);
        }

        public void SaveAs(string filterFullPath)
        {
            XmlDocument docFilter = new XmlDocument();

            XmlNode configNode = docFilter.CreateElement("Config");
            docFilter.AppendChild(configNode);

            XmlNode sourceSlnNode = docFilter.CreateElement("SourceSLN");
            sourceSlnNode.InnerText = Path.GetFileName(m_sourceSolutionFullPath);
            configNode.AppendChild(sourceSlnNode);

            XmlNode watchForChangesNode = docFilter.CreateElement("WatchForChangesOnFilteredSolution");
            watchForChangesNode.InnerText = m_watchForChangesOnFilteredSolution.ToString();
            configNode.AppendChild(watchForChangesNode);

            foreach (string projectFullName in this.ProjectsToKeep)
            {
                XmlNode node = docFilter.CreateElement("ProjectToKeep");
                node.InnerText = projectFullName;
                configNode.AppendChild(node);
            }

            docFilter.Save(filterFullPath);
        }
    }
}
