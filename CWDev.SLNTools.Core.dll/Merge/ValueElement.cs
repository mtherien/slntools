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

namespace CWDev.SLNTools.Core.Merge
{
    public class ValueElement : Element
    {
        public ValueElement(
                    ElementIdentifier identifier, 
                    string value)
            : base(identifier)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            m_value = value;
        }

        private string m_value;

        public string Value { get { return m_value; } }

        public override Difference CompareTo(Element oldElement)
        {
            if (oldElement == null)
                throw new ArgumentNullException("oldElement");
            if (!oldElement.Identifier.Equals(this.Identifier))
                throw new MergeException("Cannot compare elements that does not share the same identifier.");

            if (oldElement is EmptyElement)
            {
                return new ValueDifference(
                            this.Identifier,
                            OperationOnParent.Added,
                            null,
                            m_value);
            }
            else if (oldElement is ValueElement)
            {
                string oldValue = ((ValueElement)oldElement).Value;
                if (oldValue != m_value)
                {
                    return new ValueDifference(
                                this.Identifier,
                                OperationOnParent.Modified,
                                oldValue,
                                m_value);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new MergeException(string.Format("Cannot compare a {0} to a {1}.", oldElement.GetType().Name, this.GetType().Name));
            }
        }

        public override Element Apply(Difference difference)
        {
            if (difference == null)
                throw new ArgumentNullException("difference");
            if (!difference.Identifier.Equals(this.Identifier))
                throw new MergeException("Cannot apply a difference that does not share the same identifier with the element.");

            if (difference is ValueDifference)
            {
                return new ValueElement(
                            this.Identifier, 
                            ((ValueDifference)difference).NewValue);
            }
            else
            {
                throw new MergeException(string.Format("Cannot apply a {0} on a {1}.", difference.GetType().Name, this.GetType().Name));
            }
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}", this.Identifier, m_value);
        }
    }
}
