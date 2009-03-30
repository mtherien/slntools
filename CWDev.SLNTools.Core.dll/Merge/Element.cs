using System;

namespace CWDev.SLNTools.Core.Merge
{
    public abstract class Element
    {
        protected Element(ElementIdentifier identifier)
        {
            if (identifier == null)
                throw new Exception("TODO");

            m_identifier = identifier;
        }

        private ElementIdentifier m_identifier;

        public ElementIdentifier Identifier { get { return m_identifier; } }

        public abstract Element CreateEmptyClone();
        public abstract Difference CompareTo(Element oldElement);
    }
}
