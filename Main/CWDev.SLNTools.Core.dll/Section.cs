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
using System.Text;
using System.Text.RegularExpressions;

namespace CWDev.SLNTools.Core
{
    using Merge;

    public abstract class Section
    {
        protected Section(Section original)
        {
            m_name = original.Name;
            m_sectionType = original.SectionType;
            m_step = original.Step;
            m_propertyLines = new PropertyLineHashList(original.PropertyLines);
        }

        protected Section(string name, string sectionType, string step, IEnumerable<PropertyLine> propertyLines)
        {
            m_name = name;
            m_sectionType = sectionType;
            m_step = step;
            m_propertyLines = new PropertyLineHashList(propertyLines);
        }

        private string m_name;
        private string m_sectionType;
        private string m_step;
        private PropertyLineHashList m_propertyLines;

        public string Name { get { return m_name; } }
        public string SectionType { get { return m_sectionType; } }
        public string Step { get { return m_step; } }
        public ReadOnlyCollection<PropertyLine> PropertyLines { get { return m_propertyLines.AsReadOnly(); } }

        public override string ToString()
        {
            return string.Format("{0} '{1}'", this.SectionType, this.Name);
        }

        #region public: Methods ToElement / FromElementToConstructorArgument

        private const string TagSectionType = "SectionType";
        private const string TagStep = "Step";
        private const string TagPropertyLines = "L_";

        public NodeElement ToElement(ElementIdentifier identifier)
        {
            ElementHashList childs = new ElementHashList();
            childs.Add(new ValueElement(new ElementIdentifier(TagSectionType), this.SectionType));
            childs.Add(new ValueElement(new ElementIdentifier(TagStep), this.Step));
            foreach (PropertyLine propertyLine in this.PropertyLines)
            {
                ElementIdentifier lineIdentifier = new ElementIdentifier(
                            TagPropertyLines + propertyLine.Name,
                            @"Line\" + propertyLine.Name);
                if ((m_name == "WebsiteProperties") && (propertyLine.Name == "ProjectReferences"))
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

        protected static void FromElementToConstructorArgument(
                    string name, 
                    NodeElement element,
                    out string sectionType,
                    out string step,
                    out PropertyLineHashList propertyLines)
        {
            sectionType = null;
            step = null;
            propertyLines = new PropertyLineHashList();

            foreach (Element child in element.Childs)
            {
                ElementIdentifier identifier = child.Identifier;
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
                    string lineName = identifier.Name.Substring(TagPropertyLines.Length);
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
        }

        private static ElementHashList ConvertProjectReferencesValueToHashList(string value)
        {
            ElementHashList references = new ElementHashList();
            string pattern = "^\"((?<ReferenceGuid>[^|]+)\\|(?<ReferenceName>[^;]*)(;)?)*\"$";
            Match match = Regex.Match(value, pattern);
            if (!match.Success)
            {
                throw new SolutionFileException(string.Format("Invalid format for a ProjectReferences line value.\nFound: {0}\nExpected: A value respecting the pattern '{1}'.",
                                value,
                                pattern));
            }

            CaptureCollection capturesGuid = match.Groups["ReferenceGuid"].Captures;
            CaptureCollection capturesName = match.Groups["ReferenceName"].Captures;
            for (int i = 0; i < capturesGuid.Count; i++)
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
            StringBuilder lineValue = new StringBuilder();
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