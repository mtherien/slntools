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

    public class SectionHashList<T> 
        : KeyedCollection<string, T>
        where T : Section
    {
        public SectionHashList()
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }

        public SectionHashList(IEnumerable<T> original)
            : this()
        {
            AddRange(original);
        }

        protected override string GetKeyForItem(T item)
        {
            return item.Name;
        }

        public ReadOnlyCollection<T> AsReadOnly()
        {
            return new List<T>(this).AsReadOnly();
        }

        public void AddRange(IEnumerable<T> sections)
        {
            if (sections != null)
            {
                foreach (T section in sections)
                {
                    Add(section);
                }
            }
        }

        public void AddOrUpdate(T item)
        {
            T existingItem = (Contains(GetKeyForItem(item))) ? this[GetKeyForItem(item)] : null;
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
    }
}
