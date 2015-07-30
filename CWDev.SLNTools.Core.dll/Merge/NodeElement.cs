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
        private readonly ElementHashList r_childs;

        public NodeElement(
                    ElementIdentifier identifier,
                    IEnumerable<Element> childs)
            : base(identifier)
        {
            r_childs = new ElementHashList(childs);
        }

        public ElementHashList Childs { get { return r_childs; } }

        public override Difference CompareTo(Element oldElement)
        {
            if (oldElement == null)
                throw new ArgumentNullException("oldElement");
            if (!oldElement.Identifier.Equals(this.Identifier))
                throw new MergeException("Cannot compare elements that does not share the same identifier.");

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
                throw new MergeException(string.Format("Cannot compare a {0} to a {1}.", oldElement.GetType().Name, this.GetType().Name));
            }

            var differences = new List<Difference>();
            var newChilds = this.Childs;
            foreach (var oldChild in oldChilds)
            {
                var newChild = newChilds.Contains(oldChild.Identifier)
                            ? newChilds[oldChild.Identifier]
                            : oldChild.Identifier.CreateEmptyElement();

                var difference = newChild.CompareTo(oldChild);
                if (difference != null)
                {
                    differences.Add(difference);
                }
            }
            foreach (var newChild in newChilds)
            {
                if (!oldChilds.Contains(newChild.Identifier))
                {
                    Element oldChild = newChild.Identifier.CreateEmptyElement();
                    differences.Add(newChild.CompareTo(oldChild));
                }
            }

            return differences.Count > 0
                        ? new NodeDifference(this.Identifier, operationOnParent, differences)
                        : null;
        }

        public override Element Apply(Difference difference)
        {
            if (difference == null)
                throw new ArgumentNullException("difference");
            if (!difference.Identifier.Equals(this.Identifier))
                throw new MergeException("Cannot apply a difference that does not share the same identifier with the element.");

            if (difference is NodeDifference)
            {
                var mergedChilds = new ElementHashList(r_childs);
                foreach (var subdifference in ((NodeDifference)difference).Subdifferences)
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
                            throw new ArgumentOutOfRangeException("subdifference.OperationOnParent", subdifference.OperationOnParent, "Invalid value");
                    }
                }
                return new NodeElement(this.Identifier, mergedChilds);
            }
            else
            {
                throw new MergeException(string.Format("Cannot apply a {0} on a {1}.", difference.GetType().Name, this.GetType().Name));
            }
        }

        public override string ToString()
        {
            return this.Identifier.FriendlyName;
        }
    }
}
