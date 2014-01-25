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
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CWDev.SLNTools.UIKit
{
    using Core;
    using Core.Filter;

    public partial class CreateFilterForm : Form
    {
        public CreateFilterForm(string filename)
        {
            InitializeComponent();
            Init(filename);
            FormPosition.LoadFromRegistry(this);
        }

        private FilterFile _filterFile;
        private readonly List<TreeNode> _allNodes = new List<TreeNode>();
        private int _nbProjectsSelected = 0;

        private void ClearAllFields()
        {
            this._filterFile = null;
            this._allNodes.Clear();
            m_textboxSourceSolution.Text = "";
            m_treeview.Nodes.Clear();
            m_labelErrorMessage.Text = "";
        }

        private void Init(string filename)
        {
            try
            {
                ClearAllFields();
                if (filename != null)
                {
                    switch (Path.GetExtension(filename).ToLower())
                    {
                        case ".slnfilter":
                            this._filterFile = FilterFile.FromFile(filename);
                            break;

                        default:
                        case ".sln":
                            this._filterFile = new FilterFile();
                            this._filterFile.SourceSolutionFullPath = filename;
                            break;
                    }

                    m_textboxSourceSolution.Text = this._filterFile.SourceSolutionFullPath;
                    m_treeview.BeginUpdate();
                    AddProjectToNode(m_treeview.Nodes, this._filterFile.ProjectsToKeep, this._filterFile.SourceSolution.Childs);
                    m_treeview.ExpandAll();
                    m_treeview.Sort();
                    m_treeview.SelectedNode = m_treeview.Nodes[0];
                    m_treeview.EndUpdate();
                    m_checkboxWatchForChangesOnFilteredSolution.Checked = this._filterFile.WatchForChangesOnFilteredSolution;
                }
            }
            catch (Exception ex)
            {
                ClearAllFields();
                MessageBox.Show(string.Format("Error while loading the solution file.\nException: {0}", ex));
            }
            UpdateTreeView();
            UpdateControls();
        }

        private void AddProjectToNode(TreeNodeCollection parentNodes, List<String> projectsToCheck, IEnumerable<Project> childs)
        {
            foreach (var child in childs)
            {
                var node = new TreeNode(child.ProjectName)
                            {
                                Tag = child,
                                Checked = (projectsToCheck.Contains(child.ProjectFullName))
                            };
                this._allNodes.Add(node);
                parentNodes.Add(node);

                AddProjectToNode(node.Nodes, projectsToCheck, child.Childs);
            }
        }

        private void UpdateTreeView()
        {
            this._nbProjectsSelected = 0;
            var nbProjectsIncludedInFilteredSolution = 0;
            if (this._filterFile != null)
            {
                var filteredSolution = this._filterFile.Apply();

                m_treeview.BeginUpdate();
                foreach (var treeNode in this._allNodes)
                {
                    var project = treeNode.Tag as Project;
                    if (filteredSolution.Projects.FindByGuid(project.ProjectGuid) != null)
                    {
                        nbProjectsIncludedInFilteredSolution++;
                        treeNode.ForeColor = Color.Green;
                    }
                    else
                    {
                        treeNode.ForeColor = Color.Red;
                    }

                    if (treeNode.Checked)
                        this._nbProjectsSelected++;
                }

                m_treeview.EndUpdate();
            }
            m_labelSelected.Text = string.Format(
                        "Checked = {0}, Included in filter = {1}/{2}",
                        this._nbProjectsSelected,
                        nbProjectsIncludedInFilteredSolution,
                        this._allNodes.Count);
        }

        private void UpdateControls()
        {
            m_labelErrorMessage.Text = "";

            if (this._filterFile != null)
            {
                m_treeview.Enabled = true;
                m_groupboxOptions.Enabled = true;
                m_buttonSaveAndQuit.Enabled = (this._nbProjectsSelected > 0);
                m_menuitemSave.Enabled = (this._filterFile.FilterFullPath != null) && (this._nbProjectsSelected > 0);
                m_menuitemSaveAs.Enabled = (this._nbProjectsSelected > 0);

                if (this._nbProjectsSelected == 0)
                {
                    m_labelErrorMessage.Text = "At least one project or solution folder need to be checked";
                }
            }
            else
            {
                m_treeview.Enabled = false;
                m_groupboxOptions.Enabled = false;
                m_buttonSaveAndQuit.Enabled = false;
                m_menuitemSave.Enabled = false;
                m_menuitemSaveAs.Enabled = false;
            }
        }

        private void m_treeview_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var project = e.Node.Tag as Project;
            if (e.Node.Checked)
            {
                this._filterFile.ProjectsToKeep.Add(project.ProjectFullName);
            }
            else
            {
                this._filterFile.ProjectsToKeep.Remove(project.ProjectFullName);
            }
            UpdateTreeView();
            UpdateControls();
        }

        private void m_menuitemOpen_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.CheckFileExists = true;
                fileDialog.Filter = "Solution File or Solution Filter File(*.sln;*.slnfilter)|*.sln;*.slnfilter|Solution File (*.sln)|*.sln|Solution Filter File (*.slnfilter)|*.slnfilter|All files (*.*)|*.*";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Init(fileDialog.FileName);
                }
            }
            UpdateControls();
        }

        private void m_menuitemSave_Click(object sender, EventArgs e)
        {
            Save();
            UpdateControls();
        }

        private void m_menuitemSaveAs_Click(object sender, EventArgs e)
        {
            SaveAs();
            UpdateControls();
        }

        private void m_menuitemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void m_buttonSaveAndQuit_Click(object sender, EventArgs e)
        {
            if (this._filterFile.FilterFullPath != null)
            {
                if (Save())
                    this.Close();
            }
            else
            {
                if (SaveAs())
                    this.Close();
            }
        }

        private bool Save()
        {
            try
            {
                this._filterFile.WatchForChangesOnFilteredSolution = m_checkboxWatchForChangesOnFilteredSolution.Checked;
                this._filterFile.CopyReSharperFiles = m_checkboxCopyReSharperFiles.Checked;
                this._filterFile.Save();

                SolutionFile filteredSolution = this._filterFile.Apply();
                filteredSolution.Save();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error while creating the filter file.\nException: {0}", ex));
            }
            return false;
        }

        private bool SaveAs()
        {
            try
            {
                using (var fileDialog = new SaveFileDialog())
                {
                    fileDialog.InitialDirectory = Path.GetDirectoryName(this._filterFile.SourceSolutionFullPath);
                    fileDialog.Filter = "Solution Filter File (*.slnfilter)|*.slnfilter|All files (*.*)|*.*";
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (string.Equals(
                                    Path.GetDirectoryName(Path.GetFullPath(fileDialog.FileName)), 
                                    Path.GetDirectoryName(Path.GetFullPath(this._filterFile.SourceSolutionFullPath)), 
                                    StringComparison.InvariantCultureIgnoreCase))                                    
                        {
                            this._filterFile.FilterFullPath = fileDialog.FileName;
                            this._filterFile.WatchForChangesOnFilteredSolution = m_checkboxWatchForChangesOnFilteredSolution.Checked;
                            this._filterFile.CopyReSharperFiles = m_checkboxCopyReSharperFiles.Checked;
                            this._filterFile.Save();

                            var filteredSolution = this._filterFile.Apply();
                            filteredSolution.Save();
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Filter file need to be in the same folder as the source solution.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error while creating the filter file.\nException: {0}", ex));
            }
            return false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            FormPosition.SaveInRegistry(this);
            base.OnClosing(e);
        }
    }
}