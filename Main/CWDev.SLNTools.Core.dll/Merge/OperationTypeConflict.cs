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
    public class OperationTypeConflict : Conflict
    {
        private readonly Difference r_differenceInSourceBranch;
        private readonly Difference r_differenceInDestinationBranch;

        public OperationTypeConflict(
                    Difference differenceInSourceBranch,
                    Difference differenceInDestinationBranch)
            : base(differenceInSourceBranch.Identifier)
        {
            r_differenceInSourceBranch = differenceInSourceBranch;
            r_differenceInDestinationBranch = differenceInDestinationBranch;
        }

        public Difference DifferenceInSourceBranch { get { return r_differenceInSourceBranch; } }
        public Difference DifferenceInDestinationBranch { get { return r_differenceInDestinationBranch; } }

        public override Difference Resolve(
                    ConflictContext context,
                    OperationTypeConflictResolver operationTypeConflictResolver,
                    ValueConflictResolver valueConflictResolver)
        {
            return operationTypeConflictResolver(context.CreateSubcontext(this), r_differenceInSourceBranch, r_differenceInDestinationBranch);
        }

        public override string ToString()
        {
            return string.Format("{0} was {1} in the source branch but it was {2} in the destination branch.",
                        this.Identifier,
                        this.DifferenceInSourceBranch.OperationOnParent.ToString().ToLower(),
                        this.DifferenceInDestinationBranch.OperationOnParent.ToString().ToLower());
        }
    }
}
