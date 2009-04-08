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
    using Core.Merge;

    public partial class ValueConflictResolverForm : Form
    {
        public ValueConflictResolverForm(
                    ConflictContext context, 
                    string valueInSourceBranch, 
                    string valueInDestinationBranch)
        {
            InitializeComponent();
            FormPosition.LoadFromRegistry(this);

            m_result = null;

            m_conflictcontextcontrol.Data = context;
            m_textboxValueFromSourceBranch.Text = valueInSourceBranch;
            m_textboxValueFromDestinationBranch.Text = valueInDestinationBranch;
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

        protected override void OnClosing(CancelEventArgs e)
        {
            FormPosition.SaveInRegistry(this);
            base.OnClosing(e);
        }
    }
}
