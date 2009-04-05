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
                differenceInSourceBranch = new NodeDifference(new ElementIdentifier("Solution File"), OperationOnParent.Modified, new List<Difference>());
            }

            differenceInDestinationBranch = (NodeDifference) latestElementInDestinationBranch.CompareTo(rootElement);
            if (differenceInDestinationBranch == null)
            {
                differenceInDestinationBranch = new NodeDifference(new ElementIdentifier("Solution File"), OperationOnParent.Modified, new List<Difference>());
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
