using System.Collections.Generic;
using System.Windows.Forms;

namespace CWDev.SLNTools.UIKit
{
    using Core.Merge;

    public partial class ConflictsControl : UserControl
    {
        public ConflictsControl()
        {
            InitializeComponent();
        }

        public IEnumerable<Conflict> Data
        {
            set
            {
                m_treeview.Nodes.Clear();
                if (value != null)
                {
                    foreach (Conflict conflict in value)
                    {
                        TreeNode node = m_treeview.Nodes.Add(conflict.ToString());
                        FillConflictNode(node, conflict);
                    }
                    m_treeview.Sort();
                }
             }
        }

        private void FillConflictNode(TreeNode node, Conflict conflict)
        {
            node.Tag = conflict;

            NodeConflict nodeConflict = conflict as NodeConflict;
            if (nodeConflict != null)
            {
                if (nodeConflict.AcceptedSubdifferences.Count > 0)
                {
                    TreeNode acceptedNode = node.Nodes.Add("Accepted Differences");
                    foreach (Difference subdifference in nodeConflict.AcceptedSubdifferences)
                    {
                        TreeNode subnode = acceptedNode.Nodes.Add(subdifference.ToString());
                        FillDifferenceNode(subnode, subdifference);
                    }
                }

                TreeNode conflictNode = node.Nodes.Add("Conflicts");
                foreach (Conflict subconflict in nodeConflict.Subconflicts)
                {
                    TreeNode subnode = conflictNode.Nodes.Add(subconflict.ToString());
                    FillConflictNode(subnode, subconflict);
                }
            }
        }

        private void FillDifferenceNode(TreeNode node, Difference difference)
        {
            node.Tag = difference;

            NodeDifference nodeDifference = difference as NodeDifference;
            if (nodeDifference != null)
            {
                foreach (Difference subdifference in nodeDifference.Subdifferences)
                {
                    TreeNode subnode = node.Nodes.Add(subdifference.ToString());
                    FillDifferenceNode(subnode, subdifference);
                }
            }
        }
    }
}
