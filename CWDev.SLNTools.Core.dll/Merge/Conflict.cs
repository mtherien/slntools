using System.Collections.Generic;

namespace CWDev.VSSolutionTools.Core.Merge
{
    public delegate string ValueConflictResolver(
                string conflictDescription,
                string latestValueInSourceBranch,
                string latestValueInDestinationBranch);
    public delegate Difference TypeDifferenceConflictResolver(
                string conflictDescription,
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
       
        public abstract Difference Resolve(
                        TypeDifferenceConflictResolver typeDifferenceConflictResolver,
                        ValueConflictResolver valueConflictResolver);
    }
}
