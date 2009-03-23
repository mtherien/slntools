using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CWDev.VSSolutionTools.Core
{
    public class SolutionFileReader : IDisposable
    {
        public SolutionFileReader(string solutionFullPath) : this(new FileStream(solutionFullPath, FileMode.Open, FileAccess.Read))
        {
        }

        public SolutionFileReader(Stream reader)
        {
            m_reader = new StreamReader(reader, Encoding.Default);
            m_currentLineNumber = 0;
        }

        private StreamReader m_reader;
        private int m_currentLineNumber;
        private SolutionFile m_solutionFile;

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (m_reader != null)
            {
                m_reader.Dispose();
                m_reader = null;
            }
        }

        #endregion

        public SolutionFile ReadSolutionFile()
        {
            lock (m_reader)
            {
                m_solutionFile = new SolutionFile();
                ReadHeader();
                for (string line = ReadLine(); line != null; line = ReadLine())
                {
                    if (line.StartsWith("Project(", StringComparison.Ordinal))
                    {
                        ReadProject(line);
                    }
                    else if (String.Compare(line, "Global", StringComparison.Ordinal) == 0)
                    {
                        ReadGlobal();
                        // TODO valide end of file
                        break;
                    }
                    else
                    {
                        throw new IOException(string.Format("Invalid line read on line #{0}.\nFound: {1}\nExpected: A line beginning with 'Project(' or 'Global'.",
                                        m_currentLineNumber, 
                                        line));
                    }
                }
                return m_solutionFile;
            }
        }

        private string ReadLine()
        {
            string line = m_reader.ReadLine();
            if (line == null)
            {
                throw new IOException(string.Format("Unexpected end of file encounted while reading the solution file."));
            }

            m_currentLineNumber++;
            return line.Trim();
        }

        private Project FindProjectByGuid(string guid, int lineNumber)
        {
            Project p = m_solutionFile.FindProjectByGuid(guid);
            if (p == null)
            {
                throw new IOException(string.Format("Invalid guid found on line #{0}.\nFound: {1}\nExpected: A guid from one of the projects in the solution.",
                                lineNumber,
                                guid));
            }
            return p;
        }

        private void ReadHeader()
        {
            for (int i = 1; i <= 3; i++)
            {
                string line = ReadLine();
                m_solutionFile.AddHeaderLine(line);
                if (line.StartsWith("#", StringComparison.Ordinal))
                {
                    return;
                }
            }
        }

        private static readonly string ms_patternParseProject = "^Project\\(\"(?<PROJECTTYPEGUID>.*)\"\\)\\s*=\\s*\"(?<PROJECTNAME>.*)\"\\s*,\\s*\"(?<RELATIVEPATH>.*)\"\\s*,\\s*\"(?<PROJECTGUID>.*)\"$";
        private static readonly Regex ms_regexParseProject = new Regex(ms_patternParseProject);

        private void ReadProject(string firstLine)
        {
            Match match = ms_regexParseProject.Match(firstLine);
            if (!match.Success)
            {
                throw new IOException(string.Format("Invalid format for a project on line #{0}.\nFound: {1}\nExpected: A line starting with 'Global' or respecting the pattern '{2}'.",
                                m_currentLineNumber,
                                firstLine,
                                ms_patternParseProject));
            }

            string projectTypeGuid = match.Groups["PROJECTTYPEGUID"].Value.Trim();
            string projectName = match.Groups["PROJECTNAME"].Value.Trim();
            string relativePath = match.Groups["RELATIVEPATH"].Value.Trim();
            string projectGuid = match.Groups["PROJECTGUID"].Value.Trim();
            List<ProjectSection> projectSections = new List<ProjectSection>();
            for (string line = ReadLine(); !line.StartsWith("EndProject"); line = ReadLine())
            {
                projectSections.Add(ReadProjectSection(line));
            }

            m_solutionFile.AddOrUpdateProject(projectGuid, projectTypeGuid, projectName, relativePath, projectSections, null, null);
        }

        private void ReadGlobal()
        {
            for (string line = ReadLine(); !line.StartsWith("EndGlobal"); line = ReadLine())
            {
                ReadGlobalSection(line);
            }
        }

        private static readonly string ms_patternParseProjectSection = @"^(?<TYPE>ProjectSection)\((?<NAME>.*)\) = (?<STEP>.*)$";
        private static readonly Regex ms_regexParseProjectSection = new Regex(ms_patternParseProjectSection);

        private ProjectSection ReadProjectSection(string firstLine)
        {
            Match match = ms_regexParseProjectSection.Match(firstLine);
            if (!match.Success)
            {
                throw new IOException(string.Format("Invalid format for a project section on line #{0}.\nFound: {1}\nExpected: A line starting with 'EndProject' or respecting the pattern '{2}'.",
                                m_currentLineNumber,
                                firstLine,
                                ms_patternParseProjectSection));
            }

            string type = match.Groups["TYPE"].Value.Trim();
            string name = match.Groups["NAME"].Value.Trim();
            string step = match.Groups["STEP"].Value.Trim();

            List<PropertyLine> propertyLines = new List<PropertyLine>();
            int startLineNumber = m_currentLineNumber;
            string endOfSectionToken = "End" + type;
            for (string line = ReadLine(); !line.StartsWith(endOfSectionToken, StringComparison.Ordinal); line = ReadLine())
            {
                propertyLines.Add(ReadPropertyLine(line, endOfSectionToken));
            }
            return new ProjectSection(name, type, step, propertyLines);
        }

        private static readonly string ms_patternParseGlobalSection = @"^(?<TYPE>GlobalSection)\((?<NAME>.*)\) = (?<STEP>.*)$";
        private static readonly Regex ms_regexParseGlobalSection = new Regex(ms_patternParseGlobalSection);

        private void ReadGlobalSection(string firstLine)
        {
            Match match = ms_regexParseGlobalSection.Match(firstLine);
            if (! match.Success)
            {
                throw new IOException(string.Format("Invalid format for a global section on line #{0}.\nFound: {1}\nExpected: A line starting with 'EndGlobal' or respecting the pattern '{2}'.",
                                m_currentLineNumber,
                                firstLine,
                                ms_patternParseGlobalSection));
            }

            string type = match.Groups["TYPE"].Value.Trim();
            string name = match.Groups["NAME"].Value.Trim();
            string step = match.Groups["STEP"].Value.Trim();

            List<PropertyLine> propertyLines = new List<PropertyLine>();
            int startLineNumber = m_currentLineNumber;
            string endOfSectionToken = "End" + type;
            for (string line = ReadLine(); !line.StartsWith(endOfSectionToken, StringComparison.Ordinal); line = ReadLine())
            {
                propertyLines.Add(ReadPropertyLine(line, endOfSectionToken));
            }

            switch (name)
            {
                case "NestedProjects":
                    HandleNestedProjects(name, type, step, propertyLines, startLineNumber);
                    break;

                case "ProjectConfigurationPlatforms":
                    HandleProjectConfigurationPlatforms(name, type, step, propertyLines, startLineNumber);
                    break;

                default:
                    if (name.EndsWith("Control", StringComparison.Ordinal))
                    {
                        HandleVersionControlLines(name, type, step, propertyLines);
                    }
                    else
                    {
                        m_solutionFile.AddGlobalSection(name, type, step, propertyLines);
                    }
                    break;
            }
        }

        private void HandleNestedProjects(string name, string type, string step, List<PropertyLine> propertyLines, int startLineNumber)
        {
            int currentLineNumber = startLineNumber;
            foreach (PropertyLine propertyLine in propertyLines)
            {
                currentLineNumber++;
                Project left = FindProjectByGuid(propertyLine.Name, currentLineNumber);
                left.ParentFolderGuid = propertyLine.Value;
            }
            m_solutionFile.AddGlobalSection(name, type, step, new List<PropertyLine>());
        }

        private static readonly Regex ms_regexParseProjectConfigurationPlatformsName = new Regex(@"^(?<GUID>\{[-0-9a-zA-Z]+\})\.(?<DESCRIPTION>.*)$");

        private void HandleProjectConfigurationPlatforms(string name, string type, string step, List<PropertyLine> propertyLines, int startLineNumber)
        {
            int currentLineNumber = startLineNumber;
            foreach (PropertyLine propertyLine in propertyLines)
            {
                currentLineNumber++;
                Match match = ms_regexParseProjectConfigurationPlatformsName.Match(propertyLine.Name);
                if (match.Success)
                {
                    string projectGuid = match.Groups["GUID"].Value;
                    string description = match.Groups["DESCRIPTION"].Value;
                    Project left = FindProjectByGuid(projectGuid, currentLineNumber);
                    left.ProjectConfigurationPlatformsLines.Add(
                                new PropertyLine(
                                    description,
                                    propertyLine.Value));
                }
                else 
                {
                    throw new Exception("TODO");
                }
            }
            m_solutionFile.AddGlobalSection(name, type, step, new List<PropertyLine>());
        }

        private static readonly Regex ms_regexParseVersionControlName = new Regex(@"^(?<NAME_WITHOUT_INDEX>[a-zA-Z]*)(?<INDEX>[1-9][0-9]*)$");
        private static readonly Regex ms_regexConvertEscapedValues = new Regex(@"\\u(?<HEXACODE>[0-9a-fA-F]{4})");

        private void HandleVersionControlLines(string name, string type, string step, List<PropertyLine> propertyLines)
        {
            Dictionary<int, List<PropertyLine>> propertyLinesByIndex = new Dictionary<int, List<PropertyLine>>();
            List<PropertyLine> othersVersionControlLines = new List<PropertyLine>();
            foreach (PropertyLine propertyLine in propertyLines)
            {
                Match match = ms_regexParseVersionControlName.Match(propertyLine.Name);
                if (match.Success)
                {
                    string nameWithoutIndex = match.Groups["NAME_WITHOUT_INDEX"].Value.Trim();
                    int index = int.Parse(match.Groups["INDEX"].Value.Trim());

                    if (! propertyLinesByIndex.ContainsKey(index))
                    {
                        propertyLinesByIndex[index] = new List<PropertyLine>();
                    }
                    propertyLinesByIndex[index].Add(new PropertyLine(nameWithoutIndex, propertyLine.Value));
                }
                else
                {
                    // Ignore SccNumberOfProjects. This number will be computed and added by the SolutionFileWriter class.
                    if (propertyLine.Name != "SccNumberOfProjects")
                    {
                        othersVersionControlLines.Add(propertyLine);
                    }
                }
            }

            foreach (KeyValuePair<int, List<PropertyLine>> item in propertyLinesByIndex)
            {
                int index = item.Key;
                List<PropertyLine> propertiesForIndex = item.Value;

                PropertyLine uniqueNameProperty = propertiesForIndex.Find(delegate(PropertyLine property)
                            {
                                return property.Name == "SccProjectUniqueName";
                            });
                if (uniqueNameProperty == null)
                {
                    throw new IOException(string.Format("Missing property 'SccProjectUniqueName' for the element #{0} in the global section '{1}'.\nFound: Nothing.\nExpected: A property with the name 'SccProjectUniqueName{0}'.",
                                    index,
                                    name));
                }

                string uniqueName = ms_regexConvertEscapedValues.Replace(uniqueNameProperty.Value, delegate(Match match)
                            {
                                int hexaValue = int.Parse(match.Groups["HEXACODE"].Value, NumberStyles.AllowHexSpecifier);
                                return char.ConvertFromUtf32(hexaValue);
                            });
                uniqueName = uniqueName.Replace(@"\\", @"\");

                Project relatedProject = null;
                foreach (Project project in m_solutionFile.ProjectsInOrders)
                {
                    if (project.RelativePath == uniqueName)
                    {
                        relatedProject = project;
                    }
                }
                if (relatedProject == null)
                {
                    throw new IOException(string.Format("Invalid value for the property 'SccProjectUniqueName{0}' of the global section '{1}'.\nFound: {2}\nExpected: A value equal to the field 'RelativePath' of one of the projects in the solution.",
                                    index,
                                    name,
                                    uniqueName));
                }

                foreach (PropertyLine propertyLine in propertiesForIndex)
                {
                    relatedProject.VersionControlLines.Add(propertyLine);
                }
            }

            m_solutionFile.AddGlobalSection(name, type, step, othersVersionControlLines);
        }

        private static readonly string ms_patternParsePropertyLine = @"^(?<PROPERTYNAME>[^=]*)\s*=\s*(?<PROPERTYVALUE>[^=]*)$";
        private static readonly Regex ms_regexParsePropertyLine = new Regex(ms_patternParsePropertyLine);

        private PropertyLine ReadPropertyLine(string line, string endOfSectionToken)
        {
            Match match = ms_regexParsePropertyLine.Match(line);
            if (!match.Success)
            {
                throw new IOException(string.Format("Invalid format for a property on line #{0}.\nFound: {1}\nExpected: A line starting with '{2}' or respecting the pattern '{3}'.",
                                m_currentLineNumber,
                                line,
                                endOfSectionToken,
                                ms_patternParsePropertyLine));
            }

            return new PropertyLine(
                        match.Groups["PROPERTYNAME"].Value.Trim(),
                        match.Groups["PROPERTYVALUE"].Value.Trim());
        }
    }
}
