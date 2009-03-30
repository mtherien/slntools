using System.Collections.ObjectModel;

namespace CWDev.SLNTools.Core.Merge
{
    public class DifferenceHashList : KeyedCollection<ElementIdentifier, Difference>
    {
        public DifferenceHashList()
        {

        }

        protected override ElementIdentifier GetKeyForItem(Difference item)
        {
            return item.Identifier;
        }
    }
}
