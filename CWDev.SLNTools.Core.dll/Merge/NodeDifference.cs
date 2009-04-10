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

            m_subdifferences = new ReadOnlyCollection<Difference>(new List<Difference>(subdifferences));
        }

        private ReadOnlyCollection<Difference> m_subdifferences;

        public ReadOnlyCollection<Difference> Subdifferences { get { return m_subdifferences; } }

        public override Conflict CompareTo(Difference destinationDifference)
        {
            if (destinationDifference == null)
                throw new ArgumentNullException("destinationDifference");
            if (!destinationDifference.Identifier.Equals(this.Identifier))
                throw new MergeException("Cannot compare differences that does not share the same identifier.");

            NodeDifference source = this;
            NodeDifference destination = destinationDifference as NodeDifference;
            if (destination == null)
                throw new MergeException(string.Format("Cannot compare a {0} to a {1}.", destinationDifference.GetType().Name, this.GetType().Name));

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
            return string.Format("{0} has been {1}.", this.Identifier, this.OperationOnParent.ToString().ToLower());
        }
    }
}
