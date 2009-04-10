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
using System.Collections.ObjectModel;
using System.IO;

namespace CWDev.SLNTools.Core
{
    using Merge;

    public class SolutionFile
    {
        public static SolutionFile FromFile(string solutionFullPath)
        {
            using (SolutionFileReader reader = new SolutionFileReader(solutionFullPath))
            {
                SolutionFile f = reader.ReadSolutionFile();
                f.SolutionFullPath = solutionFullPath;
                return f;
            }
        }

        public static SolutionFile FromStream(string solutionFullPath, Stream stream)
        {
            using (SolutionFileReader reader = new SolutionFileReader(stream))
            {
                SolutionFile f = reader.ReadSolutionFile();
                f.SolutionFullPath = solutionFullPath;
                return f;
            }
        }

        public SolutionFile()
        {
            m_solutionFullPath = null;
            m_headers = new List<string>();
            m_projects = new ProjectHashList();
            m_globalSections = new SectionHashList<GlobalSection>();
        }

        public SolutionFile(SolutionFile original)
                    : this(original.SolutionFullPath, original.Headers, original.ProjectsInOrders, original.GlobalSections)
        {
        }

        public SolutionFile(string fullpath, IEnumerable<string> headers, IEnumerable<Project> projects, IEnumerable<GlobalSection> globalSections)
        {
            m_solutionFullPath = fullpath;
            m_headers = new List<string>(headers);
            m_projects = new ProjectHashList();
            foreach (Project project in projects)
            {
                AddOrUpdateProject(project);
            }
            m_globalSections = new SectionHashList<GlobalSection>();
            foreach (GlobalSection globalSection in globalSections)
            {
                AddOrUpdateGlobalSection(globalSection);
            }
        }

        private string m_solutionFullPath;
        private List<string> m_headers;
        private ProjectHashList m_projects;
        private SectionHashList<GlobalSection> m_globalSections;

        public string SolutionFullPath
        { 
            get { return m_solutionFullPath; }
            set { m_solutionFullPath = value; }
        }
        public string SolutionPath { get { return Path.GetDirectoryName(m_solutionFullPath); } }

        public ReadOnlyCollection<string> Headers { get { return m_headers.AsReadOnly(); } }

        public void AddHeaderLine(string line)
        {
            m_headers.Add(line);
        }

        public void RemoveAllHeaderLine()
        {
            m_headers.Clear();
        }

        public ReadOnlyCollection<Project> ProjectsInOrders { get { return m_projects.AsReadOnly(); } }

        public Project AddOrUpdateProject(Project newProject)
        {
            Project clone = new Project(this, newProject);
            m_projects.AddOrUpdate(clone);
            return clone;
        }
        public void RemoveProject(Project oldProject)
        {
            if (oldProject != null)
            {
                m_projects.Remove(oldProject);
            }
        }
        public void RemoveProjectByGuid(string guid)
        {
            RemoveProject(FindProjectByGuid(guid));
        }
        public void SortProjects()
        { 
            SortProjects(delegate(Project p1, Project p2)
                        {
                            return string.Compare(p1.ProjectFullName, p2.ProjectFullName);
                        });
        }
        public void SortProjects(Comparison<Project> comparer)
        {
            m_projects.Sort(comparer);
        }
        public Project FindProjectByGuid(string guid)
        {
            return (m_projects.Contains(guid)) ? m_projects[guid] : null;
        }
        public Project FindProjectByFullName(string projectFullName)
        {
            foreach (Project project in m_projects)
            {
                if (string.Compare(project.ProjectFullName, projectFullName, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return project;
                }
            }
            return null;
        }
        public IEnumerable<Project> Childs
        {
            get
            {
                foreach (Project project in m_projects)
                {
                    if (project.ParentFolder == null)
                    {
                        yield return project;
                    }
                }
            }
        }

        public ReadOnlyCollection<GlobalSection> GlobalSections { get { return m_globalSections.AsReadOnly(); } }

        public void AddOrUpdateGlobalSection(GlobalSection newGlobalSection)
        {
            m_globalSections.AddOrUpdate(newGlobalSection);
        }
        public void RemoveGlobalSection(GlobalSection oldGlobalSection)
        {
            m_globalSections.Remove(oldGlobalSection);
        }
        public void RemoveGlobalSectionByName(string name)
        {
            m_globalSections.Remove(FindGlobalSectionByName(name));
        }
        public GlobalSection FindGlobalSectionByName(string name)
        {
            return (m_globalSections.Contains(name)) ? m_globalSections[name] : null;
        }

        public void Save()
        {
            SaveAs(m_solutionFullPath);
        }

        public void SaveAs(string solutionPath)
        {
            using (SolutionFileWriter writer = new SolutionFileWriter(solutionPath))
            {
                writer.WriteSolutionFile(this);
            }
        }

        public NodeDifference CompareTo(SolutionFile oldSolution)
        {
            return (NodeDifference) this.ToElement().CompareTo(oldSolution.ToElement());
        }

        #region Methods ToElement / FromElement

        private const string TagHeader = "Header";
        private const string TagProject = "P_";
        private const string TagGlobalSection = "GS_";

        public NodeElement ToElement()
        {
            ElementHashList childs = new ElementHashList();
            childs.Add(
                        new ValueElement(
                            new ElementIdentifier(TagHeader),
                            String.Join("|", m_headers.ToArray())));

            foreach (Project project in this.ProjectsInOrders)
            {
                childs.Add(
                            project.ToElement(
                                new ElementIdentifier(
                                    TagProject + project.ProjectGuid,
                                    string.Format("Project \"{0}\"", project.ProjectFullName))));
            }
            foreach (GlobalSection globalSection in this.GlobalSections)
            {
                childs.Add(
                            globalSection.ToElement(
                                new ElementIdentifier(
                                    TagGlobalSection + globalSection.Name,
                                    string.Format("GlobalSection \"{0}\"", globalSection.Name))));
            }
            return new NodeElement(
                        new ElementIdentifier("SolutionFile"),
                        childs);
        }

        public static SolutionFile FromElement(NodeElement element)
        {
            string[] headers = new string[0];
            ProjectHashList projects = new ProjectHashList();
            SectionHashList<GlobalSection> globalSections = new SectionHashList<GlobalSection>();

            foreach (Element child in element.Childs)
            {
                ElementIdentifier identifier = child.Identifier;
                if (identifier.Name.StartsWith(TagHeader))
                {
                    headers = ((ValueElement)child).Value.Split('|');
                }
                else if (identifier.Name.StartsWith(TagProject))
                {
                    string projectGuid = identifier.Name.Substring(TagProject.Length);
                    projects.Add(Project.FromElement(projectGuid, (NodeElement)child));
                }
                else if (identifier.Name.StartsWith(TagGlobalSection))
                {
                    string sectionName = identifier.Name.Substring(TagGlobalSection.Length);
                    globalSections.Add(GlobalSection.FromElement(sectionName, (NodeElement)child));
                }
                else
                {
                    throw new Exception(string.Format("Invalid identifier '{0}'.", identifier.Name));
                }
            }
            return new SolutionFile(null, headers, projects, globalSections);
        }

        #endregion
    }
}
