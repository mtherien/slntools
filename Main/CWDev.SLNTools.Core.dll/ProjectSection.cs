using System.Collections.Generic;

namespace CWDev.VSSolutionTools.Core
{
    using Merge;

    public class ProjectSection : Section
    {
        public ProjectSection(ProjectSection original)
                    : base(original)
        {
        }

        public ProjectSection(string name, string sectionType, string step, IEnumerable<PropertyLine> propertyLines)
                    : base(name, sectionType, step, propertyLines)
        {
        }

        public ProjectSection(ProjectSection original, IEnumerable<Difference> differences)
            : base(original, differences)
        {
        }

        public ProjectSection(string name, IEnumerable<Difference> differences)
            : base(name, differences)
        {
        }
    }
}
