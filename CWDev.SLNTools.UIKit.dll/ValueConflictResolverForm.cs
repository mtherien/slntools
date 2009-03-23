﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CWDev.VSSolutionTools.UIKit
{
    public partial class ValueConflictResolverForm : Form
    {
        public ValueConflictResolverForm(
                    string conflictDescription, 
                    string latestValueInSourceBranch, 
                    string latestValueInDestinationBranch)
        {
            InitializeComponent();

            m_result = null;

            m_richtextboxConflictDescription.Text = conflictDescription;
            m_textboxValueFromSourceBranch.Text = latestValueInSourceBranch;
            m_textboxValueFromDestinationBranch.Text = latestValueInDestinationBranch;
            m_buttonAccept.Enabled = false;
        }

        private string m_result;

        public string Result { get { return m_result; } }

        private void m_radio_CheckedChanged(object sender, EventArgs e)
        {
            m_buttonAccept.Enabled = true;           
        }

        private void m_textboxCustomValue_TextChanged(object sender, EventArgs e)
        {
            m_radioSelectCustomValue.Checked = true;
        }

        private void m_buttonAccept_Click(object sender, EventArgs e)
        {
            if (m_radioKeepSource.Checked)
            {
                m_result = m_textboxValueFromSourceBranch.Text;
            }
            else if (m_radioKeepDestination.Checked)
            {
                m_result = m_textboxValueFromDestinationBranch.Text;
            }
            else
            {
                m_result = m_textboxCustomValue.Text;
            }
        }
    }
}
