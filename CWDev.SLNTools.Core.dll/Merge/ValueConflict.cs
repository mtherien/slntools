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

namespace CWDev.SLNTools.Core.Merge
{
    public class ValueConflict : Conflict
    {
        public ValueConflict(
                    ElementIdentifier identifier,
                    OperationOnParent operationOnParent,
                    string commonAncestrorValue,
                    string newValueInSourceBranch,
                    string newValueInDestinationBranch)
            : base(identifier)
        {
            if (!Enum.IsDefined(operationOnParent.GetType(), operationOnParent))
                throw new ArgumentOutOfRangeException("operationOnParent", operationOnParent.ToString(), "Invalid value");

            m_operationOnParent = operationOnParent;
            m_oldValue = commonAncestrorValue;
            m_newValueInSourceBranch = newValueInSourceBranch;
            m_newValueInDestinationBranch = newValueInDestinationBranch;
        }

        private OperationOnParent m_operationOnParent;
        private string m_oldValue;
        private string m_newValueInSourceBranch;
        private string m_newValueInDestinationBranch;

        public OperationOnParent OperationOnParent { get { return m_operationOnParent; } }
        public string OldValue { get { return m_oldValue; } }
        public string NewValueInSourceBranch { get { return m_newValueInSourceBranch; } }
        public string NewValueInDestinationBranch { get { return m_newValueInDestinationBranch; } }

        public override Difference Resolve(
                    ConflictContext context,
                    OperationTypeConflictResolver operationTypeConflictResolver,
                    ValueConflictResolver valueConflictResolver)
        {
            string resolvedValue = valueConflictResolver(context.CreateSubcontext(this), m_newValueInSourceBranch, m_newValueInDestinationBranch);
            if (resolvedValue != null)
            {
                return new ValueDifference(this.Identifier, this.OperationOnParent, m_oldValue, resolvedValue);
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return string.Format("Both branches {0} {1} but with different values: Source = \"{2}\" and Destination = \"{3}\".",
                        this.OperationOnParent.ToString().ToLower(),
                        this.Identifier,
                        this.NewValueInSourceBranch,
                        this.NewValueInDestinationBranch);
        }
    }
}
