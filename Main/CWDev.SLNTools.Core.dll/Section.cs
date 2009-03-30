using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CWDev.SLNTools.Core
{
    using System;
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
                        throw new Exception("TODO");

                    m_sectionType = difference.NewValue;
                } 
                else if (identifier.Name == "Step")
                {
                    if (difference.OperationOnParent == OperationOnParent.Removed)
                        throw new Exception("TODO");

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
                    throw new Exception("TODO");
                }
            }
        }
    }
}
