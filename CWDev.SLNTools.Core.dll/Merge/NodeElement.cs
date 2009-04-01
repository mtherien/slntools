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
                throw new Exception("TODO");

            m_childs = childs;
        }

        private ElementHashList m_childs;

        public ElementHashList Childs { get { return m_childs; } }

        public override Element CreateEmptyClone()
        {
            return new EmptyNodeElement(this.Identifier);
        }

        public override Difference CompareTo(Element oldElement)
        {
            NodeElement old = oldElement as NodeElement;
            if (old == null)
                throw new Exception("TODO");
            if (! old.Identifier.Equals(this.Identifier))
                throw new Exception("TODO");

            List<Difference> differences = new List<Difference>();
            ElementHashList newChilds = this.Childs;
            foreach (Element oldChild in old.Childs)
            {
                Element newChild;
                if (newChilds.Contains(oldChild.Identifier))
                {
                    newChild = newChilds[oldChild.Identifier];
                }
                else
                {
                    newChild = oldChild.CreateEmptyClone();
                }

                Difference difference = newChild.CompareTo(oldChild);
                if (difference != null)
                {
                    differences.Add(difference);
                }
            }
            foreach (Element newChild in newChilds)
            {
                if (!old.Childs.Contains(newChild.Identifier))
                {
                    Element oldChild = newChild.CreateEmptyClone();
                    differences.Add(newChild.CompareTo(oldChild));
                }
            }

            if (differences.Count > 0)
            {
                OperationOnParent operationOnParent;
                if (this is EmptyNodeElement)
                    operationOnParent = OperationOnParent.Removed;
                else if (old is EmptyNodeElement)
                    operationOnParent = OperationOnParent.Added;
                else
                    operationOnParent = OperationOnParent.Modified;
                return new NodeDifference(this.Identifier, operationOnParent, differences);
            }
            else
            {
                return null;
            }
        }
    }
}
