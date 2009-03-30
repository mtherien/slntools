using System.Collections.ObjectModel;

namespace CWDev.SLNTools.Core.Merge
{
    public class ElementHashList : KeyedCollection<ElementIdentifier, Element>
    {
        public ElementHashList()
        {

        }

        protected override ElementIdentifier GetKeyForItem(Element item)
        {
            return item.Identifier;
        }
    }
}
