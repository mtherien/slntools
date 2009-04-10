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
                throw new ArgumentOutOfRangeException("operationOnParent", operationOnParent, "Invalid value");
            if (acceptedSubdifferences == null)
                throw new ArgumentNullException("acceptedSubdifferences");
            if (subconflicts == null)
                throw new ArgumentNullException("subconflicts");

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
                    ConflictContext context,
                    TypeDifferenceConflictResolver typeDifferenceConflictResolver,
                    ValueConflictResolver valueConflictResolver)
        {
            foreach (Conflict subconflict in new List<Conflict>(m_subconflicts)) // Iterate on a copy of the list to be able to modify the original list in the loop
            {
                Difference resolvedDifference = subconflict.Resolve(context.CreateSubcontext(this), typeDifferenceConflictResolver, valueConflictResolver);
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
            return string.Format("{0} has been {1} in both branches.", this.Identifier, this.OperationOnParent.ToString().ToLower());
        }
    }
}
