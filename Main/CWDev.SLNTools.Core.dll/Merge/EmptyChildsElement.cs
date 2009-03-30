namespace CWDev.SLNTools.Core.Merge
{
    public class EmptyNodeElement : NodeElement 
    {
        public EmptyNodeElement(ElementIdentifier identifier)
            : base(identifier, new ElementHashList())
        { }
    }
}
