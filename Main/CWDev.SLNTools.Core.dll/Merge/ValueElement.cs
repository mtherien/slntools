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
            m_value = value;
        }

        private string m_value;

        public string Value { get { return m_value; } }

        public override Element CreateEmptyClone()
        {
            return new ValueElement(this.Identifier, null);
        }

        public override Difference CompareTo(Element oldElement)
        {
            ValueElement old = oldElement as ValueElement;
            if (old == null)
                throw new Exception("TODO");
            if (!old.Identifier.Equals(this.Identifier))
                throw new Exception("TODO");

            if (this.Value != old.Value)
            {
                OperationOnParent operationOnParent;
                if (old.Value == null)
                    operationOnParent = OperationOnParent.Added;
                else if (this.Value == null)
                    operationOnParent = OperationOnParent.Removed;
                else
                    operationOnParent = OperationOnParent.Modified;
                return new ValueDifference(this.Identifier, operationOnParent, old.Value, this.Value);
            }
            else
            {
                return null;
            }
        }
    }
}
