using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CWDev.VSSolutionTools.UIKit
{
    using Core;
    using Core.Filter;

    public partial class CreateFilterForm : Form
    {
        public CreateFilterForm(string filename)
        {
            InitializeComponent();
            Init(filename);
        }

        private FilterFile m_filterFile;
        private readonly List<TreeNode> m_allNodes = new List<TreeNode>();
        private int m_nbProjectsSelected = 0;

        private void ClearAllFields()
        {
            m_filterFile = null;
            m_allNodes.Clear();
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
                            m_filterFile = FilterFile.FromFile(filename);
                            break;

                        default:
                        case ".sln":
                            m_filterFile = new FilterFile();
                            m_filterFile.SourceSolutionFullPath = filename;
                            break;
                    }

                    m_textboxSourceSolution.Text = m_filterFile.SourceSolutionFullPath;
                    m_treeview.BeginUpdate();
                    AddProjectToNode(m_treeview.Nodes, m_filterFile.ProjectsToKeep, m_filterFile.SourceSolution.Childs);
                    m_treeview.ExpandAll();
                    m_treeview.Sort();
                    m_treeview.SelectedNode = m_treeview.Nodes[0];
                    m_treeview.EndUpdate();
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
            foreach (Project child in childs)
            {
                TreeNode node = new TreeNode(child.ProjectName);
                node.Tag = child;
                node.Checked = (projectsToCheck.Contains(child.ProjectFullName));
                m_allNodes.Add(node);
                parentNodes.Add(node);

                AddProjectToNode(node.Nodes, projectsToCheck, child.Childs);
            }
        }

        private void UpdateTreeView()
        {
            m_nbProjectsSelected = 0;
            int nbProjectsIncludedInFilteredSolution = 0;
            SolutionFile filteredSolution = m_filterFile.Apply();

            m_treeview.BeginUpdate();
            foreach (TreeNode treeNode in m_allNodes)
            {
                Project project = treeNode.Tag as Project;
                if (filteredSolution.FindProjectByGuid(project.ProjectGuid) != null)
                {
                    nbProjectsIncludedInFilteredSolution++;
                    treeNode.ForeColor = Color.Green;
                }
                else
                {
                    treeNode.ForeColor = Color.Red;
                }

                if (treeNode.Checked)
                    m_nbProjectsSelected++;
            }

            m_treeview.EndUpdate();

            m_labelSelected.Text = string.Format(
                        "Checked = {0}, Included in filter = {1}/{2}",
                        m_nbProjectsSelected,
                        nbProjectsIncludedInFilteredSolution,
                        m_allNodes.Count);
        }

        private void UpdateControls()
        {
            m_labelErrorMessage.Text = "";
            m_menuitemSave.Enabled = false;
            m_menuitemSaveAs.Enabled = false;

            if (m_filterFile != null)
            {
                m_buttonSaveAndQuit.Enabled = (m_nbProjectsSelected > 0);
                m_menuitemSave.Enabled = (m_filterFile.FilterFullPath != null) && (m_nbProjectsSelected > 0);
                m_menuitemSaveAs.Enabled = (m_nbProjectsSelected > 0);

                if (m_nbProjectsSelected == 0)
                {
                    m_labelErrorMessage.Text = "At least one project or solution folder need to be checked";
                }
            }
        }

        private void m_treeview_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Project project = e.Node.Tag as Project;
            if (e.Node.Checked)
            {
                m_filterFile.ProjectsToKeep.Add(project.ProjectFullName);
            }
            else
            {
                m_filterFile.ProjectsToKeep.Remove(project.ProjectFullName);
            }
            UpdateTreeView();
            UpdateControls();
        }

        private void m_menuitemOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
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
            if (m_filterFile.FilterFullPath != null)
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
                Save();
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
                using (SaveFileDialog fileDialog = new SaveFileDialog())
                {
                    fileDialog.InitialDirectory = Path.GetDirectoryName(m_filterFile.SourceSolutionFullPath);
                    fileDialog.Filter = "Solution Filter File (*.slnfilter)|*.slnfilter|All files (*.*)|*.*";
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        m_filterFile.FilterFullPath = fileDialog.FileName;
                        m_filterFile.Save();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error while creating the filter file.\nException: {0}", ex));
            }
            return false;
        }
    }
}