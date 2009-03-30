using System.Collections.Generic;

namespace CWDev.SLNTools.Core
{
    using Merge;

    public class PropertyLineList
        : List<PropertyLine>
    {
        public PropertyLineList()
        {
        }

        public PropertyLineList(IEnumerable<PropertyLine> original)
            : base(original)
        {
        }

        public void ModifyLine(string name, string value)
        {
            for (int i = 0; i < this.Count; i++)
            {
                PropertyLine line = this[i];
                if (line.Name == name)
                {
                    if (value == null)
                    {
                        Remove(line);
                    }
                    else
                    {
                        this[i] = new PropertyLine(name, value);
                    }
                    return;
                }
            }

            Add(new PropertyLine(name, value));
        }
    }
}
