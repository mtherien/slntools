using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CWDev.SLNTools.UIKit
{
    using Core;
    using Core.Merge;

    public partial class MergeSolutionsForm : Form
    {
        public MergeSolutionsForm(
                NodeDifference differenceInSourceBranch,
                NodeDifference differenceInDestinationBranch,
                Conflict conflict,
                TypeDifferenceConflictResolver typeDifferenceConflictResolver,
                ValueConflictResolver valueConflictResolver)
        {
            InitializeComponent();
            FormPosition.LoadFromRegistry(this);

            m_differencesInSourceBranchControl.Data = differenceInSourceBranch.Subdifferences;
            m_differencesInDestinationBranchControl.Data = differenceInDestinationBranch.Subdifferences;
            m_conflict = conflict as NodeConflict;
            m_typeDifferenceConflictResolver = typeDifferenceConflictResolver;
            m_valueConflictResolver = valueConflictResolver;

            UpdateUI();
        }

        private NodeConflict m_conflict;
        private TypeDifferenceConflictResolver m_typeDifferenceConflictResolver;
        private ValueConflictResolver m_valueConflictResolver;

        private void MergeSolutionsForm_Load(object sender, EventArgs e)
        {
            // Add the PanelXMinSize here because there is a bug in the designer that set the 'PanelMinSize' before the control size. 
            m_splitContainerDifferencesFoundAndWorkArea.Panel1MinSize = 100;
            m_splitContainerDifferencesFoundAndWorkArea.Panel2MinSize = 250;
            m_splitContainerConflictAndResult.Panel1MinSize = 100;
            m_splitContainerConflictAndResult.Panel2MinSize = 100;
            m_splitContainerDifferencesFound.Panel1MinSize = 250;
            m_splitContainerDifferencesFound.Panel2MinSize = 250;
        }

        public IEnumerable<Difference> Result
        {
            get
            {
                if (m_conflict.Subconflicts.Count == 0)
                {
                    return m_conflict.AcceptedSubdifferences;
                }
                else
                {
                    return null;
                }
            }
        }

        private void UpdateUI()
        {
            m_conflictsControl.Data = m_conflict.Subconflicts;
            m_acceptedDifferencesControl.Data = m_conflict.AcceptedSubdifferences;
            m_buttonSave.Enabled = (m_conflict.Subconflicts.Count == 0);
        }

        private void m_buttonResolveAll_Click(object sender, EventArgs e)
        {
            m_conflict.Resolve(m_typeDifferenceConflictResolver, m_valueConflictResolver);
            UpdateUI();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            FormPosition.SaveInRegistry(this);
            base.OnClosing(e);
        }
    }
}