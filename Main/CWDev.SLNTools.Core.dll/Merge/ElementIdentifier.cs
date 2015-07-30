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

namespace CWDev.SLNTools.Core.Merge
{
    public class ElementIdentifier
    {
        private readonly string r_name;
        private readonly string r_friendlyName;

        public ElementIdentifier(string name)
            : this(name, null)
        {
        }

        public ElementIdentifier(string name, string friendlyName)
        {
            r_name = name;
            r_friendlyName = friendlyName ?? name;
        }

        public string Name { get { return r_name; } }
        public string FriendlyName { get { return r_friendlyName; } }

        public EmptyElement CreateEmptyElement()
        {
            return new EmptyElement(this);
        }

        public override bool Equals(object obj)
        {
            var objAsElementIdentifier = obj as ElementIdentifier;
            if (objAsElementIdentifier == null)
                return false;

            return (string.Compare(this.Name, objAsElementIdentifier.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public override int GetHashCode()
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(this.Name);
        }

        public override string ToString()
        {
            return this.FriendlyName;
        }
    }
}
