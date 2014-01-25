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
        public SolutionFileReader(string solutionFullPath) : this(new FileStream(solutionFullPath, FileMode.Open, FileAccess.Read))
        {
        }

        public SolutionFileReader(Stream reader)
        {
            this._reader = new StreamReader(reader, Encoding.Default);
            this._currentLineNumber = 0;
        }

        private StreamReader _reader;
        private int _currentLineNumber;
        private SolutionFile _solutionFile;

        #region IDisposable Members

        public void Dispose()
        {
            if (this._reader != null)
            {
                this._reader.Dispose();
                this._reader = null;
            }
        }

        #endregion

        private static readonly string ms_patternParseHeader = @"^(\s*|Microsoft Visual Studio Solution File.*|#.*|VisualStudioVersion.*|MinimumVisualStudioVersion.*)$";
        private static readonly Regex ms_regexParseHeader = new Regex(ms_patternParseHeader);

        public SolutionFile ReadSolutionFile()
        {
            lock (this._reader)
            {
                this._solutionFile = new SolutionFile();

                var isHeader = true;
                for (var line = ReadLine(); line != null; line = ReadLine())
                {
                    if (isHeader && ms_regexParseHeader.IsMatch(line))
                    {
                        _solutionFile.Headers.Add(line);
                        continue;
                    }

                    isHeader = false;
                    if (line.StartsWith("Project(", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this._solutionFile.Projects.Add(ReadProject(line));
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
                                        this._currentLineNumber, 
                                        line));
                    }
                }
                return this._solutionFile;
            }
        }

        private string ReadLine()
        {
            string line = this._reader.ReadLine();
            if (line == null)
            {
                throw new SolutionFileException("Unexpected end of file encountered while reading the solution file.");
            }

            this._currentLineNumber++;
            return line.Trim();
        }

        private Project FindProjectByGuid(string guid, int lineNumber)
        {
            Project p = this._solutionFile.Projects.FindByGuid(guid);
            if (p == null)
            {
                throw new SolutionFileException(string.Format("Invalid guid found on line #{0}.\nFound: {1}\nExpected: A guid from one of the projects in the solution.",
                                lineNumber,
                                guid));
            }
            return p;
        }

        private static readonly string ms_patternParseProject = "^Project\\(\"(?<PROJECTTYPEGUID>.*)\"\\)\\s*=\\s*\"(?<PROJECTNAME>.*)\"\\s*,\\s*\"(?<RELATIVEPATH>.*)\"\\s*,\\s*\"(?<PROJECTGUID>.*)\"$";
        private static readonly Regex ms_regexParseProject = new Regex(ms_patternParseProject);

        private Project ReadProject(string firstLine)
        {
            var match = ms_regexParseProject.Match(firstLine);
            if (!match.Success)
            {
                throw new SolutionFileException(string.Format("Invalid format for a project on line #{0}.\nFound: {1}\nExpected: A line starting with 'Global' or respecting the pattern '{2}'.",
                                this._currentLineNumber,
                                firstLine,
                                ms_patternParseProject));
            }

            string projectTypeGuid = match.Groups["PROJECTTYPEGUID"].Value.Trim();
            string projectName = match.Groups["PROJECTNAME"].Value.Trim();
            string relativePath = match.Groups["RELATIVEPATH"].Value.Trim();
            string projectGuid = match.Groups["PROJECTGUID"].Value.Trim();

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
                ReadGlobalSection(line);
            }
        }

        private static readonly string ms_patternParseProjectSection = @"^(?<TYPE>ProjectSection)\((?<NAME>.*)\) = (?<STEP>.*)$";
        private static readonly Regex ms_regexParseProjectSection = new Regex(ms_patternParseProjectSection);

        private Section ReadProjectSection(string firstLine)
        {
            var match = ms_regexParseProjectSection.Match(firstLine);
            if (!match.Success)
            {
                throw new SolutionFileException(string.Format("Invalid format for a project section on line #{0}.\nFound: {1}\nExpected: A line starting with 'EndProject' or respecting the pattern '{2}'.",
                                this._currentLineNumber,
                                firstLine,
                                ms_patternParseProjectSection));
            }

            string type = match.Groups["TYPE"].Value.Trim();
            string name = match.Groups["NAME"].Value.Trim();
            string step = match.Groups["STEP"].Value.Trim();

            var propertyLines = new List<PropertyLine>();
            string endOfSectionToken = "End" + type;
            for (string line = ReadLine(); !line.StartsWith(endOfSectionToken, StringComparison.InvariantCultureIgnoreCase); line = ReadLine())
            {
                propertyLines.Add(ReadPropertyLine(line, endOfSectionToken));
            }
            return new Section(name, type, step, propertyLines);
        }

        private static readonly string ms_patternParseGlobalSection = @"^(?<TYPE>GlobalSection)\((?<NAME>.*)\) = (?<STEP>.*)$";
        private static readonly Regex ms_regexParseGlobalSection = new Regex(ms_patternParseGlobalSection);

        private void ReadGlobalSection(string firstLine)
        {
            var match = ms_regexParseGlobalSection.Match(firstLine);
            if (! match.Success)
            {
                throw new SolutionFileException(string.Format("Invalid format for a global section on line #{0}.\nFound: {1}\nExpected: A line starting with 'EndGlobal' or respecting the pattern '{2}'.",
                                this._currentLineNumber,
                                firstLine,
                                ms_patternParseGlobalSection));
            }

            var type = match.Groups["TYPE"].Value.Trim();
            var name = match.Groups["NAME"].Value.Trim();
            var step = match.Groups["STEP"].Value.Trim();

            var propertyLines = new List<PropertyLine>();
            var startLineNumber = this._currentLineNumber;
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
                    else
                    {
                        this._solutionFile.GlobalSections.Add(
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
            this._solutionFile.GlobalSections.Add(
                        new Section(
                            name, 
                            type, 
                            step, 
                            null));
        }

        private static readonly string ms_patternParseProjectConfigurationPlatformsName = @"^(?<GUID>\{[-0-9a-zA-Z]+\})\.(?<DESCRIPTION>.*)$";
        private static readonly Regex ms_regexParseProjectConfigurationPlatformsName = new Regex(ms_patternParseProjectConfigurationPlatformsName);

        private void HandleProjectConfigurationPlatforms(string name, string type, string step, IEnumerable<PropertyLine> propertyLines, int startLineNumber)
        {
            var currentLineNumber = startLineNumber;
            foreach (var propertyLine in propertyLines)
            {
                currentLineNumber++;
                var match = ms_regexParseProjectConfigurationPlatformsName.Match(propertyLine.Name);
                if (! match.Success)
                {
                    throw new SolutionFileException(string.Format("Invalid format for a project configuration name on line #{0}.\nFound: {1}\nExpected: A line respecting the pattern '{2}'.",
                                    this._currentLineNumber,
                                    propertyLine.Name,
                                    ms_patternParseProjectConfigurationPlatformsName));
                }

                var projectGuid = match.Groups["GUID"].Value;
                var description = match.Groups["DESCRIPTION"].Value;
                var left = FindProjectByGuid(projectGuid, currentLineNumber);
                left.ProjectConfigurationPlatformsLines.Add(
                            new PropertyLine(
                                description,
                                propertyLine.Value));
            }
            this._solutionFile.GlobalSections.Add(
                        new Section(
                            name, 
                            type, 
                            step, 
                            null));
        }

        private static readonly Regex ms_regexParseVersionControlName = new Regex(@"^(?<NAME_WITHOUT_INDEX>[a-zA-Z]*)(?<INDEX>[0-9]+)$");
        private static readonly Regex ms_regexConvertEscapedValues = new Regex(@"\\u(?<HEXACODE>[0-9a-fA-F]{4})");

        private void HandleVersionControlLines(string name, string type, string step, IEnumerable<PropertyLine> propertyLines)
        {
            var propertyLinesByIndex = new Dictionary<int, List<PropertyLine>>();
            var othersVersionControlLines = new List<PropertyLine>();
            foreach (var propertyLine in propertyLines)
            {
                var match = ms_regexParseVersionControlName.Match(propertyLine.Name);
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
                    var uniqueName = ms_regexConvertEscapedValues.Replace(uniqueNameProperty.Value, delegate(Match match)
                                {
                                    int hexaValue = int.Parse(match.Groups["HEXACODE"].Value, NumberStyles.AllowHexSpecifier);
                                    return char.ConvertFromUtf32(hexaValue);
                                });
                    uniqueName = uniqueName.Replace(@"\\", @"\");

                    Project relatedProject = null;
                    foreach (var project in this._solutionFile.Projects)
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

            this._solutionFile.GlobalSections.Add(
                    new Section(
                        name, 
                        type, 
                        step, 
                        othersVersionControlLines));
        }

        private static readonly string ms_patternParsePropertyLine = @"^(?<PROPERTYNAME>[^=]*)\s*=\s*(?<PROPERTYVALUE>[^=]*)$";
        private static readonly Regex ms_regexParsePropertyLine = new Regex(ms_patternParsePropertyLine);

        private PropertyLine ReadPropertyLine(string line, string endOfSectionToken)
        {
            var match = ms_regexParsePropertyLine.Match(line);
            if (!match.Success)
            {
                throw new SolutionFileException(string.Format("Invalid format for a property on line #{0}.\nFound: {1}\nExpected: A line starting with '{2}' or respecting the pattern '{3}'.",
                                this._currentLineNumber,
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
