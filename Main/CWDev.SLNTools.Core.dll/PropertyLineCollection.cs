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

    public class PropertyLineCollection 
        : KeyedCollection<string, PropertyLine>        
    {
        public PropertyLineCollection()
        {
        }

        public PropertyLineCollection(IEnumerable<PropertyLine> original)
        {
            AddRange(original);
        }

        public ReadOnlyCollection<PropertyLine> AsReadOnly()
        {
            return new List<PropertyLine>(this).AsReadOnly();
        }

        public void AddRange(IEnumerable<PropertyLine> original)
        {
            foreach (PropertyLine propertyLine in original)
            {
                Add(propertyLine);
            }
        }

        protected override string GetKeyForItem(PropertyLine item)
        {
            return item.Name;
        }

        protected override void InsertItem(int index, PropertyLine item)
        {
            PropertyLine existingItem = (Contains(item.Name)) ? this[item.Name] : null;

            if (existingItem == null)                
            {
                base.InsertItem(index, item);
            }
            else if (item.Value != existingItem.Value)
            {
                throw new SolutionFileException(
                            string.Format("Trying to add a new property line '{0}={1}' when there is already a line with the same key and the value '{2}' in the collection.",
                                item.Name,
                                item.Value,
                                existingItem.Value));
            }
            else
            {
                // Nothing to do, the item provided is a duplicate of a line already present in the collection.
            }
        }
    }
}
