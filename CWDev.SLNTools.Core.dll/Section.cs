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
            m_propertyLines = new PropertyLineList(original.PropertyLines);
        }

        protected Section(string name, string sectionType, string step, IEnumerable<PropertyLine> propertyLines)
        {
            m_name = name;
            m_sectionType = sectionType;
            m_step = step;
            m_propertyLines = new PropertyLineList(propertyLines);
        }

        public Section(Section original, IEnumerable<Difference> differences)
            : this(original)
        {
            ApplyDifferences(differences);
        }

        public Section(string name, IEnumerable<Difference> differences)
        {
            m_name = name;
            m_sectionType = "<undefined>";
            m_step = "<undefined>";
            m_propertyLines = new PropertyLineList();
            ApplyDifferences(differences);
        }

        private string m_name;
        private string m_sectionType;
        private string m_step;
        private PropertyLineList m_propertyLines;

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
                elements.Add(
                            new ValueElement(
                                new ElementIdentifier(
                                    "L_" + propertyLine.Name,
                                    @"Line\" + propertyLine.Name), 
                                propertyLine.Value));
            }
            return new NodeElement(
                            identifier,
                            elements);
        }

        private void ApplyDifferences(IEnumerable<Difference> differences)
        {
            foreach (ValueDifference difference in differences)
            {
                ElementIdentifier identifier = difference.Identifier;
                if (identifier.Name == "SectionType")
                {
                    if (difference.OperationOnParent == OperationOnParent.Removed)
                        throw new Exception("Cannot remove the SectionType attribute of a section.");

                    m_sectionType = difference.NewValue;
                } 
                else if (identifier.Name == "Step")
                {
                    if (difference.OperationOnParent == OperationOnParent.Removed)
                        throw new Exception("Cannot remove the Step attribute of a section.");

                    m_step = difference.NewValue;
                }
                else if (identifier.Name.StartsWith("L_"))
                {
                    string name = identifier.Name.Substring(2);
                    string value = difference.NewValue;
                    m_propertyLines.ModifyLine(name, value);
                }
                else
                {
                    throw new Exception(string.Format("Invalid identifier '{0}'.", identifier.Name));
                }
            }
        }
    }
}
