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
                OperationTypeConflictResolver operationTypeConflictResolver,
                ValueConflictResolver valueConflictResolver)
        {
            InitializeComponent();
            FormPosition.LoadFromRegistry(this);

            m_differencesInSourceBranchControl.Data = differenceInSourceBranch;
            m_differencesInDestinationBranchControl.Data = differenceInDestinationBranch;
            m_conflict = conflict as NodeConflict;
            m_operationTypeConflictResolver = operationTypeConflictResolver;
            m_valueConflictResolver = valueConflictResolver;
            if (m_conflict.Subconflicts.Count == 0)
            {
                m_result = (NodeDifference)m_conflict.Resolve(m_operationTypeConflictResolver, m_valueConflictResolver);
            }
            else
            {
                m_result = null;
            }

            UpdateUI();
        }

        private NodeConflict m_conflict;
        private OperationTypeConflictResolver m_operationTypeConflictResolver;
        private ValueConflictResolver m_valueConflictResolver;
        private NodeDifference m_result;

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

        public NodeDifference Result
        {
            get
            {
                return m_result;
            }
        }

        private void UpdateUI()
        {
            m_conflictsControl.Data = m_conflict;
            m_buttonResolveAll.Enabled = (m_conflict.Subconflicts.Count != 0);
            m_acceptedDifferencesControl.Data = new NodeDifference(new ElementIdentifier("SolutionFile"), OperationOnParent.Modified, m_conflict.AcceptedSubdifferences);
            m_buttonSave.Enabled = (m_conflict.Subconflicts.Count == 0);
        }

        private void m_buttonResolveAll_Click(object sender, EventArgs e)
        {
            m_result = (NodeDifference)m_conflict.Resolve(m_operationTypeConflictResolver, m_valueConflictResolver);
            UpdateUI();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            FormPosition.SaveInRegistry(this);
            base.OnClosing(e);
        }
    }
}