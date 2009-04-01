using System.Collections.Generic;

namespace CWDev.SLNTools.Core.Merge
{
    public delegate string ValueConflictResolver(
                ConflictContext context,
                string latestValueInSourceBranch,
                string latestValueInDestinationBranch);
    public delegate Difference TypeDifferenceConflictResolver(
                ConflictContext context,
                Difference differenceInSourceBranch,
                Difference differenceInDestinationBranch);

    public abstract class Conflict
    {
        public static NodeConflict Merge(
                    NodeElement rootElement,
                    NodeElement latestElementInSourceBranch,
                    NodeElement latestElementInDestinationBranch,
                    out NodeDifference differenceInSourceBranch,
                    out NodeDifference differenceInDestinationBranch)
        {
            differenceInSourceBranch = (NodeDifference) latestElementInSourceBranch.CompareTo(rootElement);
            if (differenceInSourceBranch == null)
            {
                differenceInSourceBranch = new NodeDifference(new ElementIdentifier(""), OperationOnParent.Modified, new List<Difference>());
            }

            differenceInDestinationBranch = (NodeDifference) latestElementInDestinationBranch.CompareTo(rootElement);
            if (differenceInDestinationBranch == null)
            {
                differenceInDestinationBranch = new NodeDifference(new ElementIdentifier(""), OperationOnParent.Modified, new List<Difference>());
            }

            return (NodeConflict)differenceInSourceBranch.CompareTo(differenceInDestinationBranch);
        }

        protected Conflict(ElementIdentifier identifier)
        {
            m_identifier = identifier;
        }

        private ElementIdentifier m_identifier;

        public ElementIdentifier Identifier { get { return m_identifier; } }

        public Difference Resolve(
                    TypeDifferenceConflictResolver typeDifferenceConflictResolver,
                    ValueConflictResolver valueConflictResolver)
        {
            return Resolve(new ConflictContext(), typeDifferenceConflictResolver, valueConflictResolver);
        }

        public abstract Difference Resolve(
                        ConflictContext context,
                        TypeDifferenceConflictResolver typeDifferenceConflictResolver,
                        ValueConflictResolver valueConflictResolver);
    }
}
