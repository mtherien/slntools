using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CWDev.SLNTools.Core.Merge
{
    public delegate bool ShouldBeRemovedHandler(
                Difference difference);

    public class NodeDifference : Difference
    {
        public NodeDifference(
                    ElementIdentifier identifier,
                    OperationOnParent operationOnParent,
                    IEnumerable<Difference> subdifferences)
            : base(identifier, operationOnParent)
        {
            if (subdifferences == null)
                throw new ArgumentNullException("subdifferences");

            if (operationOnParent == OperationOnParent.Removed)
            {
                m_subdifferences = new ReadOnlyCollection<Difference>(new List<Difference>());
            }
            else
            {
                m_subdifferences = new ReadOnlyCollection<Difference>(new List<Difference>(subdifferences));
            }
        }

        private ReadOnlyCollection<Difference> m_subdifferences;

        public ReadOnlyCollection<Difference> Subdifferences { get { return m_subdifferences; } }

        public override Conflict CompareTo(Difference destinationDifference)
        {
            NodeDifference source = this;
            NodeDifference destination = destinationDifference as NodeDifference;
            if (destination == null)
                throw new Exception("TODO");
            if (!source.Identifier.Equals(destination.Identifier))
                throw new Exception("TODO");

            if (source.OperationOnParent != destination.OperationOnParent)
            {
                return new TypeDifferenceConflict(
                            source,
                            destination);
            }
            else
            {
                List<Conflict> subconflicts = new List<Conflict>();
                DifferenceHashList acceptedSubdifferences = new DifferenceHashList();
                foreach (Difference destinationSubdifference in destination.Subdifferences)
                {
                    // Add all the destinationBranchDifferences to the acceptedSubdifferences (they might be removed from the list later).
                    acceptedSubdifferences.Add(destinationSubdifference);
                }
                foreach (Difference sourceSubdifference in source.Subdifferences)
                {
                    if (!acceptedSubdifferences.Contains(sourceSubdifference.Identifier))
                    {
                        // This is a new difference that is not present in the destination branch, add the difference to the acceptedSubdifferences.
                        acceptedSubdifferences.Add(sourceSubdifference);
                    }
                    else
                    {
                        Difference destinationSubdifference = acceptedSubdifferences[sourceSubdifference.Identifier];
                        Conflict conflict = sourceSubdifference.CompareTo(destinationSubdifference);
                        if (conflict != null)
                        {
                            NodeConflict nodeConflict = conflict as NodeConflict;
                            if ((nodeConflict != null) && (nodeConflict.Subconflicts.Count == 0))
                            {
                                acceptedSubdifferences.Remove(sourceSubdifference.Identifier);
                                acceptedSubdifferences.Add(
                                            new NodeDifference(
                                                nodeConflict.Identifier, 
                                                nodeConflict.OperationOnParent, 
                                                nodeConflict.AcceptedSubdifferences));
                            }
                            else
                            {
                                acceptedSubdifferences.Remove(sourceSubdifference.Identifier);
                                subconflicts.Add(conflict);
                            }
                        }
                    }
                }

                return new NodeConflict(
                            source.Identifier, 
                            source.OperationOnParent, 
                            acceptedSubdifferences, 
                            subconflicts);
            }
        }

        public void Remove(ShouldBeRemovedHandler shouldBeRemovedHandler)
        {
            List<Difference> filteredSubdifferences = new List<Difference>();
            foreach (Difference subdifference in m_subdifferences)
            {
                if (shouldBeRemovedHandler(subdifference))
                {
                    // Do nothing
                }
                else if (subdifference is NodeDifference)
                {
                    NodeDifference nodeSubdifference = subdifference as NodeDifference;
                    int nbSubdiffsBefore = nodeSubdifference.Subdifferences.Count;
                    nodeSubdifference.Remove(shouldBeRemovedHandler);
                    if ((nodeSubdifference.Subdifferences.Count > 0) || (nbSubdiffsBefore == 0))
                    {
                        filteredSubdifferences.Add(subdifference);
                    }
                }
                else
                {
                    filteredSubdifferences.Add(subdifference);
                }
            }
            m_subdifferences = filteredSubdifferences.AsReadOnly();
        }

        public override string ToString()
        {
            return string.Format("{0} has been {1}", this.Identifier, this.OperationOnParent.ToString().ToLower());
        }
    }
}
