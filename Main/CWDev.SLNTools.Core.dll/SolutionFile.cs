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
            m_projectsInOrder = new List<Project>();
            m_projectsByGuid = new Dictionary<string, Project>(StringComparer.OrdinalIgnoreCase);
            m_globalSections = new List<GlobalSection>();
        }

        public SolutionFile(SolutionFile original)
                    : this(original.SolutionFullPath, original.Headers, original.GlobalSections)
        {
            foreach (Project project in original.ProjectsInOrders)
            {
                AddOrUpdateProject(project);
            }
        }

        public SolutionFile(string fullpath, ReadOnlyCollection<string> headers, IEnumerable<GlobalSection> globalSections)
        {
            m_solutionFullPath = fullpath;
            m_headers = new List<string>(headers);
            m_projectsInOrder = new List<Project>();
            m_projectsByGuid = new Dictionary<string, Project>(StringComparer.OrdinalIgnoreCase);
            m_globalSections = new List<GlobalSection>();
            foreach (GlobalSection globalSection in globalSections)
            {
                AddOrUpdateGlobalSection(globalSection);
            }
        }

        private string m_solutionFullPath;
        private List<string> m_headers;
        private List<Project> m_projectsInOrder;
        private Dictionary<string, Project> m_projectsByGuid;
        private List<GlobalSection> m_globalSections;

        public string SolutionFullPath
        { 
            get { return m_solutionFullPath; }
            set { m_solutionFullPath = value; }
        }
        public string SolutionPath { get { return Path.GetDirectoryName(m_solutionFullPath); } }

        public ReadOnlyCollection<Project> ProjectsInOrders { get { return m_projectsInOrder.AsReadOnly(); } }

        public Project AddOrUpdateProject(
                    string projectGuid, 
                    string projectTypeGuid, 
                    string projectName, 
                    string relativePath, 
                    IEnumerable<ProjectSection> projectSections,
                    IEnumerable<PropertyLine> versionControlLines,
                    IEnumerable<PropertyLine> projectConfigurationPlatformsLines)
        {
            return AddOrUpdateProject(new Project(this, projectGuid, projectTypeGuid, projectName, relativePath, projectSections, versionControlLines, projectConfigurationPlatformsLines));
        }
        public Project AddOrUpdateProject(Project newProject)
        {
            Project clone = new Project(this, newProject);

            for (int i = 0; i < m_projectsInOrder.Count; i++)
            {
                if (m_projectsInOrder[i].ProjectGuid == newProject.ProjectGuid)
                {
                    m_projectsByGuid[clone.ProjectGuid] = clone;
                    m_projectsInOrder[i] = clone;
                    return clone;
                }
            }

            m_projectsByGuid.Add(clone.ProjectGuid, clone);
            m_projectsInOrder.Add(clone);
            return clone;
        }
        public void RemoveProject(Project oldProject)
        {
            if (oldProject != null)
            {
                m_projectsInOrder.Remove(oldProject);
                m_projectsByGuid.Remove(oldProject.ProjectGuid);
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
            m_projectsInOrder.Sort(comparer);
        }
        public Project FindProjectByGuid(string guid)
        {
            Project project = null;
            m_projectsByGuid.TryGetValue(guid, out project);
            return project;
        }
        public Project FindProjectByFullName(string projectFullName)
        {
            return m_projectsInOrder.Find(delegate(Project project)
                        {
                            return (project.ProjectFullName == projectFullName);
                        });
        }
        public IEnumerable<Project> Childs
        {
            get
            {
                foreach (Project project in m_projectsInOrder)
                {
                    if (project.ParentFolder == null)
                    {
                        yield return project;
                    }
                }
            }
        }

        public ReadOnlyCollection<string> Headers { get { return m_headers.AsReadOnly(); } }

        public void AddHeaderLine(string line)
        {
            m_headers.Add(line);
        }

        public void RemoveAllHeaderLine()
        {
            m_headers.Clear();
        }

        public ReadOnlyCollection<GlobalSection> GlobalSections { get { return m_globalSections.AsReadOnly(); } }

        public GlobalSection AddGlobalSection(string name, string sectionType, string step, List<PropertyLine> propertyLines)
        {
            GlobalSection newSection = new GlobalSection(name, sectionType, step, propertyLines);
            m_globalSections.Add(newSection);
            return newSection;
        }
        public void AddOrUpdateGlobalSection(GlobalSection newGlobalSection)
        {
            for (int i = 0; i < m_globalSections.Count; i++)
            {
                if (m_globalSections[i].Name == newGlobalSection.Name)
                {
                    m_globalSections[i] = newGlobalSection;
                    return;
                }
            }

            m_globalSections.Add(newGlobalSection);
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
            return m_globalSections.Find(delegate(GlobalSection globalSection)
                        {
                            return (globalSection.Name == name);
                        });
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
            return (NodeDifference) this.GetElement().CompareTo(oldSolution.GetElement());
        }

        public NodeElement GetElement()
        {
            ElementHashList elements = new ElementHashList();
            elements.Add(
                        new ValueElement(
                            new ElementIdentifier("Header"),
                            String.Join("|", m_headers.ToArray())));

            foreach (Project project in this.ProjectsInOrders)
            {
                elements.Add(
                            project.GetElement(
                                new ElementIdentifier(
                                    "P_" + project.ProjectGuid,
                                    string.Format("Project \"{0}\"", project.ProjectFullName))));
            }
            foreach (GlobalSection globalSection in this.GlobalSections)
            {
                elements.Add(
                            globalSection.GetElement(
                                new ElementIdentifier(
                                    "GS_" + globalSection.Name,
                                    string.Format("GlobalSection \"{0}\"", globalSection.Name))));
            }
            return new NodeElement(
                        new ElementIdentifier("SolutionFile"), 
                        elements);
        }

        public SolutionFile(NodeElement element) 
            : base()
        {
            m_solutionFullPath = null;
            m_headers = new List<string>();
            m_projectsInOrder = new List<Project>();
            m_projectsByGuid = new Dictionary<string, Project>(StringComparer.OrdinalIgnoreCase);
            m_globalSections = new List<GlobalSection>();

            foreach (Element child in element.Childs)
            {
                ElementIdentifier identifier = child.Identifier;
                if (identifier.Name.StartsWith("Header"))
                {
                    RemoveAllHeaderLine();
                    foreach (string line in ((ValueElement) child).Value.Split('|'))
                    {
                        AddHeaderLine(line);
                    }
                }
                else if (identifier.Name.StartsWith("P_"))
                {
                    string projectGuid = identifier.Name.Substring(2);
                    AddOrUpdateProject(new Project(projectGuid, (NodeElement)child));
                }
                else if (identifier.Name.StartsWith("GS_"))
                {
                    string sectionName = identifier.Name.Substring(3);
                    AddOrUpdateGlobalSection(new GlobalSection(sectionName, (NodeElement)child));
                }
                else
                {
                    throw new Exception(string.Format("Invalid identifier '{0}'.", identifier.Name));
                }
            }
        }
    }
}
