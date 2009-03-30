namespace CWDev.SLNTools.Core
{
    public class PropertyLine
    {
        public PropertyLine(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public readonly string Name;
        public readonly string Value;

        public override string ToString()
        {
            return string.Format("{0} = {1}", this.Name, this.Value);
        }
    }
}
