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
using System.Text.RegularExpressions;
using System.Xml;

namespace CWDev.SLNTools.Core
{
    using Merge;

    public class Project
    {
        public Project(SolutionFile container, Project original)
        {
            m_container = container;
            m_projectGuid = original.ProjectGuid;
            m_projectTypeGuid = original.ProjectTypeGuid;
            m_projectName = original.ProjectName;
            m_relativePath = original.RelativePath;
            m_parentFolderGuid = original.ParentFolderGuid;
            m_projectSections = new List<ProjectSection>(original.ProjectSections);
            m_versionControlLines = new PropertyLineHashList(original.VersionControlLines);
            m_projectConfigurationPlatformsLines = new PropertyLineHashList(original.ProjectConfigurationPlatformsLines);
        }

        public Project(
                    SolutionFile container, 
                    string projectGuid, 
                    string projectTypeGuid, 
                    string projectName, 
                    string relativePath, 
                    IEnumerable<ProjectSection> projectSections,
                    IEnumerable<PropertyLine> versionControlLines,
                    IEnumerable<PropertyLine> projectConfigurationPlatformsLines)
        {
            m_container = container;
            m_projectGuid = projectGuid;
            m_projectTypeGuid = projectTypeGuid;
            m_projectName = projectName;
            m_relativePath = relativePath;
            m_parentFolderGuid = null;
            m_projectSections = new List<ProjectSection>();
            if (projectSections != null)
            {
                foreach (ProjectSection projectSection in projectSections)
                {
                    m_projectSections.Add(new ProjectSection(projectSection));
                }
            }
            m_versionControlLines = new PropertyLineHashList();
            if (versionControlLines != null)
            {
                m_versionControlLines.AddRange(versionControlLines);
            }
            m_projectConfigurationPlatformsLines = new PropertyLineHashList();
            if (projectConfigurationPlatformsLines != null)
            {
                m_projectConfigurationPlatformsLines.AddRange(projectConfigurationPlatformsLines);
            }
        }

        private SolutionFile m_container;
        private string m_projectGuid;
        private string m_projectTypeGuid;
        private string m_projectName;
        private string m_relativePath;
        private string m_parentFolderGuid;
        private List<ProjectSection> m_projectSections;
        private PropertyLineHashList m_versionControlLines;
        private PropertyLineHashList m_projectConfigurationPlatformsLines;

        public string ProjectGuid { get { return m_projectGuid; } }
        public string ProjectTypeGuid { get { return m_projectTypeGuid; } }
        public string ProjectName { get { return m_projectName; } }
        public string RelativePath { get { return m_relativePath; } }
        public string FullPath { get { return Path.Combine(m_container.SolutionPath, m_relativePath); } }
        public string ParentFolderGuid
        {
            get { return m_parentFolderGuid; }
            set { m_parentFolderGuid = value; }
        }
        public ReadOnlyCollection<ProjectSection> ProjectSections { get { return m_projectSections.AsReadOnly(); } }
        public PropertyLineHashList VersionControlLines { get { return m_versionControlLines; } }
        public PropertyLineHashList ProjectConfigurationPlatformsLines { get { return m_projectConfigurationPlatformsLines; } }

        private Project FindProjectInContainer(string projectGuid, string errorMessageFormat, params object[] errorMessageParams)
        {
            Project project = m_container.FindProjectByGuid(projectGuid);
            if (project == null)
            {
                throw new MissingProjectException(string.Format(errorMessageFormat, errorMessageParams));
            }
            return project;
        }

        public Project ParentFolder
        {
            get
            {
                if (m_parentFolderGuid == null)
                    return null;

                return FindProjectInContainer(
                            m_parentFolderGuid,
                            "Cannot find the parent folder of project '{0}'. \nProject guid: {1}\nParent folder guid: {2}", 
                            m_projectName,
                            m_projectGuid,
                            m_parentFolderGuid);
            }
        }
        public string ProjectFullName
        {
            get
            {
                if (this.ParentFolder != null)
                {
                    return this.ParentFolder.ProjectFullName + @"\" + this.ProjectName;
                }
                else
                {
                    return this.ProjectName;
                }
            }
        }
        public IEnumerable<Project> Childs
        {
            get
            {
                if (m_projectTypeGuid == KnownProjectTypeGuid.SolutionFolder)
                {
                    foreach (Project project in m_container.ProjectsInOrders)
                    {
                        if (project.m_parentFolderGuid == m_projectGuid)
                            yield return project;
                    }
                }
            }
        }
        public IEnumerable<Project> AllDescendants
        {
            get
            {
                foreach (Project child in this.Childs)
                {
                    yield return child;
                    foreach (Project subchild in child.AllDescendants)
                    {
                        yield return subchild;
                    }
                }
            }
        }
        public IEnumerable<Project> Dependencies
        {
            get
            {
                if (this.ParentFolder != null)
                {
                    yield return this.ParentFolder;
                }

                ProjectSection projectDependenciesSection = m_projectSections.Find(delegate(ProjectSection section)
                            {
                                return (section.Name == "ProjectDependencies");
                            });
                if (projectDependenciesSection != null)
                {
                    foreach (PropertyLine propertyLine in projectDependenciesSection.PropertyLines)
                    {
                        string dependencyGuid = propertyLine.Name;
                        yield return FindProjectInContainer(
                                    dependencyGuid,
                                    "Cannot find one of the dependency of project '{0}'.\nProject guid: {1}\nDependency guid: {2}\nReference found in: ProjectDependencies section of the solution file",
                                    m_projectName,
                                    m_projectGuid,
                                    dependencyGuid);
                    }
                }

                switch (m_projectTypeGuid)
                {
                    case KnownProjectTypeGuid.VisualBasic:
                    case KnownProjectTypeGuid.CSharp:
                    case KnownProjectTypeGuid.JSharp:
                    default:
                        if (! File.Exists(this.FullPath))
                        { 
                            throw new IOException(string.Format(
                                        "Cannot detect dependencies of projet '{0}' because the project file cannot be found.\nProject full path: '{1}'",
                                        m_projectName,
                                        this.FullPath));
                        }

                        XmlDocument docManaged = new XmlDocument();
                        docManaged.Load(this.FullPath);

                        XmlNamespaceManager xmlManager = new XmlNamespaceManager(docManaged.NameTable);
                        xmlManager.AddNamespace("prefix", "http://schemas.microsoft.com/developer/msbuild/2003");

                        foreach (XmlNode xmlNode in docManaged.SelectNodes(@"//prefix:ProjectReference", xmlManager))
                        {
                            string dependencyGuid = xmlNode.SelectSingleNode(@"prefix:Project", xmlManager).InnerText.Trim(); // TODO handle null
                            string dependencyName = xmlNode.SelectSingleNode(@"prefix:Name", xmlManager).InnerText.Trim(); // TODO handle null
                            yield return FindProjectInContainer(
                                        dependencyGuid,
                                        "Cannot find one of the dependency of project '{0}'.\nProject guid: {1}\nDependency guid: {2}\nDependency name: {3}\nReference found in: ProjectReference node of file '{4}'",
                                        m_projectName,
                                        m_projectGuid,
                                        dependencyGuid,
                                        dependencyName,
                                        this.FullPath);
                        }
                        break;

                    case KnownProjectTypeGuid.SolutionFolder:
                        break;

                    case KnownProjectTypeGuid.VisualC:
                        if (!File.Exists(this.FullPath))
                        {
                            throw new IOException(string.Format(
                                        "Cannot detect dependencies of projet '{0}' because the project file cannot be found.\nProject full path: '{1}'",
                                        m_projectName,
                                        this.FullPath));
                        }

                        XmlDocument docVisualC = new XmlDocument();
                        docVisualC.Load(this.FullPath);

                        foreach (XmlNode xmlNode in docVisualC.SelectNodes(@"//ProjectReference"))
                        {
                            string dependencyGuid = xmlNode.Attributes["ReferencedProjectIdentifier"].Value; // TODO handle null
                            string dependencyRelativePathToProject = xmlNode.Attributes["RelativePathToProject"].Value; // TODO handle null
                            yield return FindProjectInContainer(
                                        dependencyGuid,
                                        "Cannot find one of the dependency of project '{0}'.\nProject guid: {1}\nDependency guid: {2}\nDependency relative path: '{3}'\nReference found in: ProjectReference node of file '{4}'",
                                        m_projectName,
                                        m_projectGuid,
                                        dependencyGuid,
                                        dependencyRelativePathToProject,
                                        this.FullPath);
                        }
                        break;

                    case KnownProjectTypeGuid.Setup:
                        if (!File.Exists(this.FullPath))
                        {
                            throw new IOException(string.Format(
                                        "Cannot detect dependencies of projet '{0}' because the project file cannot be found.\nProject full path: '{1}'",
                                        m_projectName,
                                        this.FullPath));
                        }

                        foreach (string line in File.ReadAllLines(this.FullPath))
                        {
                            Regex regex = new Regex("^\\s*\"OutputProjectGuid\" = \"\\d*\\:(?<PROJECTGUID>.*)\"$");
                            Match match = regex.Match(line);
                            if (match.Success)
                            {
                                string dependencyGuid = match.Groups["PROJECTGUID"].Value.Trim();
                                yield return FindProjectInContainer(
                                            dependencyGuid,
                                            "Cannot find one of the dependency of project '{0}'.\nProject guid: {1}\nDependency guid: {2}\nReference found in: OutputProjectGuid line of file '{3}'",
                                            m_projectName,
                                            m_projectGuid,
                                            dependencyGuid,
                                            this.FullPath);
                            }
                        }
                        break;

                    case KnownProjectTypeGuid.WebProject:
                        ProjectSection websitePropertiesSection = m_projectSections.Find(delegate(ProjectSection section)
                                    {
                                        return (section.Name == "WebsiteProperties");
                                    });
                        foreach (PropertyLine propertyLine in websitePropertiesSection.PropertyLines)
                        {
                            if (string.Compare(propertyLine.Name, "ProjectReferences", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                // Format is: "({GUID}|ProjectName;)*"
                                // Example: "{GUID}|Infra.dll;{GUID2}|Services.dll;"
                                string value = propertyLine.Value;
                                if (value.StartsWith("\""))
                                    value = value.Substring(1);
                                if (value.EndsWith("\""))
                                    value = value.Substring(0, value.Length - 1);

                                foreach (string dependency in value.Split(';'))
                                {
                                    if (dependency.Trim().Length > 0)
                                    {
                                        string[] parts = dependency.Split('|');
                                        string dependencyGuid = parts[0];
                                        string dependencyName = parts[1]; // TODO handle null
                                        yield return FindProjectInContainer(
                                                    dependencyGuid,
                                                    "Cannot find one of the dependency of project '{0}'.\nProject guid: {1}\nDependency guid: {2}\nDependency name: {3}\nReference found in: ProjectReferences line in WebsiteProperties section of the solution file",
                                                    m_projectName,
                                                    m_projectGuid,
                                                    dependencyGuid,
                                                    dependencyName);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("Project '{0}'", this.ProjectFullName);
        }


        public Project(string projectGuid, NodeElement element)
        {
            m_container = null;
            m_projectGuid = projectGuid;
            m_projectTypeGuid = null;
            m_projectName = null;
            m_relativePath = null;
            m_parentFolderGuid = null;
            m_projectSections = new List<ProjectSection>();
            m_versionControlLines = new PropertyLineHashList();
            m_projectConfigurationPlatformsLines = new PropertyLineHashList();

            foreach (Element child in element.Childs)
            {
                ElementIdentifier identifier = child.Identifier;
                if (identifier.Name == "ProjectTypeGuid")
                {
                    m_projectTypeGuid = ((ValueElement)child).Value;
                }
                else if (identifier.Name == "ProjectName")
                {
                    m_projectName = ((ValueElement)child).Value;
                }
                else if (identifier.Name == "RelativePath")
                {
                    m_relativePath = ((ValueElement)child).Value;
                }
                else if (identifier.Name == "ParentFolder")
                {
                    m_parentFolderGuid = ((ValueElement)child).Value.Split('|')[1];
                }
                else if (identifier.Name.StartsWith("S_"))
                {
                    string sectionName = identifier.Name.Substring(2);
                    m_projectSections.Add(
                                new ProjectSection(
                                    sectionName,
                                    (NodeElement)child));
                }
                else if (identifier.Name.StartsWith("VCL_"))
                {
                    string name = identifier.Name.Substring(4);
                    string value = ((ValueElement)child).Value;
                    m_versionControlLines.Add(new PropertyLine(name, value));
                }
                else if (identifier.Name.StartsWith("PCPL_"))
                {
                    string name = identifier.Name.Substring(5);
                    string value = ((ValueElement)child).Value;
                    m_projectConfigurationPlatformsLines.Add(new PropertyLine(name, value));
                }
                else
                {
                    throw new Exception(string.Format("Invalid identifier '{0}'.", identifier.Name));
                }
            }

            if (m_projectTypeGuid == null)
                throw new Exception("TODO element doesn't containt ProjectTypeGuid");

            if (m_projectName == null)
                throw new Exception("TODO element doesn't containt ProjectName");

            if (m_relativePath == null)
                throw new Exception("TODO element doesn't containt RelativePath");
        }

        public NodeElement GetElement(ElementIdentifier identifier)
        {
            ElementHashList elements = new ElementHashList();
            elements.Add(new ValueElement(new ElementIdentifier("ProjectTypeGuid"), this.ProjectTypeGuid));
            elements.Add(new ValueElement(new ElementIdentifier("ProjectName"), this.ProjectName));
            elements.Add(new ValueElement(new ElementIdentifier("RelativePath"), this.RelativePath));
            if (this.ParentFolder != null)
            {
                elements.Add(new ValueElement(
                            new ElementIdentifier("ParentFolder"),
                            string.Format("{0}|{1}",
                                this.ParentFolder.ProjectFullName,
                                this.ParentFolder.ProjectGuid)));
            }

            foreach (ProjectSection projectSection in this.ProjectSections)
            {
                elements.Add(
                            projectSection.GetElement(
                                new ElementIdentifier(
                                    "S_" + projectSection.Name,
                                    string.Format("{0} \"{1}\"", projectSection.SectionType, projectSection.Name))));
            }
            foreach (PropertyLine propertyLine in this.VersionControlLines)
            {
                elements.Add(
                            new ValueElement(
                                new ElementIdentifier(
                                    "VCL_" + propertyLine.Name,
                                    @"VersionControlLine\" + propertyLine.Name), 
                                propertyLine.Value));
            }
            foreach (PropertyLine propertyLine in this.ProjectConfigurationPlatformsLines)
            {
                elements.Add(
                            new ValueElement(
                                new ElementIdentifier(
                                    "PCPL_" + propertyLine.Name,
                                    @"ProjectConfigurationPlatformsLine\" + propertyLine.Name),
                                propertyLine.Value));
            }
            return new NodeElement(
                            identifier,
                            elements);
        }
    }
}
