using System;

namespace CWDev.SLNTools.Core.Merge
{
    public class ValueDifference : Difference
    {
        public ValueDifference(
                    ElementIdentifier identifier,
                    OperationOnParent operationOnParent,
                    string oldValue,
                    string newValue)
            : base(identifier, operationOnParent)
        {
            if (oldValue == newValue)
                throw new Exception("TODO"); // This also handle the null/null case

            m_oldValue = oldValue;
            m_newValue = newValue;
        }

        private string m_oldValue;
        private string m_newValue;

        public string OldValue { get { return m_oldValue; } }
        public string NewValue { get { return m_newValue; } }

        public override Conflict CompareTo(Difference destinationDifference)
        {
            ValueDifference source = this;
            ValueDifference destination = destinationDifference as ValueDifference;
            if (destination == null)
                throw new Exception("TODO");
            if (!source.Identifier.Equals(destination.Identifier))
                throw new Exception("TODO");
            if (source.OldValue != destination.OldValue)
                throw new Exception("TODO");

            if (source.OperationOnParent != destination.OperationOnParent)
            {
                return new TypeDifferenceConflict(
                            source, 
                            destination);
            }
            else if (source.NewValue != destination.NewValue)
            {
                return new ValueConflict(
                            source.Identifier, 
                            source.OperationOnParent,
                            source.OldValue, 
                            source.NewValue, 
                            destination.NewValue);
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            switch (this.OperationOnParent)
            {
                case OperationOnParent.Added:
                    return string.Format("{0} has been added with the value \"{1}\".", this.Identifier, this.NewValue);                

                case OperationOnParent.Modified:
                    return string.Format("{0} value modified from \"{1}\" to \"{2}\".", this.Identifier, this.OldValue, this.NewValue);

                case OperationOnParent.Removed:
                    return string.Format("{0} has been removed. Old value = \"{1}\".", this.Identifier, this.OldValue);

                default:
                    throw new Exception("TODO");
            }
        }
    }
}
