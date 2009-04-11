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
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CWDev.SLNTools.UIKit
{
    using Core.Merge;

    public partial class UpdateOriginalSolutionForm : Form
    {
        public UpdateOriginalSolutionForm(NodeDifference difference, string originalSolutionFullPath)
        {
            InitializeComponent();
            FormPosition.LoadFromRegistry(this);

            m_differencesInFilteredSolution.Data = difference;
            m_originalSolutionFullPath = originalSolutionFullPath;
            UpdateState();

            m_watcher = new FileSystemWatcher();
            m_watcher.NotifyFilter = NotifyFilters.Attributes;
            m_watcher.Path = Path.GetDirectoryName(originalSolutionFullPath);
            m_watcher.Filter = Path.GetFileName(originalSolutionFullPath);
            m_watcher.Changed += delegate(object source, FileSystemEventArgs e)
                        {
                            UpdateState();
                        };
            m_watcher.EnableRaisingEvents = true;
        }

        private string m_originalSolutionFullPath;
        private FileSystemWatcher m_watcher;

        private void UpdateState()
        {
            MethodInvoker invoker = new MethodInvoker(delegate()
                        {
                            if ((File.GetAttributes(m_originalSolutionFullPath) & FileAttributes.ReadOnly) != 0)
                            {
                                m_labelState.ForeColor = Color.Red;
                                m_labelState.Text = "ReadOnly";
                                m_labelStateDescription.Visible = true;
                                m_buttonYes.Enabled = false;
                            }
                            else
                            {
                                m_labelState.ForeColor = Color.Black;
                                m_labelState.Text = "Writable";
                                m_labelStateDescription.Visible = false;
                                m_buttonYes.Enabled = true;
                            }
                        });
            if (m_labelState.InvokeRequired)
            {
                this.Invoke(invoker);
            }
            else
            {
                invoker.Invoke();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            FormPosition.SaveInRegistry(this);
            base.OnClosing(e);
        }
    }
}