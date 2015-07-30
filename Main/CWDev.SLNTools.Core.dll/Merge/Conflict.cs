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

namespace CWDev.SLNTools.Core.Merge
{
    public delegate string ValueConflictResolver(
                ConflictContext context,
                string valueInSourceBranch,
                string valueInDestinationBranch);
    public delegate Difference OperationTypeConflictResolver(
                ConflictContext context,
                Difference differenceInSourceBranch,
                Difference differenceInDestinationBranch);

    public abstract class Conflict
    {
        public static NodeConflict Merge(
                    NodeElement commonAncestrorElement,
                    NodeElement elementInSourceBranch,
                    NodeElement elementInDestinationBranch)
        {
            NodeDifference ignoreA, ignoreB;
            return Merge(commonAncestrorElement, elementInSourceBranch, elementInDestinationBranch, out ignoreA, out ignoreB);
        }

        public static NodeConflict Merge(
                    NodeElement commonAncestrorElement,
                    NodeElement elementInSourceBranch,
                    NodeElement elementInDestinationBranch,
                    out NodeDifference differenceInSourceBranch,
                    out NodeDifference differenceInDestinationBranch)
        {
            differenceInSourceBranch = (NodeDifference) elementInSourceBranch.CompareTo(commonAncestrorElement)
                            ?? new NodeDifference(new ElementIdentifier("SolutionFile"), OperationOnParent.Modified, null);

            differenceInDestinationBranch = (NodeDifference) elementInDestinationBranch.CompareTo(commonAncestrorElement)
                            ?? new NodeDifference(new ElementIdentifier("SolutionFile"), OperationOnParent.Modified, null);

            return (NodeConflict)differenceInSourceBranch.CompareTo(differenceInDestinationBranch);
        }


        private readonly ElementIdentifier r_identifier;

        protected Conflict(ElementIdentifier identifier)
        {
            r_identifier = identifier;
        }

        public ElementIdentifier Identifier { get { return r_identifier; } }

        public Difference Resolve(
                    OperationTypeConflictResolver operationTypeConflictResolver,
                    ValueConflictResolver valueConflictResolver)
        {
            return Resolve(new ConflictContext(), operationTypeConflictResolver, valueConflictResolver);
        }

        public abstract Difference Resolve(
                        ConflictContext context,
                        OperationTypeConflictResolver operationTypeConflictResolver,
                        ValueConflictResolver valueConflictResolver);
    }
}
