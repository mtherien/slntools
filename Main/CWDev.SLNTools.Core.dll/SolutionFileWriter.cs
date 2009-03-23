using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CWDev.VSSolutionTools.Core
{
    public class SolutionFileWriter : IDisposable
    {
        public SolutionFileWriter(string solutionFullPath) : this(new FileStream(solutionFullPath, FileMode.Create, FileAccess.Write))
        {
        }

        public SolutionFileWriter(Stream writer)
        {
            m_writer = new StreamWriter(writer, Encoding.UTF8);
        }

        private StreamWriter m_writer;

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (m_writer != null)
            {
                m_writer.Dispose();
                m_writer = null;
            }
        }

        #endregion

        public void WriteSolutionFile(SolutionFile solutionFile)
        {
            lock (m_writer)
            {
                WriteHeader(solutionFile);
                WriteProjects(solutionFile);
                WriteGlobal(solutionFile);
            }
        }

        private void WriteHeader(SolutionFile solutionFile)
        {
            foreach (string line in solutionFile.Headers)
            {
                m_writer.WriteLine(line);
            }
        }

        private void WriteProjects(SolutionFile solutionFile)
        {
            foreach (Project project in solutionFile.ProjectsInOrders)
            {
                m_writer.WriteLine("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"",
                            project.ProjectTypeGuid,
                            project.ProjectName,
                            project.RelativePath,
                            project.ProjectGuid);
                foreach (ProjectSection projectSection in project.ProjectSections)
                {
                    WriteSection(projectSection, projectSection.PropertyLines);
                }
                m_writer.WriteLine("EndProject");
            }
        }

        private void WriteGlobal(SolutionFile solutionFile)
        {
            m_writer.WriteLine("Global");
            WriteGlobalSections(solutionFile);
            m_writer.WriteLine("EndGlobal");
        }

        private void WriteGlobalSections(SolutionFile solutionFile)
        {
            foreach (GlobalSection globalSection in solutionFile.GlobalSections)
            {
                List<PropertyLine> propertyLines = new List<PropertyLine>(globalSection.PropertyLines);
                switch (globalSection.Name)
                {
                    case "NestedProjects":
                        foreach (Project project in solutionFile.ProjectsInOrders)
                        {
                            if (project.ParentFolderGuid != null)
                            {
                                propertyLines.Add(new PropertyLine(project.ProjectGuid, project.ParentFolderGuid));
                            }
                        }
                        break;

                    case "ProjectConfigurationPlatforms":
                        foreach (Project project in solutionFile.ProjectsInOrders)
                        {
                            foreach (PropertyLine propertyLine in project.ProjectConfigurationPlatformsLines)
                            {
                                propertyLines.Add(
                                            new PropertyLine(
                                                project.ProjectGuid + "." + propertyLine.Name,
                                                propertyLine.Value));
                            }
                        }
                        break;

                    default:
                        if (globalSection.Name.EndsWith("Control", StringComparison.Ordinal))
                        {
                            int index = 1;
                            foreach (Project project in solutionFile.ProjectsInOrders)
                            {
                                if (project.VersionControlLines.Count > 0)
                                {
                                    foreach (PropertyLine propertyLine in project.VersionControlLines)
                                    {
                                        propertyLines.Add(
                                                    new PropertyLine(
                                                        string.Format("{0}{1}", propertyLine.Name, index),
                                                        propertyLine.Value));
                                    }
                                    index++;
                                }
                            }

                            propertyLines.Insert(0, new PropertyLine("SccNumberOfProjects", index.ToString()));                            
                        }
                        break;
                }

                WriteSection(globalSection, propertyLines);
            }
        }

        private void WriteSection(Section section, IEnumerable<PropertyLine> propertyLines)
        {
            m_writer.WriteLine("\t{0}({1}) = {2}", section.SectionType, section.Name, section.Step);
            foreach (PropertyLine propertyLine in propertyLines)
            {
                m_writer.WriteLine("\t\t{0} = {1}", propertyLine.Name, propertyLine.Value);
            }
            m_writer.WriteLine("\tEnd{0}", section.SectionType);
        }
    }
}
