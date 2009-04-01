using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace CWDev.SLNTools.UIKit
{
    using Core.Merge;

    public partial class ConflictContextControl : UserControl
    {
        public ConflictContextControl()
        {
            InitializeComponent();
        }

        public ConflictContext Data
        {
            set
            {
                m_treeview.Nodes.Clear();
                if (value != null)
                {
                    TreeNodeCollection current = m_treeview.Nodes;
                    bool first = true;
                    foreach (Conflict conflict in value.HierachyZoom)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            TreeNode newNode = current.Add(conflict.ToString());
                            current = newNode.Nodes;
                        }
                    }
                    m_treeview.ExpandAll();
                }
             }
        }
    }
}
