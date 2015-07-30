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
        private readonly OperationOnParent r_operationOnParent;
        private readonly DifferenceHashList r_acceptedSubdifferences;
        private readonly List<Conflict> r_subconflicts;

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

            r_operationOnParent = operationOnParent;
            r_acceptedSubdifferences = new DifferenceHashList(acceptedSubdifferences);
            r_subconflicts = new List<Conflict>(subconflicts);
        }

        public OperationOnParent OperationOnParent { get { return r_operationOnParent; } }
        public DifferenceHashList AcceptedSubdifferences { get { return r_acceptedSubdifferences; } }
        public ReadOnlyCollection<Conflict> Subconflicts { get { return r_subconflicts.AsReadOnly(); } }

        public override Difference Resolve(
                    ConflictContext context,
                    OperationTypeConflictResolver operationTypeConflictResolver,
                    ValueConflictResolver valueConflictResolver)
        {
            foreach (var subconflict in new List<Conflict>(r_subconflicts)) // Iterate on a copy of the list to be able to modify the original list in the loop
            {
                var resolvedDifference = subconflict.Resolve(context.CreateSubcontext(this), operationTypeConflictResolver, valueConflictResolver);
                if (resolvedDifference != null)
                {
                    r_subconflicts.Remove(subconflict);
                    r_acceptedSubdifferences.Add(resolvedDifference);
                }
            }

            if (r_subconflicts.Count == 0)
            {
                return new NodeDifference(
                            this.Identifier,
                            r_operationOnParent,
                            r_acceptedSubdifferences);
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
