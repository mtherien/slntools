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
using System.Text;

namespace CWDev.SLNTools.Core
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
            foreach (Project project in solutionFile.Projects)
            {
                m_writer.WriteLine("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"",
                            project.ProjectTypeGuid,
                            project.ProjectName,
                            project.RelativePath,
                            project.ProjectGuid);
                foreach (Section projectSection in project.ProjectSections)
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
            foreach (Section globalSection in solutionFile.GlobalSections)
            {
                List<PropertyLine> propertyLines = new List<PropertyLine>(globalSection.PropertyLines);
                switch (globalSection.Name)
                {
                    case "NestedProjects":
                        foreach (Project project in solutionFile.Projects)
                        {
                            if (project.ParentFolderGuid != null)
                            {
                                propertyLines.Add(new PropertyLine(project.ProjectGuid, project.ParentFolderGuid));
                            }
                        }
                        break;

                    case "ProjectConfigurationPlatforms":
                        foreach (Project project in solutionFile.Projects)
                        {
                            foreach (PropertyLine propertyLine in project.ProjectConfigurationPlatformsLines)
                            {
                                propertyLines.Add(
                                            new PropertyLine(
                                                string.Format("{0}.{1}", project.ProjectGuid, propertyLine.Name),
                                                propertyLine.Value));
                            }
                        }
                        break;

                    default:
                        if (globalSection.Name.EndsWith("Control", StringComparison.InvariantCultureIgnoreCase))
                        {
                            int index = 1;
                            foreach (Project project in solutionFile.Projects)
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
