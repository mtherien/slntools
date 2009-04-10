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
    using Merge;

    public class ProjectHashList 
        : KeyedCollection<string, Project>        
    {
        public ProjectHashList()
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }

        public ProjectHashList(IEnumerable<Project> original)
            : this()
        {
            AddRange(original);
        }

        protected override string GetKeyForItem(Project item)
        {
            return item.ProjectGuid;
        }

        public ReadOnlyCollection<Project> AsReadOnly()
        {
            return new List<Project>(this).AsReadOnly();
        }

        public void AddRange(IEnumerable<Project> original)
        {
            foreach (Project project in original)
            {
                Add(project);
            }
        }

        public void AddOrUpdate(Project item)
        {
            Project existingItem = (Contains(item.ProjectGuid)) ? this[item.ProjectGuid] : null;
            if (existingItem == null)
            {
                Add(item);
            }
            else
            {
                // If the item already exist in the list, we put the new version in the same spot.
                SetItem(IndexOf(existingItem), item);
            }
        }

        public void Sort(Comparison<Project> comparer)
        {
            List<Project> tempList = new List<Project>(this);
            tempList.Sort(comparer);
            
            Clear();
            AddRange(tempList);
        }
    }
}
