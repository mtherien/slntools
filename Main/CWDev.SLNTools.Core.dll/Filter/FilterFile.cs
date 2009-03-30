using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CWDev.SLNTools.Core.Filter
{
    public class FilterFile
    {
        public const string OriginalSolutionFolderGuid = "{3D86F2A1-6348-4351-9B53-2A75735A2AB4}";

        public static FilterFile FromFile(string filterFullPath)
        {
            FilterFile filterFile = new FilterFile();
            filterFile.FilterFullPath = filterFullPath;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(filterFullPath);

            XmlNode nodeSourceSln = xmldoc.SelectSingleNode("/Config/SourceSLN");
            filterFile.SourceSolutionFullPath = Path.Combine(Path.GetDirectoryName(filterFullPath), Path.GetFileName(nodeSourceSln.InnerText));

            foreach (XmlNode node in xmldoc.SelectNodes("/Config/ProjectToKeep"))
            {
                filterFile.ProjectsToKeep.Add(node.InnerText);
            }
            return filterFile;
        }

        public static FilterFile FromStream(string filterFullPath, Stream stream)
        {
            FilterFile filterFile = new FilterFile();
            filterFile.FilterFullPath = filterFullPath;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(stream);

            XmlNode nodeSourceSln = xmldoc.SelectSingleNode("/Config/SourceSLN");
            filterFile.SourceSolutionFullPath = Path.Combine(Path.GetDirectoryName(filterFullPath), Path.GetFileName(nodeSourceSln.InnerText));

            foreach (XmlNode node in xmldoc.SelectNodes("/Config/ProjectToKeep"))
            {
                filterFile.ProjectsToKeep.Add(node.InnerText);
            }
            return filterFile;
        }

        public FilterFile()
        {
            m_sourceSolutionFullPath = null;
            m_sourceSolution = null;
            m_filterFullPath = null;
            m_projectsToKeep = new List<string>();
        }

        private string m_sourceSolutionFullPath;
        private SolutionFile m_sourceSolution;
        private string m_filterFullPath;
        private List<string> m_projectsToKeep;

        public string SourceSolutionFullPath
        {
            get { return m_sourceSolutionFullPath; }
            set 
            { 
                m_sourceSolutionFullPath = value;
                ReloadSourceSolution();
            }
        }
        public SolutionFile SourceSolution
        {
            get { return m_sourceSolution; }
        }
        public void ReloadSourceSolution()
        {
            m_sourceSolution = SolutionFile.FromFile(m_sourceSolutionFullPath);
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
            SolutionFile filteredSolutionFile = new SolutionFile(original.SolutionFullPath, original.Headers, original.GlobalSections);
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

            string solutionName = Path.GetFileName(original.SolutionFullPath);
            filteredSolutionFile.AddOrUpdateProject(
                        OriginalSolutionFolderGuid,
                        KnownProjectTypeGuid.SolutionFolder,
                        "-OriginalSolution-",
                        "",
                        new ProjectSection[]
                        {
                            new ProjectSection(
                                "SolutionItems",
                                "ProjectSection",
                                "preProject",
                                new PropertyLine[] { new PropertyLine(solutionName, solutionName) })
                        },
                        null,
                        null);

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
            XmlNode nodeConfig = docFilter.CreateElement("Config");
            docFilter.AppendChild(nodeConfig);
            XmlNode nodeSourceSLN = docFilter.CreateElement("SourceSLN");
            nodeSourceSLN.InnerText = Path.GetFileName(m_sourceSolutionFullPath);
            nodeConfig.AppendChild(nodeSourceSLN);

            foreach (string projectFullName in this.ProjectsToKeep)
            {
                XmlNode node = docFilter.CreateElement("ProjectToKeep");
                node.InnerText = projectFullName;
                nodeConfig.AppendChild(node);
            }

            docFilter.Save(filterFullPath);
        }

        public SolutionFile SaveFilteredSolution()
        {
            SolutionFile filteredSolution = Apply();
            filteredSolution.SaveAs(this.DestinationSolutionFullPath);
            return filteredSolution;
        }

        public override bool Equals(object obj)
        {
            if (! base.Equals(obj))
                return false;

            FilterFile other = obj as FilterFile;
            if (this.FilterFullPath != other.FilterFullPath)
                return false;
            if (this.ProjectsToKeep.Count != other.ProjectsToKeep.Count)
                return false;
            // TODO
            return true;
        }
    }
}
