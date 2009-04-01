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

using System.Collections.Generic;
using System.Windows.Forms;

namespace CWDev.SLNTools.UIKit
{
    using Core.Merge;

    public partial class DifferencesControl : UserControl
    {
        public DifferencesControl()
        {
            InitializeComponent();
        }

        public IEnumerable<Difference> Data
        {
            set
            {
                m_treeview.Nodes.Clear();
                if (value != null)
                {
                    foreach (Difference difference in value)
                    {
                        TreeNode node = m_treeview.Nodes.Add(difference.ToString());
                        FillNode(node, difference);
                    }
                    m_treeview.Sort();
                }
             }
        }

        private void FillNode(TreeNode node, Difference difference)
        {
            node.Tag = difference;

            NodeDifference nodeDifference = difference as NodeDifference;
            if (nodeDifference != null)
            {
                foreach (Difference subdifference in nodeDifference.Subdifferences)
                {
                    TreeNode subnode = node.Nodes.Add(subdifference.ToString());
                    FillNode(subnode, subdifference);
                }
            }
        }
    }
}
