using System.Collections.Generic;

namespace CWDev.VSSolutionTools.Core
{
    using Merge;

    public class GlobalSection : Section
    {
        public GlobalSection(GlobalSection original)
                    : base(original)
        {
        }

        public GlobalSection(string name, string sectionType, string step, IEnumerable<PropertyLine> propertyLines)
                    : base(name, sectionType, step, propertyLines)
        {
        }

        public GlobalSection(GlobalSection original, IEnumerable<Difference> differences)
            : base(original, differences)
        {
        }

        public GlobalSection(string name, IEnumerable<Difference> differences)
            : base(name, differences)
        {
        }
    }
}
