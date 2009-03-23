using System;

namespace CWDev.VSSolutionTools.Core.Merge
{
    public abstract class Difference
    {
        protected Difference(
                    ElementIdentifier identifier, 
                    OperationOnParent operationOnParent)
        {
            if (identifier == null)
                throw new ArgumentNullException("identifier");
            if (! Enum.IsDefined(operationOnParent.GetType(), operationOnParent))
                throw new ArgumentNullException("TODO operationOnParent");
            m_identifier = identifier;
            m_operationOnParent = operationOnParent;
        }

        private ElementIdentifier m_identifier;
        private OperationOnParent m_operationOnParent;

        public ElementIdentifier Identifier { get { return m_identifier; } }
        public OperationOnParent OperationOnParent { get { return m_operationOnParent; } }

        public abstract Conflict CompareTo(Difference destinationDifference);
    }
}
