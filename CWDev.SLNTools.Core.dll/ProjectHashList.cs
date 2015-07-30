#region License

// SLNTools
// Copyright (c) 2009
// by Christian Warren
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CWDev.SLNTools.Core
{
    public class ProjectHashList
        : KeyedCollection<string, Project>
    {
        private readonly SolutionFile r_container;

        public ProjectHashList(SolutionFile container)
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            r_container = container;
        }

        public ProjectHashList(SolutionFile container, IEnumerable<Project> items)
            : this(container)
        {
            AddRange(items);
        }

        protected override string GetKeyForItem(Project item)
        {
            return item.ProjectGuid;
        }

        protected override void InsertItem(int index, Project item)
        {
            // Add a clone of the item instead of the item itself
            base.InsertItem(index, new Project(r_container, item));
        }

        protected override void SetItem(int index, Project item)
        {
            // Add a clone of the item instead of the item itself
            base.SetItem(index, new Project(r_container, item));
        }

        public SolutionFile Container
        {
            get { return r_container; }
        }

        public void AddRange(IEnumerable<Project> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    Add(item);
                }
            }
        }

        public Project FindByGuid(string guid)
        {
            return (Contains(guid)) ? this[guid] : null;
        }

        public Project FindByFullName(string projectFullName)
        {
            foreach (var item in this)
            {
                if (string.Compare(item.ProjectFullName, projectFullName, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return item;
                }
            }
            return null;
        }

        public void Sort()
        {
            Sort((p1, p2) => StringComparer.InvariantCultureIgnoreCase.Compare(p1.ProjectFullName, p2.ProjectFullName));
        }

        public void Sort(Comparison<Project> comparer)
        {
            var tempList = new List<Project>(this);
            tempList.Sort(comparer);

            Clear();
            AddRange(tempList);
        }
    }
}
