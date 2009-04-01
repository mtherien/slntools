using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CWDev.SLNTools.UIKit
{
    using Core.Merge;

    public partial class TypeDifferenceConflictResolverForm : Form
    {
        public TypeDifferenceConflictResolverForm(
                    ConflictContext context,
                    Difference differenceTypeInSourceBranch, 
                    Difference differenceTypeInDestinationBranch)
        {
            InitializeComponent();
            FormPosition.LoadFromRegistry(this);

            m_result = null;

            m_conflictcontextcontrol.Data = context;
            m_radioKeepSource.Tag = differenceTypeInSourceBranch;
            m_textboxChangeDescriptionFromSourceBranch.Text = differenceTypeInSourceBranch.ToString();
            m_radioKeepDestination.Tag = differenceTypeInDestinationBranch;
            m_textboxChangeDescriptionFromDestinationBranch.Text = differenceTypeInDestinationBranch.ToString();
            m_buttonAccept.Enabled = false;
        }

        private Difference m_result;

        public Difference Result { get { return m_result; } }

        private void m_radio_CheckedChanged(object sender, EventArgs e)
        {
            m_buttonAccept.Enabled = true;           
        }

        private void m_buttonAccept_Click(object sender, EventArgs e)
        {
            if (m_radioKeepSource.Checked)
            {
                m_result = (Difference)m_radioKeepSource.Tag;
            }
            else
            {
                m_result = (Difference)m_radioKeepDestination.Tag;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            FormPosition.SaveInRegistry(this);
            base.OnClosing(e);
        }
    }
}
