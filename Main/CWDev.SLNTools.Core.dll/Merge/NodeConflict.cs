using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CWDev.SLNTools.Core.Merge
{
    public class NodeConflict : Conflict
    {
        public NodeConflict(
                    ElementIdentifier identifier, 
                    OperationOnParent operationOnParent, 
                    IEnumerable<Difference> acceptedSubdifferences, 
                    IEnumerable<Conflict> subconflicts)
            : base(identifier)
        {
            if (!Enum.IsDefined(operationOnParent.GetType(), operationOnParent))
                throw new ArgumentNullException("TODO operationOnParent");
            if (acceptedSubdifferences == null)
                throw new NullReferenceException("acceptedSubdifferences");
            if (subconflicts == null)
                throw new NullReferenceException("subconflicts");

            m_operationOnParent = operationOnParent;
            m_acceptedSubdifferences = new List<Difference>(acceptedSubdifferences);
            m_subconflicts = new List<Conflict>(subconflicts);
        }

        private OperationOnParent m_operationOnParent;
        private List<Difference> m_acceptedSubdifferences;
        private List<Conflict> m_subconflicts;

        public OperationOnParent OperationOnParent { get { return m_operationOnParent; } }
        public ReadOnlyCollection<Difference> AcceptedSubdifferences { get { return m_acceptedSubdifferences.AsReadOnly(); } }
        public ReadOnlyCollection<Conflict> Subconflicts { get { return m_subconflicts.AsReadOnly(); } }

        public override Difference Resolve(
                    TypeDifferenceConflictResolver typeDifferenceConflictResolver,
                    ValueConflictResolver valueConflictResolver)
        {
            foreach (Conflict subconflict in new List<Conflict>(m_subconflicts)) // Iterate on a copy of the list to be able to modify the original list in the loop
            {
                Difference resolvedDifference = subconflict.Resolve(typeDifferenceConflictResolver, valueConflictResolver);
                if (resolvedDifference != null)
                {
                    m_subconflicts.Remove(subconflict);
                    m_acceptedSubdifferences.Add(resolvedDifference);
                }
            }
            if (m_subconflicts.Count == 0)
            {
                return new NodeDifference(
                            this.Identifier,
                            m_operationOnParent,
                            m_acceptedSubdifferences);
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} element conflict in {1}:", this.OperationOnParent, this.Identifier);
        }
    }
}
