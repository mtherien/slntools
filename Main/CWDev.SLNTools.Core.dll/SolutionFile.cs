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
            using (var reader = new SolutionFileReader(solutionFullPath))
            {
                var f = reader.ReadSolutionFile();
                f.SolutionFullPath = solutionFullPath;
                return f;
            }
        }

        public static SolutionFile FromStream(string solutionFullPath, Stream stream)
        {
            using (var reader = new SolutionFileReader(stream))
            {
                var f = reader.ReadSolutionFile();
                f.SolutionFullPath = solutionFullPath;
                return f;
            }
        }

        #endregion

        private string m_solutionFullPath;

        public SolutionFile()
        {
            m_solutionFullPath = null;
            this.Headers = new List<string>();
            this.Projects = new ProjectHashList(this);
            this.GlobalSections = new SectionHashList();
        }

        public SolutionFile(SolutionFile original)
                    : this(original.SolutionFullPath, original.Headers, original.Projects, original.GlobalSections)
        {
        }

        public SolutionFile(string fullpath, IEnumerable<string> headers, IEnumerable<Project> projects, IEnumerable<Section> globalSections)
        {
            m_solutionFullPath = fullpath;
            this.Headers = new List<string>(headers);
            this.Projects = new ProjectHashList(this, projects);
            this.GlobalSections = new SectionHashList(globalSections);
        }

        public string SolutionFullPath
        {
            get { return m_solutionFullPath; }
            set { m_solutionFullPath = value; }
        }

        public List<string> Headers { get; private set; }

        public ProjectHashList Projects { get; private set; }

        public SectionHashList GlobalSections { get; private set; }

        public IEnumerable<Project> Childs
        {
            get
            {
                foreach (var project in this.Projects)
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
            using (var writer = new SolutionFileWriter(solutionPath))
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

            var projectsByFullName = new Dictionary<string, Project>(StringComparer.InvariantCultureIgnoreCase);
            var acceptedDifferences = new List<Difference>();
            var conflicts = new List<Conflict>();
            foreach (var project in this.Projects)
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

                    var otherProjectIdentifier = new ElementIdentifier(
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
        private const string TagSolutionFolderGuids = "SolutionFolderGuids";
        private const string TagProject = "P_";
        private const string TagSolutionFolder = "SF_";
        private const string TagGlobalSection = "GS_";

        public NodeElement ToElement()
        {
            var childs = new List<Element>
                        {
                            new ValueElement(
                                        new ElementIdentifier(TagHeader),
                                        String.Join("|", this.Headers.ToArray()))
                        };

            var solutionFoldersElements = new List<Element>();
            foreach (var project in this.Projects)
            {
                if (project.ProjectTypeGuid == KnownProjectTypeGuid.SolutionFolder)
                {
                    childs.Add(
                                project.ToElement(
                                    new ElementIdentifier(
                                        TagSolutionFolder + project.ProjectFullName,
                                        string.Format("SolutionFolder \"{0}\"", project.ProjectFullName))));
                    solutionFoldersElements.Add(
                                new ValueElement(
                                    new ElementIdentifier(project.ProjectFullName),
                                    project.ProjectGuid));
                }
                else
                {
                    childs.Add(
                                project.ToElement(
                                    new ElementIdentifier(
                                        TagProject + project.ProjectGuid,
                                        string.Format("Project \"{0}\"", project.ProjectFullName))));
                }
            }
            childs.Add(new NodeElement(
                            new ElementIdentifier(TagSolutionFolderGuids),
                            solutionFoldersElements));

            foreach (var globalSection in this.GlobalSections)
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
            var headers = new string[0];
            var projects = new List<Project>();
            var globalSections = new List<Section>();

            var solutionFolderGuids = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (ValueElement solutionGuid in ((NodeElement)element.Childs[new ElementIdentifier(TagSolutionFolderGuids)]).Childs)
            {
                solutionFolderGuids.Add(solutionGuid.Identifier.Name, solutionGuid.Value);
            }

            foreach (var child in element.Childs)
            {
                var identifier = child.Identifier;
                if (identifier.Name == TagHeader)
                {
                    headers = ((ValueElement)child).Value.Split('|');
                }
                else if (identifier.Name == TagSolutionFolderGuids)
                {
                    // Ignore it because we already handled it above
                }
                else if (identifier.Name.StartsWith(TagProject))
                {
                    var projectGuid = identifier.Name.Substring(TagProject.Length);
                    projects.Add(Project.FromElement(projectGuid, (NodeElement)child, solutionFolderGuids));
                }
                else if (identifier.Name.StartsWith(TagSolutionFolder))
                {
                    var projectFullPath = identifier.Name.Substring(TagSolutionFolder.Length);
                    if (!solutionFolderGuids.ContainsKey(projectFullPath))
                    {
                        throw new Exception("TODO");
                    }
                    projects.Add(Project.FromElement(solutionFolderGuids[projectFullPath], (NodeElement)child, solutionFolderGuids));
                }
                else if (identifier.Name.StartsWith(TagGlobalSection))
                {
                    var sectionName = identifier.Name.Substring(TagGlobalSection.Length);
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
