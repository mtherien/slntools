using System.Collections.Generic;

namespace CWDev.SLNTools.Core.Merge
{
    public class TypeDifferenceConflict : Conflict
    {
        public TypeDifferenceConflict(
                    Difference differenceInSourceBranch,
                    Difference differenceInDestinationBranch)
            : base(differenceInSourceBranch.Identifier)
        {
            m_differenceInSourceBranch = differenceInSourceBranch;
            m_differenceInDestinationBranch = differenceInDestinationBranch;
        }

        private Difference m_differenceInSourceBranch;
        private Difference m_differenceInDestinationBranch;

        public Difference DifferenceInSourceBranch { get { return m_differenceInSourceBranch; } }
        public Difference DifferenceInDestinationBranch { get { return m_differenceInDestinationBranch; } }

        public override Difference Resolve(
                    ConflictContext context,
                    TypeDifferenceConflictResolver typeDifferenceConflictResolver,
                    ValueConflictResolver valueConflictResolver)
        {
            return typeDifferenceConflictResolver(context.CreateSubcontext(this), m_differenceInSourceBranch, m_differenceInDestinationBranch);
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
