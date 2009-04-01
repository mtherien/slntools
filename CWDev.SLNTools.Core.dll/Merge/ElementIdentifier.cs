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
