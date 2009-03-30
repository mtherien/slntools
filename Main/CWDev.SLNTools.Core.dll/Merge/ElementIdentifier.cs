namespace CWDev.SLNTools.Core.Merge
{
    public class ElementIdentifier
    {
        public ElementIdentifier(string name)
            : this(name, null)
        {
        }

        public ElementIdentifier(string name, string friendlyName)
        {
            m_name = name;
            m_friendlyName = (friendlyName == null) ? name : friendlyName;
        }

        private string m_name;
        private string m_friendlyName;

        public string Name { get { return m_name; } }
        public string FriendlyName { get { return m_friendlyName; } }

        public override bool Equals(object obj)
        {
            ElementIdentifier objAsElementIdentifier = obj as ElementIdentifier;
            if (objAsElementIdentifier == null)
                return false;

            return this.Name.Equals(objAsElementIdentifier.Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override string ToString()
        {
            return this.FriendlyName;
        }
    }
}
