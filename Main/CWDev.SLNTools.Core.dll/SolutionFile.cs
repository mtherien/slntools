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
using System.IO;

namespace CWDev.SLNTools.Core
{
    using Merge;

    public class SolutionFile
    {
        #region public static: Methods FromFile / FromStream

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

        #endregion

        public SolutionFile()
        {
            m_solutionFullPath = null;
            m_headers = new List<string>();
            m_projects = new ProjectHashList(this);
            m_globalSections = new SectionHashList();
        }

        public SolutionFile(SolutionFile original)
                    : this(original.SolutionFullPath, original.Headers, original.Projects, original.GlobalSections)
        {
        }

        public SolutionFile(string fullpath, IEnumerable<string> headers, IEnumerable<Project> projects, IEnumerable<Section> globalSections)
        {
            m_solutionFullPath = fullpath;
            m_headers = new List<string>(headers);
            m_projects = new ProjectHashList(this, projects);
            m_globalSections = new SectionHashList(globalSections);
        }

        private string m_solutionFullPath;
        private List<string> m_headers;
        private ProjectHashList m_projects;
        private SectionHashList m_globalSections;

        public string SolutionFullPath
        { 
            get { return m_solutionFullPath; }
            set { m_solutionFullPath = value; }
        }
        public List<string> Headers 
        { 
            get { return m_headers; } 
        }
        public ProjectHashList Projects 
        { 
            get { return m_projects; } 
        }
        public SectionHashList GlobalSections 
        { 
            get { return m_globalSections; } 
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

        public NodeConflict Validate(out List<string> messages)
        {
            // TODO Finish this.
            messages = new List<string>();

            Dictionary<string, Project> projectsByFullName = new Dictionary<string, Project>(StringComparer.InvariantCultureIgnoreCase);
            List<Difference> acceptedDifferences = new List<Difference>();
            List<Conflict> conflicts = new List<Conflict>();
            foreach (Project project in m_projects)
            {
                Project otherProject;
                if (projectsByFullName.TryGetValue(project.ProjectFullName, out otherProject))
                {
                    acceptedDifferences.Add(
                                new NodeDifference(
                                    new ElementIdentifier(
                                        TagProject + project.ProjectGuid,
                                        string.Format("Project \"{0}\" [{1}]", project.ProjectFullName, project.ProjectGuid)), 
                                    OperationOnParent.Removed, 
                                    null));

                    ElementIdentifier otherProjectIdentifier = new ElementIdentifier(
                                        TagProject + otherProject.ProjectGuid,
                                        string.Format("Project \"{0}\" [{1}]", otherProject.ProjectFullName, otherProject.ProjectGuid));

                    conflicts.Add(
                                Conflict.Merge(
                                    new NodeElement(otherProjectIdentifier, null),
                                    otherProject.ToElement(otherProjectIdentifier),
                                    project.ToElement(otherProjectIdentifier)));
                }
                else
                {
                    projectsByFullName.Add(project.ProjectFullName, project);
                } 
            }

            return new NodeConflict(new ElementIdentifier("SolutionFile"), OperationOnParent.Modified, acceptedDifferences, conflicts);
        }

        #region Methods ToElement / FromElement

        private const string TagHeader = "Header";
        private const string TagProject = "P_";
        private const string TagGlobalSection = "GS_";

        public NodeElement ToElement()
        {
            List<Element> childs = new List<Element>();
            childs.Add(
                        new ValueElement(
                            new ElementIdentifier(TagHeader),
                            String.Join("|", m_headers.ToArray())));

            foreach (Project project in this.Projects)
            {
                childs.Add(
                            project.ToElement(
                                new ElementIdentifier(
                                    TagProject + project.ProjectGuid,
                                    string.Format("Project \"{0}\"", project.ProjectFullName))));
            }
            foreach (Section globalSection in this.GlobalSections)
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
            List<Project> projects = new List<Project>();
            List<Section> globalSections = new List<Section>();

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
                    globalSections.Add(Section.FromElement(sectionName, (NodeElement)child));
                }
                else
                {
                    throw new SolutionFileException(string.Format("Invalid identifier '{0}'.", identifier.Name));
                }
            }
            return new SolutionFile(null, headers, projects, globalSections);
        }

        #endregion
    }
}
