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
