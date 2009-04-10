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

        public NodeElement GetElement(ElementIdentifier identifier)
        {
            ElementHashList elements = new ElementHashList();
            elements.Add(new ValueElement(new ElementIdentifier("SectionType"), this.SectionType));
            elements.Add(new ValueElement(new ElementIdentifier("Step"), this.Step));
            foreach (PropertyLine propertyLine in this.PropertyLines)
            {
                ElementIdentifier lineIdentifier = new ElementIdentifier(
                            "L_" + propertyLine.Name,
                            @"Line\" + propertyLine.Name);
                if ((m_name == "WebsiteProperties") && (propertyLine.Name == "ProjectReferences"))
                {
                    ElementHashList references = new ElementHashList();
                    string pattern = "^\"((?<ReferenceGuid>[^|]+)\\|(?<ReferenceName>[^;]*)(;)?)*\"$";
                    Match match = Regex.Match(propertyLine.Value, pattern);
                    if (!match.Success)
                    {
                        throw new SolutionFileException(string.Format("Invalid format for a ProjectReferences line value.\nFound: {0}\nExpected: A line respecting the pattern '{1}'.",
                                        propertyLine.Value,
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
                    elements.Add(
                                new NodeElement(
                                    lineIdentifier,
                                    references));
                }
                else
                {
                    elements.Add(
                                new ValueElement(
                                    lineIdentifier,
                                    propertyLine.Value));
                }
            }
            return new NodeElement(
                            identifier,
                            elements);
        }

        public Section(string name, NodeElement element)
        {
            m_name = name;
            m_sectionType = null;
            m_step = null;
            m_propertyLines = new PropertyLineHashList();

            foreach (Element child in element.Childs)
            {
                ElementIdentifier identifier = child.Identifier;
                if (identifier.Name == "SectionType")
                {
                    m_sectionType = ((ValueElement)child).Value;
                }
                else if (identifier.Name == "Step")
                {
                    m_step = ((ValueElement)child).Value;
                }
                else if (identifier.Name.StartsWith("L_"))
                {
                    string lineName = identifier.Name.Substring(2);
                    if ((m_name == "WebsiteProperties") && (lineName == "ProjectReferences"))
                    {
                        StringBuilder lineValue = new StringBuilder();
                        lineValue.Append("\"");
                        foreach (ValueElement reference in ((NodeElement)child).Childs)
                        {
                            lineValue.AppendFormat("{0}|{1};", reference.Identifier.Name, reference.Value);
                        }
                        lineValue.Append("\"");
                        m_propertyLines.Add(new PropertyLine(lineName, lineValue.ToString()));
                    }
                    else
                    {
                        string lineValue = ((ValueElement)child).Value;
                        m_propertyLines.Add(new PropertyLine(lineName, lineValue));
                    }
                }
                else
                {
                    throw new Exception(string.Format("Invalid identifier '{0}'.", identifier.Name));
                }
            }

            if (m_sectionType == null)
                throw new Exception("TODO element doesn't containt SectionType");
            if (m_step == null)
                throw new Exception("TODO element doesn't containt Step");
        }
    }
}