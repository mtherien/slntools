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

namespace CWDev.SLNTools.Core.Merge
{
    public class NodeElement : Element
    {
        public NodeElement(
                    ElementIdentifier identifier,
                    ElementHashList childs)
            : base(identifier)
        {
            if (childs == null)
                throw new ArgumentNullException("childs");

            m_childs = childs;
        }

        private ElementHashList m_childs;

        public ElementHashList Childs { get { return m_childs; } }

        public override Difference CompareTo(Element oldElement)
        {
            if (oldElement == null)
                throw new ArgumentNullException("oldElement");
            if (!oldElement.Identifier.Equals(this.Identifier))
                throw new Exception("Cannot compare elements that does not share the same identifier.");

            OperationOnParent operationOnParent;
            ElementHashList oldChilds;
            if (oldElement is EmptyElement)
            {
                operationOnParent = OperationOnParent.Added;
                oldChilds = new ElementHashList();
            }
            else if (oldElement is NodeElement)
            {
                operationOnParent = OperationOnParent.Modified;
                oldChilds = ((NodeElement)oldElement).Childs;
            }
            else
            {
                throw new Exception("TODO cannot compare value with node");
            }

            List<Difference> differences = new List<Difference>();
            ElementHashList newChilds = this.Childs;
            foreach (Element oldChild in oldChilds)
            {
                Element newChild;
                if (newChilds.Contains(oldChild.Identifier))
                {
                    newChild = newChilds[oldChild.Identifier];
                }
                else
                {
                    newChild = oldChild.Identifier.CreateEmptyElement();
                }

                Difference difference = newChild.CompareTo(oldChild);
                if (difference != null)
                {
                    differences.Add(difference);
                }
            }
            foreach (Element newChild in newChilds)
            {
                if (!oldChilds.Contains(newChild.Identifier))
                {
                    Element oldChild = newChild.Identifier.CreateEmptyElement();
                    differences.Add(newChild.CompareTo(oldChild));
                }
            }

            if (differences.Count > 0)
            {
                return new NodeDifference(this.Identifier, operationOnParent, differences);
            }
            else
            {
                return null;
            }
        }

        public override Element Apply(Difference difference)
        {
            if (difference == null)
                throw new ArgumentNullException("difference");
            if (!difference.Identifier.Equals(this.Identifier))
                throw new Exception("Cannot apply a difference that does not share the same identifier with the element.");

            if (difference is NodeDifference)
            {
                ElementHashList mergedChilds = new ElementHashList(m_childs);
                foreach (Difference subdifference in ((NodeDifference)difference).Subdifferences)
                {
                    switch (subdifference.OperationOnParent)
                    {
                        case OperationOnParent.Added:
                            mergedChilds.Add(subdifference.Identifier.CreateEmptyElement().Apply(subdifference));
                            break;
                        case OperationOnParent.Modified:
                            mergedChilds.AddOrUpdate(mergedChilds[subdifference.Identifier].Apply(subdifference));
                            break;
                        case OperationOnParent.Removed:
                            mergedChilds.Remove(subdifference.Identifier);
                            break;
                        default:
                            throw new Exception("TODO");
                    }
                }
                return new NodeElement(this.Identifier, mergedChilds);
            }
            else
            {
                throw new ArgumentException("TODO cannot apply difference.GetType() on a NodeElement");
            }
        }

        public override string ToString()
        {
            return this.Identifier.FriendlyName;
        }
    }
}
