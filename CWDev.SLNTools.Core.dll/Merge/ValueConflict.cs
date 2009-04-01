using System;

namespace CWDev.SLNTools.Core.Merge
{
    public class ValueConflict : Conflict
    {
        public ValueConflict(
                    ElementIdentifier identifier,
                    OperationOnParent operationOnParent,
                    string rootValue,
                    string newValueInSourceBranch,
                    string newValueInDestinationBranch)
            : base(identifier)
        {
            if (!Enum.IsDefined(operationOnParent.GetType(), operationOnParent))
                throw new ArgumentNullException("TODO operationOnParent");

            m_operationOnParent = operationOnParent;
            m_oldValue = rootValue;
            m_newValueInSourceBranch = newValueInSourceBranch;
            m_newValueInDestinationBranch = newValueInDestinationBranch;
        }

        private OperationOnParent m_operationOnParent;
        private string m_oldValue;
        private string m_newValueInSourceBranch;
        private string m_newValueInDestinationBranch;

        public OperationOnParent OperationOnParent { get { return m_operationOnParent; } }
        public string OldValue { get { return m_oldValue; } }
        public string NewValueInSourceBranch { get { return m_newValueInSourceBranch; } }
        public string NewValueInDestinationBranch { get { return m_newValueInDestinationBranch; } }

        public override Difference Resolve(
                    ConflictContext context,
                    TypeDifferenceConflictResolver typeDifferenceConflictResolver,
                    ValueConflictResolver valueConflictResolver)
        {
            string resolvedValue = valueConflictResolver(context.CreateSubcontext(this), m_newValueInSourceBranch, m_newValueInDestinationBranch);
            if (resolvedValue != null)
            {
                return new ValueDifference(this.Identifier, this.OperationOnParent, m_oldValue, resolvedValue);
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return string.Format("Both branches {0} {1} but with different values: Source = \"{2}\" and Destination = \"{3}\".",
                        this.OperationOnParent.ToString().ToLower(),
                        this.Identifier,
                        this.NewValueInSourceBranch,
                        this.NewValueInDestinationBranch);
        }
    }
}
