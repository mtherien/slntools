namespace CWDev.SLNTools.UIKit
{
    partial class MergeSolutionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Panel mainpanel;
            System.Windows.Forms.GroupBox groupBoxDifferencesFoundInSourceBranch;
            System.Windows.Forms.GroupBox groupBoxDifferencesFoundInDestinationBranch;
            System.Windows.Forms.GroupBox groupboxConflicts;
            System.Windows.Forms.GroupBox groupboxAcceptedDifferences;
            this.m_splitContainerDifferencesFoundAndWorkArea = new System.Windows.Forms.SplitContainer();
            this.m_splitContainerDifferencesFound = new System.Windows.Forms.SplitContainer();
            this.m_differencesInSourceBranchControl = new CWDev.SLNTools.UIKit.DifferencesControl();
            this.m_differencesInDestinationBranchControl = new CWDev.SLNTools.UIKit.DifferencesControl();
            this.m_splitContainerConflictAndResult = new System.Windows.Forms.SplitContainer();
            this.m_conflictsControl = new CWDev.SLNTools.UIKit.ConflictsControl();
            this.m_buttonResolveAll = new System.Windows.Forms.Button();
            this.m_acceptedDifferencesControl = new CWDev.SLNTools.UIKit.DifferencesControl();
            this.m_buttonSave = new System.Windows.Forms.Button();
            this.m_buttonCancel = new System.Windows.Forms.Button();
            mainpanel = new System.Windows.Forms.Panel();
            groupBoxDifferencesFoundInSourceBranch = new System.Windows.Forms.GroupBox();
            groupBoxDifferencesFoundInDestinationBranch = new System.Windows.Forms.GroupBox();
            groupboxConflicts = new System.Windows.Forms.GroupBox();
            groupboxAcceptedDifferences = new System.Windows.Forms.GroupBox();
            mainpanel.SuspendLayout();
            this.m_splitContainerDifferencesFoundAndWorkArea.Panel1.SuspendLayout();
            this.m_splitContainerDifferencesFoundAndWorkArea.Panel2.SuspendLayout();
            this.m_splitContainerDifferencesFoundAndWorkArea.SuspendLayout();
            this.m_splitContainerDifferencesFound.Panel1.SuspendLayout();
            this.m_splitContainerDifferencesFound.Panel2.SuspendLayout();
            this.m_splitContainerDifferencesFound.SuspendLayout();
            groupBoxDifferencesFoundInSourceBranch.SuspendLayout();
            groupBoxDifferencesFoundInDestinationBranch.SuspendLayout();
            this.m_splitContainerConflictAndResult.Panel1.SuspendLayout();
            this.m_splitContainerConflictAndResult.Panel2.SuspendLayout();
            this.m_splitContainerConflictAndResult.SuspendLayout();
            groupboxConflicts.SuspendLayout();
            groupboxAcceptedDifferences.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainpanel
            // 
            mainpanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            mainpanel.Controls.Add(this.m_splitContainerDifferencesFoundAndWorkArea);
            mainpanel.Location = new System.Drawing.Point(12, 12);
            mainpanel.Name = "mainpanel";
            mainpanel.Size = new System.Drawing.Size(806, 584);
            mainpanel.TabIndex = 0;
            // 
            // m_splitContainerDifferencesFoundAndWorkArea
            // 
            this.m_splitContainerDifferencesFoundAndWorkArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_splitContainerDifferencesFoundAndWorkArea.Location = new System.Drawing.Point(0, 0);
            this.m_splitContainerDifferencesFoundAndWorkArea.Name = "m_splitContainerDifferencesFoundAndWorkArea";
            this.m_splitContainerDifferencesFoundAndWorkArea.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // m_splitContainerDifferencesFoundAndWorkArea.Panel1
            // 
            this.m_splitContainerDifferencesFoundAndWorkArea.Panel1.Controls.Add(this.m_splitContainerDifferencesFound);
            // 
            // m_splitContainerDifferencesFoundAndWorkArea.Panel2
            // 
            this.m_splitContainerDifferencesFoundAndWorkArea.Panel2.Controls.Add(this.m_splitContainerConflictAndResult);
            this.m_splitContainerDifferencesFoundAndWorkArea.Size = new System.Drawing.Size(806, 584);
            this.m_splitContainerDifferencesFoundAndWorkArea.SplitterDistance = 157;
            this.m_splitContainerDifferencesFoundAndWorkArea.TabIndex = 1;
            // 
            // m_splitContainerDifferencesFound
            // 
            this.m_splitContainerDifferencesFound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_splitContainerDifferencesFound.Location = new System.Drawing.Point(0, 0);
            this.m_splitContainerDifferencesFound.Name = "m_splitContainerDifferencesFound";
            // 
            // m_splitContainerDifferencesFound.Panel1
            // 
            this.m_splitContainerDifferencesFound.Panel1.Controls.Add(groupBoxDifferencesFoundInSourceBranch);
            // 
            // m_splitContainerDifferencesFound.Panel2
            // 
            this.m_splitContainerDifferencesFound.Panel2.Controls.Add(groupBoxDifferencesFoundInDestinationBranch);
            this.m_splitContainerDifferencesFound.Size = new System.Drawing.Size(806, 157);
            this.m_splitContainerDifferencesFound.SplitterDistance = 403;
            this.m_splitContainerDifferencesFound.TabIndex = 0;
            // 
            // groupBoxDifferencesFoundInSourceBranch
            // 
            groupBoxDifferencesFoundInSourceBranch.Controls.Add(this.m_differencesInSourceBranchControl);
            groupBoxDifferencesFoundInSourceBranch.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxDifferencesFoundInSourceBranch.Location = new System.Drawing.Point(0, 0);
            groupBoxDifferencesFoundInSourceBranch.Name = "groupBoxDifferencesFoundInSourceBranch";
            groupBoxDifferencesFoundInSourceBranch.Size = new System.Drawing.Size(403, 157);
            groupBoxDifferencesFoundInSourceBranch.TabIndex = 0;
            groupBoxDifferencesFoundInSourceBranch.TabStop = false;
            groupBoxDifferencesFoundInSourceBranch.Text = "Differences found in the source branch";
            // 
            // m_differencesInSourceBranchControl
            // 
            this.m_differencesInSourceBranchControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_differencesInSourceBranchControl.Location = new System.Drawing.Point(6, 19);
            this.m_differencesInSourceBranchControl.Name = "m_differencesInSourceBranchControl";
            this.m_differencesInSourceBranchControl.Size = new System.Drawing.Size(391, 132);
            this.m_differencesInSourceBranchControl.TabIndex = 0;
            // 
            // groupBoxDifferencesFoundInDestinationBranch
            // 
            groupBoxDifferencesFoundInDestinationBranch.Controls.Add(this.m_differencesInDestinationBranchControl);
            groupBoxDifferencesFoundInDestinationBranch.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxDifferencesFoundInDestinationBranch.Location = new System.Drawing.Point(0, 0);
            groupBoxDifferencesFoundInDestinationBranch.Name = "groupBoxDifferencesFoundInDestinationBranch";
            groupBoxDifferencesFoundInDestinationBranch.Size = new System.Drawing.Size(399, 157);
            groupBoxDifferencesFoundInDestinationBranch.TabIndex = 0;
            groupBoxDifferencesFoundInDestinationBranch.TabStop = false;
            groupBoxDifferencesFoundInDestinationBranch.Text = "Differences found in the destination branch";
            // 
            // m_differencesInDestinationBranchControl
            // 
            this.m_differencesInDestinationBranchControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_differencesInDestinationBranchControl.Location = new System.Drawing.Point(6, 19);
            this.m_differencesInDestinationBranchControl.Name = "m_differencesInDestinationBranchControl";
            this.m_differencesInDestinationBranchControl.Size = new System.Drawing.Size(387, 132);
            this.m_differencesInDestinationBranchControl.TabIndex = 0;
            // 
            // m_splitContainerConflictAndResult
            // 
            this.m_splitContainerConflictAndResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_splitContainerConflictAndResult.Location = new System.Drawing.Point(0, 0);
            this.m_splitContainerConflictAndResult.Name = "m_splitContainerConflictAndResult";
            this.m_splitContainerConflictAndResult.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // m_splitContainerConflictAndResult.Panel1
            // 
            this.m_splitContainerConflictAndResult.Panel1.Controls.Add(groupboxConflicts);
            // 
            // m_splitContainerConflictAndResult.Panel2
            // 
            this.m_splitContainerConflictAndResult.Panel2.Controls.Add(groupboxAcceptedDifferences);
            this.m_splitContainerConflictAndResult.Size = new System.Drawing.Size(806, 423);
            this.m_splitContainerConflictAndResult.SplitterDistance = 237;
            this.m_splitContainerConflictAndResult.TabIndex = 0;
            // 
            // groupboxConflicts
            // 
            groupboxConflicts.Controls.Add(this.m_conflictsControl);
            groupboxConflicts.Controls.Add(this.m_buttonResolveAll);
            groupboxConflicts.Dock = System.Windows.Forms.DockStyle.Fill;
            groupboxConflicts.Location = new System.Drawing.Point(0, 0);
            groupboxConflicts.Name = "groupboxConflicts";
            groupboxConflicts.Size = new System.Drawing.Size(806, 237);
            groupboxConflicts.TabIndex = 0;
            groupboxConflicts.TabStop = false;
            groupboxConflicts.Text = "Conficts";
            // 
            // m_conflictsControl
            // 
            this.m_conflictsControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_conflictsControl.Location = new System.Drawing.Point(6, 19);
            this.m_conflictsControl.Name = "m_conflictsControl";
            this.m_conflictsControl.Size = new System.Drawing.Size(794, 183);
            this.m_conflictsControl.TabIndex = 0;
            // 
            // m_buttonResolveAll
            // 
            this.m_buttonResolveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_buttonResolveAll.Location = new System.Drawing.Point(708, 208);
            this.m_buttonResolveAll.Name = "m_buttonResolveAll";
            this.m_buttonResolveAll.Size = new System.Drawing.Size(92, 23);
            this.m_buttonResolveAll.TabIndex = 1;
            this.m_buttonResolveAll.Text = "&Resolve all...";
            this.m_buttonResolveAll.UseVisualStyleBackColor = true;
            this.m_buttonResolveAll.Click += new System.EventHandler(this.m_buttonResolveAll_Click);
            // 
            // groupboxAcceptedDifferences
            // 
            groupboxAcceptedDifferences.Controls.Add(this.m_acceptedDifferencesControl);
            groupboxAcceptedDifferences.Controls.Add(this.m_buttonSave);
            groupboxAcceptedDifferences.Controls.Add(this.m_buttonCancel);
            groupboxAcceptedDifferences.Dock = System.Windows.Forms.DockStyle.Fill;
            groupboxAcceptedDifferences.Location = new System.Drawing.Point(0, 0);
            groupboxAcceptedDifferences.Name = "groupboxAcceptedDifferences";
            groupboxAcceptedDifferences.Size = new System.Drawing.Size(806, 182);
            groupboxAcceptedDifferences.TabIndex = 0;
            groupboxAcceptedDifferences.TabStop = false;
            groupboxAcceptedDifferences.Text = "Accepted differences";
            // 
            // m_acceptedDifferencesControl
            // 
            this.m_acceptedDifferencesControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_acceptedDifferencesControl.Location = new System.Drawing.Point(6, 19);
            this.m_acceptedDifferencesControl.Name = "m_acceptedDifferencesControl";
            this.m_acceptedDifferencesControl.Size = new System.Drawing.Size(794, 127);
            this.m_acceptedDifferencesControl.TabIndex = 0;
            // 
            // m_buttonSave
            // 
            this.m_buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_buttonSave.Location = new System.Drawing.Point(644, 152);
            this.m_buttonSave.Name = "m_buttonSave";
            this.m_buttonSave.Size = new System.Drawing.Size(75, 23);
            this.m_buttonSave.TabIndex = 1;
            this.m_buttonSave.Text = "&Save";
            this.m_buttonSave.UseVisualStyleBackColor = true;
            // 
            // m_buttonCancel
            // 
            this.m_buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_buttonCancel.Location = new System.Drawing.Point(725, 152);
            this.m_buttonCancel.Name = "m_buttonCancel";
            this.m_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.m_buttonCancel.TabIndex = 2;
            this.m_buttonCancel.Text = "&Cancel";
            this.m_buttonCancel.UseVisualStyleBackColor = true;
            // 
            // MergeSolutionsForm
            // 
            this.AcceptButton = this.m_buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_buttonCancel;
            this.ClientSize = new System.Drawing.Size(830, 608);
            this.Controls.Add(mainpanel);
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "MergeSolutionsForm";
            this.Text = "Solution Merger";
            this.Load += new System.EventHandler(this.MergeSolutionsForm_Load);
            mainpanel.ResumeLayout(false);
            this.m_splitContainerDifferencesFoundAndWorkArea.Panel1.ResumeLayout(false);
            this.m_splitContainerDifferencesFoundAndWorkArea.Panel2.ResumeLayout(false);
            this.m_splitContainerDifferencesFoundAndWorkArea.ResumeLayout(false);
            this.m_splitContainerDifferencesFound.Panel1.ResumeLayout(false);
            this.m_splitContainerDifferencesFound.Panel2.ResumeLayout(false);
            this.m_splitContainerDifferencesFound.ResumeLayout(false);
            groupBoxDifferencesFoundInSourceBranch.ResumeLayout(false);
            groupBoxDifferencesFoundInDestinationBranch.ResumeLayout(false);
            this.m_splitContainerConflictAndResult.Panel1.ResumeLayout(false);
            this.m_splitContainerConflictAndResult.Panel2.ResumeLayout(false);
            this.m_splitContainerConflictAndResult.ResumeLayout(false);
            groupboxConflicts.ResumeLayout(false);
            groupboxAcceptedDifferences.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_buttonSave;
        private System.Windows.Forms.Button m_buttonCancel;
        private DifferencesControl m_differencesInSourceBranchControl;
        private DifferencesControl m_differencesInDestinationBranchControl;
        private DifferencesControl m_acceptedDifferencesControl;
        private System.Windows.Forms.Button m_buttonResolveAll;
        private ConflictsControl m_conflictsControl;
        private System.Windows.Forms.SplitContainer m_splitContainerConflictAndResult;
        private System.Windows.Forms.SplitContainer m_splitContainerDifferencesFound;
        private System.Windows.Forms.SplitContainer m_splitContainerDifferencesFoundAndWorkArea;



    }
}
