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
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CWDev.SLNTools.Core
{
    public class SolutionFileReader : IDisposable
    {
        private StreamReader m_reader;
        private int m_currentLineNumber;
        private SolutionFile m_solutionFile;

        public SolutionFileReader(string solutionFullPath)
            : this(new FileStream(solutionFullPath, FileMode.Open, FileAccess.Read))
        {
        }

        public SolutionFileReader(Stream reader)
        {
            m_reader = new StreamReader(reader, Encoding.Default);
            m_currentLineNumber = 0;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (m_reader != null)
            {
                m_reader.Dispose();
                m_reader = null;
            }
        }

        #endregion

        private const string PatternParseHeader = @"^(\s*|Microsoft Visual Studio Solution File.*|#.*|VisualStudioVersion.*|MinimumVisualStudioVersion.*)$";
        private static readonly Regex rs_regexParseHeader = new Regex(PatternParseHeader);

        public SolutionFile ReadSolutionFile()
        {
            lock (m_reader)
            {
                m_solutionFile = new SolutionFile();

                var isHeader = true;
                for (var line = ReadLine(); line != null; line = ReadLine())
                {
                    if (isHeader && rs_regexParseHeader.IsMatch(line))
                    {
                        m_solutionFile.Headers.Add(line);
                        continue;
                    }

                    isHeader = false;
                    if (line.StartsWith("Project(", StringComparison.InvariantCultureIgnoreCase))
                    {
                        m_solutionFile.Projects.Add(ReadProject(line));
                    }
                    else if (String.Compare(line, "Global", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        ReadGlobal();
                        // TODO valide end of file
                        break;
                    }
                    else
                    {
                        throw new SolutionFileException(string.Format("Invalid line read on line #{0}.\nFound: {1}\nExpected: A line beginning with 'Project(' or 'Global'.",
                                        m_currentLineNumber,
                                        line));
                    }
                }
                return m_solutionFile;
            }
        }

        private string ReadLine()
        {
            var line = m_reader.ReadLine();
            if (line == null)
            {
                throw new SolutionFileException("Unexpected end of file encountered while reading the solution file.");
            }

            m_currentLineNumber++;
            return line.Trim();
        }

        private Project FindProjectByGuid(string guid, int lineNumber)
        {
            var p = m_solutionFile.Projects.FindByGuid(guid);
            if (p == null)
            {
                throw new SolutionFileException(string.Format("Invalid guid found on line #{0}.\nFound: {1}\nExpected: A guid from one of the projects in the solution.",
                                lineNumber,
                                guid));
            }
            return p;
        }

        private const string PatternParseProject = "^Project\\(\"(?<PROJECTTYPEGUID>.*)\"\\)\\s*=\\s*\"(?<PROJECTNAME>.*)\"\\s*,\\s*\"(?<RELATIVEPATH>.*)\"\\s*,\\s*\"(?<PROJECTGUID>.*)\"$";
        private static readonly Regex rs_regexParseProject = new Regex(PatternParseProject);

        private Project ReadProject(string firstLine)
        {
            var match = rs_regexParseProject.Match(firstLine);
            if (!match.Success)
            {
                throw new SolutionFileException(string.Format("Invalid format for a project on line #{0}.\nFound: {1}\nExpected: A line starting with 'Global' or respecting the pattern '{2}'.",
                                m_currentLineNumber,
                                firstLine,
                                PatternParseProject));
            }

            var projectTypeGuid = match.Groups["PROJECTTYPEGUID"].Value.Trim();
            var projectName = match.Groups["PROJECTNAME"].Value.Trim();
            var relativePath = match.Groups["RELATIVEPATH"].Value.Trim();
            var projectGuid = match.Groups["PROJECTGUID"].Value.Trim();

            var projectSections = new List<Section>();
            for (var line = ReadLine(); !line.StartsWith("EndProject"); line = ReadLine())
            {
                projectSections.Add(ReadProjectSection(line));
            }

            return new Project(
                        null,
                        projectGuid,
                        projectTypeGuid,
                        projectName,
                        relativePath,
                        null,
                        projectSections,
                        null,
                        null);
        }

        private void ReadGlobal()
        {
            for (var line = ReadLine(); !line.StartsWith("EndGlobal"); line = ReadLine())
            {
                line = line.Trim();
                if (line.Length > 0)
                {
                    ReadGlobalSection(line);
                }
            }
        }

        private const string PatternParseProjectSection = @"^(?<TYPE>ProjectSection)\((?<NAME>.*)\) = (?<STEP>.*)$";
        private static readonly Regex rs_regexParseProjectSection = new Regex(PatternParseProjectSection);

        private Section ReadProjectSection(string firstLine)
        {
            var match = rs_regexParseProjectSection.Match(firstLine);
            if (!match.Success)
            {
                throw new SolutionFileException(string.Format("Invalid format for a project section on line #{0}.\nFound: {1}\nExpected: A line starting with 'EndProject' or respecting the pattern '{2}'.",
                                m_currentLineNumber,
                                firstLine,
                                PatternParseProjectSection));
            }

            var type = match.Groups["TYPE"].Value.Trim();
            var name = match.Groups["NAME"].Value.Trim();
            var step = match.Groups["STEP"].Value.Trim();

            var propertyLines = new List<PropertyLine>();
            var endOfSectionToken = "End" + type;
            for (var line = ReadLine(); !line.StartsWith(endOfSectionToken, StringComparison.InvariantCultureIgnoreCase); line = ReadLine())
            {
                propertyLines.Add(ReadPropertyLine(line, endOfSectionToken));
            }
            return new Section(name, type, step, propertyLines);
        }

        private const string PatternParseGlobalSection = @"^(?<TYPE>GlobalSection)\((?<NAME>.*)\) = (?<STEP>.*)$";
        private static readonly Regex rs_regexParseGlobalSection = new Regex(PatternParseGlobalSection);

        private void ReadGlobalSection(string firstLine)
        {
            var match = rs_regexParseGlobalSection.Match(firstLine);
            if (! match.Success)
            {
                throw new SolutionFileException(string.Format("Invalid format for a global section on line #{0}.\nFound: {1}\nExpected: A line starting with 'EndGlobal' or respecting the pattern '{2}'.",
                                m_currentLineNumber,
                                firstLine,
                                PatternParseGlobalSection));
            }

            var type = match.Groups["TYPE"].Value.Trim();
            var name = match.Groups["NAME"].Value.Trim();
            var step = match.Groups["STEP"].Value.Trim();

            var propertyLines = new List<PropertyLine>();
            var startLineNumber = m_currentLineNumber;
            var endOfSectionToken = "End" + type;
            for (var line = ReadLine(); !line.StartsWith(endOfSectionToken, StringComparison.InvariantCultureIgnoreCase); line = ReadLine())
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
                    if (name.EndsWith("Control", StringComparison.InvariantCultureIgnoreCase))
                    {
                        HandleVersionControlLines(name, type, step, propertyLines);
                    }
                    else if (m_solutionFile.GlobalSections.Contains(name))
                    {
                        m_solutionFile.AddWarning("Duplicate global section '{0}' found in solution.", name);
                    }
                    else
                    {
                        m_solutionFile.GlobalSections.Add(
                                    new Section(
                                        name,
                                        type,
                                        step,
                                        propertyLines));
                    }
                    break;
            }
        }

        private void HandleNestedProjects(string name, string type, string step, IEnumerable<PropertyLine> propertyLines, int startLineNumber)
        {
            var currentLineNumber = startLineNumber;
            foreach (var propertyLine in propertyLines)
            {
                currentLineNumber++;
                var left = FindProjectByGuid(propertyLine.Name, currentLineNumber);
                left.ParentFolderGuid = propertyLine.Value;
            }
            m_solutionFile.GlobalSections.Add(
                        new Section(
                            name,
                            type,
                            step,
                            null));
        }

        private const string PatternParseProjectConfigurationPlatformsName = @"^(?<GUID>\{[-0-9a-zA-Z]+\})\.(?<DESCRIPTION>.*)$";
        private static readonly Regex rs_regexParseProjectConfigurationPlatformsName = new Regex(PatternParseProjectConfigurationPlatformsName);

        private void HandleProjectConfigurationPlatforms(string name, string type, string step, IEnumerable<PropertyLine> propertyLines, int startLineNumber)
        {
            var currentLineNumber = startLineNumber;
            foreach (var propertyLine in propertyLines)
            {
                currentLineNumber++;
                var match = rs_regexParseProjectConfigurationPlatformsName.Match(propertyLine.Name);
                if (! match.Success)
                {
                    throw new SolutionFileException(string.Format("Invalid format for a project configuration name on line #{0}.\nFound: {1}\nExpected: A line respecting the pattern '{2}'.",
                                    m_currentLineNumber,
                                    propertyLine.Name,
                                    PatternParseProjectConfigurationPlatformsName));
                }

                var projectGuid = match.Groups["GUID"].Value;
                var description = match.Groups["DESCRIPTION"].Value;
                var left = FindProjectByGuid(projectGuid, currentLineNumber);
                left.ProjectConfigurationPlatformsLines.Add(
                            new PropertyLine(
                                description,
                                propertyLine.Value));
            }
            m_solutionFile.GlobalSections.Add(
                        new Section(
                            name,
                            type,
                            step,
                            null));
        }

        private static readonly Regex rs_regexParseVersionControlName = new Regex(@"^(?<NAME_WITHOUT_INDEX>[a-zA-Z]*)(?<INDEX>[0-9]+)$");
        private static readonly Regex rs_regexConvertEscapedValues = new Regex(@"\\u(?<HEXACODE>[0-9a-fA-F]{4})");

        private void HandleVersionControlLines(string name, string type, string step, IEnumerable<PropertyLine> propertyLines)
        {
            var propertyLinesByIndex = new Dictionary<int, List<PropertyLine>>();
            var othersVersionControlLines = new List<PropertyLine>();
            foreach (var propertyLine in propertyLines)
            {
                var match = rs_regexParseVersionControlName.Match(propertyLine.Name);
                if (match.Success)
                {
                    var nameWithoutIndex = match.Groups["NAME_WITHOUT_INDEX"].Value.Trim();
                    var index = int.Parse(match.Groups["INDEX"].Value.Trim());

                    if (!propertyLinesByIndex.ContainsKey(index))
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

            // Handle the special case for the solution itself.
            othersVersionControlLines.Add(new PropertyLine("SccLocalPath0", "."));

            foreach (var item in propertyLinesByIndex)
            {
                var index = item.Key;
                var propertiesForIndex = item.Value;

                var uniqueNameProperty = propertiesForIndex.Find(property => property.Name == "SccProjectUniqueName");

                // If there is no ProjectUniqueName, we assume that it's the entry related to the solution by itself. We
                // can ignore it because we added the special case above.
                if (uniqueNameProperty != null)
                {
                    var uniqueName = rs_regexConvertEscapedValues.Replace(uniqueNameProperty.Value, delegate(Match match)
                                {
                                    var hexaValue = int.Parse(match.Groups["HEXACODE"].Value, NumberStyles.AllowHexSpecifier);
                                    return char.ConvertFromUtf32(hexaValue);
                                });
                    uniqueName = uniqueName.Replace(@"\\", @"\");

                    Project relatedProject = null;
                    foreach (var project in m_solutionFile.Projects)
                    {
                        if (string.Compare(project.RelativePath, uniqueName, StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            relatedProject = project;
                        }
                    }
                    if (relatedProject == null)
                    {
                        throw new SolutionFileException(string.Format("Invalid value for the property 'SccProjectUniqueName{0}' of the global section '{1}'.\nFound: {2}\nExpected: A value equal to the field 'RelativePath' of one of the projects in the solution.",
                                        index,
                                        name,
                                        uniqueName));
                    }

                    relatedProject.VersionControlLines.AddRange(propertiesForIndex);
                }
            }

            if (!m_solutionFile.GlobalSections.Contains(name))
            {
                m_solutionFile.GlobalSections.Add(
                            new Section(
                                name,
                                type,
                                step,
                                othersVersionControlLines));
            }
        }

        private const string PatternParsePropertyLine = @"^(?<PROPERTYNAME>[^=]*)\s*=\s*(?<PROPERTYVALUE>[^=]*)$";
        private static readonly Regex rs_regexParsePropertyLine = new Regex(PatternParsePropertyLine);

        private PropertyLine ReadPropertyLine(string line, string endOfSectionToken)
        {
            var match = rs_regexParsePropertyLine.Match(line);
            if (!match.Success)
            {
                throw new SolutionFileException(string.Format("Invalid format for a property on line #{0}.\nFound: {1}\nExpected: A line starting with '{2}' or respecting the pattern '{3}'.",
                                m_currentLineNumber,
                                line,
                                endOfSectionToken,
                                PatternParsePropertyLine));
            }

            return new PropertyLine(
                        match.Groups["PROPERTYNAME"].Value.Trim(),
                        match.Groups["PROPERTYVALUE"].Value.Trim());
        }
    }
}
