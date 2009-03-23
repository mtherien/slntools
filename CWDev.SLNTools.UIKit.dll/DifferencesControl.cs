using System.Collections.Generic;
using System.Windows.Forms;

namespace CWDev.VSSolutionTools.UIKit
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
