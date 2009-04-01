using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CWDev.SLNTools.Core.Merge
{
    public class ConflictContext
    {
        public ConflictContext()
        {
            m_conflicts = new List<Conflict>();
        }
        private ConflictContext(ConflictContext context, Conflict subconflict)
        {
            m_conflicts = new List<Conflict>(context.m_conflicts);
            m_conflicts.Add(subconflict);
        }

        private List<Conflict> m_conflicts;

        public ReadOnlyCollection<Conflict> HierachyZoom
        {
            get { return m_conflicts.AsReadOnly(); }
        }

        public ConflictContext CreateSubcontext(Conflict subconflict)
        {
            return new ConflictContext(this, subconflict);
        }
    }
}
