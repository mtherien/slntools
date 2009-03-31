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
        public UpdateOriginalSolutionForm(IEnumerable<Difference> differences, string originalSolutionFullPath)
        {
            InitializeComponent();
            FormPosition.LoadFromRegistry(this);

            m_differencesInFilteredSolution.Data = differences;
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