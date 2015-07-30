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

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CWDev.SLNTools.Core
{
    using Merge;

    public class Section
    {
        private readonly string r_name;
        private string m_sectionType;
        private string m_step;
        private readonly PropertyLineHashList r_propertyLines;

        public Section(Section original)
            : this(original.Name, original.SectionType, original.Step, original.PropertyLines)
        {
        }

        public Section(string name, string sectionType, string step, IEnumerable<PropertyLine> propertyLines)
        {
            r_name = name;
            m_sectionType = sectionType;
            m_step = step;
            r_propertyLines = new PropertyLineHashList(propertyLines);
        }

        public string Name
        {
            get { return r_name; }
        }
        public string SectionType
        {
            get { return m_sectionType; }
            set { m_sectionType = value; }
        }
        public string Step
        {
            get { return m_step; }
            set { m_step = value; }
        }
        public PropertyLineHashList PropertyLines
        {
            get { return r_propertyLines; }
        }

        public override string ToString()
        {
            return string.Format("{0} '{1}'", this.SectionType, this.Name);
        }

        #region public: Methods ToElement / FromElement

        private const string TagSectionType = "SectionType";
        private const string TagStep = "Step";
        private const string TagPropertyLines = "L_";

        public NodeElement ToElement(ElementIdentifier identifier)
        {
            var childs = new List<Element>();
            childs.Add(new ValueElement(new ElementIdentifier(TagSectionType), this.SectionType));
            childs.Add(new ValueElement(new ElementIdentifier(TagStep), this.Step));
            foreach (var propertyLine in this.PropertyLines)
            {
                var lineIdentifier = new ElementIdentifier(
                            TagPropertyLines + propertyLine.Name,
                            @"Line\" + propertyLine.Name);
                if ((r_name == "WebsiteProperties") && (propertyLine.Name == "ProjectReferences"))
                {
                    childs.Add(new NodeElement(lineIdentifier, ConvertProjectReferencesValueToHashList(propertyLine.Value)));
                }
                else
                {
                    childs.Add(new ValueElement(lineIdentifier, propertyLine.Value));
                }
            }
            return new NodeElement(identifier, childs);
        }

        public static Section FromElement(
                    string name,
                    NodeElement element)
        {
            string sectionType = null;
            string step = null;
            var propertyLines = new List<PropertyLine>();

            foreach (var child in element.Childs)
            {
                var identifier = child.Identifier;
                if (identifier.Name == TagSectionType)
                {
                    sectionType = ((ValueElement)child).Value;
                }
                else if (identifier.Name == TagStep)
                {
                    step = ((ValueElement)child).Value;
                }
                else if (identifier.Name.StartsWith(TagPropertyLines))
                {
                    var lineName = identifier.Name.Substring(TagPropertyLines.Length);
                    string lineValue;
                    if ((name == "WebsiteProperties") && (lineName == "ProjectReferences"))
                    {
                        lineValue = ConvertHashListToProjectReferencesValue(((NodeElement)child).Childs);
                    }
                    else
                    {
                        lineValue = ((ValueElement)child).Value;
                    }
                    propertyLines.Add(new PropertyLine(lineName, lineValue));
                }
                else
                {
                    throw new SolutionFileException(string.Format("Invalid identifier '{0}'.", identifier.Name));
                }
            }

            if (sectionType == null)
                throw new SolutionFileException(string.Format("Missing subelement '{0}' in a section element.", TagSectionType));
            if (step == null)
                throw new SolutionFileException(string.Format("Missing subelement '{0}' in a section element.", TagStep));

            return new Section(name, sectionType, step, propertyLines);
        }

        private static List<Element> ConvertProjectReferencesValueToHashList(string value)
        {
            var references = new List<Element>();
            const string pattern = "^\"((?<ReferenceGuid>[^|]+)\\|(?<ReferenceName>[^;]*)(;)?)*\"$";
            var match = Regex.Match(value, pattern);
            if (!match.Success)
            {
                throw new SolutionFileException(string.Format("Invalid format for a ProjectReferences line value.\nFound: {0}\nExpected: A value respecting the pattern '{1}'.",
                                value,
                                pattern));
            }

            var capturesGuid = match.Groups["ReferenceGuid"].Captures;
            var capturesName = match.Groups["ReferenceName"].Captures;
            for (var i = 0; i < capturesGuid.Count; i++)
            {
                references.Add(
                            new ValueElement(
                                new ElementIdentifier(capturesGuid[i].Value),
                                capturesName[i].Value));
            }
            return references;
        }

        private static string ConvertHashListToProjectReferencesValue(IEnumerable<Element> childs)
        {
            var lineValue = new StringBuilder();
            lineValue.Append("\"");
            foreach (ValueElement reference in childs)
            {
                lineValue.AppendFormat("{0}|{1};", reference.Identifier.Name, reference.Value);
            }
            lineValue.Append("\"");
            return lineValue.ToString();
        }

        #endregion
    }
}