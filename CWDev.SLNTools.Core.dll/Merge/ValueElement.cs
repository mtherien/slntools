using System;

namespace CWDev.VSSolutionTools.Core.Merge
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
