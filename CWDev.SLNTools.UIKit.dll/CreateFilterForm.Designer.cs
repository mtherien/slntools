namespace CWDev.SLNTools.UIKit
{
    partial class CreateFilterForm
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
            System.Windows.Forms.Label labelSourceSolution;
            System.Windows.Forms.Label labelSelectTheProjectsYouWishToKeep;
            this.m_groupboxOptions = new System.Windows.Forms.GroupBox();
            this.m_checkboxWatchForChangesOnFilteredSolution = new System.Windows.Forms.CheckBox();
            this.m_labelSelected = new System.Windows.Forms.Label();
            this.m_textboxSourceSolution = new System.Windows.Forms.TextBox();
            this.m_labelErrorMessage = new System.Windows.Forms.Label();
            this.m_menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuitemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_menuitemSave = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuitemSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_menuitemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.m_treeview = new System.Windows.Forms.TreeView();
            this.m_buttonSaveAndQuit = new System.Windows.Forms.Button();
            labelSourceSolution = new System.Windows.Forms.Label();
            labelSelectTheProjectsYouWishToKeep = new System.Windows.Forms.Label();
            this.m_groupboxOptions.SuspendLayout();
            this.m_menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelSourceSolution
            // 
            labelSourceSolution.AutoSize = true;
            labelSourceSolution.Location = new System.Drawing.Point(12, 40);
            labelSourceSolution.Name = "labelSourceSolution";
            labelSourceSolution.Size = new System.Drawing.Size(85, 13);
            labelSourceSolution.TabIndex = 0;
            labelSourceSolution.Text = "&Source Solution:";
            // 
            // labelSelectTheProjectsYouWishToKeep
            // 
            labelSelectTheProjectsYouWishToKeep.AutoSize = true;
            labelSelectTheProjectsYouWishToKeep.Location = new System.Drawing.Point(12, 74);
            labelSelectTheProjectsYouWishToKeep.Name = "labelSelectTheProjectsYouWishToKeep";
            labelSelectTheProjectsYouWishToKeep.Size = new System.Drawing.Size(181, 13);
            labelSelectTheProjectsYouWishToKeep.TabIndex = 2;
            labelSelectTheProjectsYouWishToKeep.Text = "Select the &projects you wish to keep:";
            // 
            // m_groupboxOptions
            // 
            this.m_groupboxOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_groupboxOptions.Controls.Add(this.m_checkboxWatchForChangesOnFilteredSolution);
            this.m_groupboxOptions.Location = new System.Drawing.Point(8, 157);
            this.m_groupboxOptions.Name = "m_groupboxOptions";
            this.m_groupboxOptions.Size = new System.Drawing.Size(452, 47);
            this.m_groupboxOptions.TabIndex = 5;
            this.m_groupboxOptions.TabStop = false;
            this.m_groupboxOptions.Text = "Options (when the filter file is opened)";
            // 
            // m_checkboxWatchForChangesOnFilteredSolution
            // 
            this.m_checkboxWatchForChangesOnFilteredSolution.AutoSize = true;
            this.m_checkboxWatchForChangesOnFilteredSolution.Location = new System.Drawing.Point(6, 19);
            this.m_checkboxWatchForChangesOnFilteredSolution.Name = "m_checkboxWatchForChangesOnFilteredSolution";
            this.m_checkboxWatchForChangesOnFilteredSolution.Size = new System.Drawing.Size(223, 17);
            this.m_checkboxWatchForChangesOnFilteredSolution.TabIndex = 0;
            this.m_checkboxWatchForChangesOnFilteredSolution.Text = "&Watch for changes on the filtered solution";
            this.m_checkboxWatchForChangesOnFilteredSolution.UseVisualStyleBackColor = true;
            // 
            // m_labelSelected
            // 
            this.m_labelSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_labelSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.m_labelSelected.Location = new System.Drawing.Point(237, 74);
            this.m_labelSelected.Name = "m_labelSelected";
            this.m_labelSelected.Size = new System.Drawing.Size(220, 13);
            this.m_labelSelected.TabIndex = 3;
            // 
            // m_textboxSourceSolution
            // 
            this.m_textboxSourceSolution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_textboxSourceSolution.Location = new System.Drawing.Point(117, 37);
            this.m_textboxSourceSolution.Name = "m_textboxSourceSolution";
            this.m_textboxSourceSolution.ReadOnly = true;
            this.m_textboxSourceSolution.Size = new System.Drawing.Size(340, 20);
            this.m_textboxSourceSolution.TabIndex = 1;
            // 
            // m_labelErrorMessage
            // 
            this.m_labelErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_labelErrorMessage.ForeColor = System.Drawing.Color.Red;
            this.m_labelErrorMessage.Location = new System.Drawing.Point(9, 207);
            this.m_labelErrorMessage.Name = "m_labelErrorMessage";
            this.m_labelErrorMessage.Size = new System.Drawing.Size(448, 13);
            this.m_labelErrorMessage.TabIndex = 6;
            // 
            // m_menuStrip
            // 
            this.m_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.m_menuStrip.Location = new System.Drawing.Point(0, 0);
            this.m_menuStrip.Name = "m_menuStrip";
            this.m_menuStrip.Size = new System.Drawing.Size(472, 24);
            this.m_menuStrip.TabIndex = 0;
            this.m_menuStrip.Text = "m_menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuitemOpen,
            this.toolStripSeparator1,
            this.m_menuitemSave,
            this.m_menuitemSaveAs,
            this.toolStripSeparator2,
            this.m_menuitemExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // m_menuitemOpen
            // 
            this.m_menuitemOpen.Name = "m_menuitemOpen";
            this.m_menuitemOpen.Size = new System.Drawing.Size(136, 22);
            this.m_menuitemOpen.Text = "Open...";
            this.m_menuitemOpen.Click += new System.EventHandler(this.m_menuitemOpen_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            // 
            // m_menuitemSave
            // 
            this.m_menuitemSave.Name = "m_menuitemSave";
            this.m_menuitemSave.Size = new System.Drawing.Size(136, 22);
            this.m_menuitemSave.Text = "Save";
            this.m_menuitemSave.Click += new System.EventHandler(this.m_menuitemSave_Click);
            // 
            // m_menuitemSaveAs
            // 
            this.m_menuitemSaveAs.Name = "m_menuitemSaveAs";
            this.m_menuitemSaveAs.Size = new System.Drawing.Size(136, 22);
            this.m_menuitemSaveAs.Text = "Save As...";
            this.m_menuitemSaveAs.Click += new System.EventHandler(this.m_menuitemSaveAs_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(133, 6);
            // 
            // m_menuitemExit
            // 
            this.m_menuitemExit.Name = "m_menuitemExit";
            this.m_menuitemExit.Size = new System.Drawing.Size(136, 22);
            this.m_menuitemExit.Text = "Exit";
            this.m_menuitemExit.Click += new System.EventHandler(this.m_menuitemExit_Click);
            // 
            // m_treeview
            // 
            this.m_treeview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_treeview.CheckBoxes = true;
            this.m_treeview.Location = new System.Drawing.Point(8, 90);
            this.m_treeview.Name = "m_treeview";
            this.m_treeview.Size = new System.Drawing.Size(452, 61);
            this.m_treeview.TabIndex = 4;
            this.m_treeview.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.m_treeview_AfterCheck);
            // 
            // m_buttonSaveAndQuit
            // 
            this.m_buttonSaveAndQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_buttonSaveAndQuit.Location = new System.Drawing.Point(359, 231);
            this.m_buttonSaveAndQuit.Name = "m_buttonSaveAndQuit";
            this.m_buttonSaveAndQuit.Size = new System.Drawing.Size(101, 23);
            this.m_buttonSaveAndQuit.TabIndex = 7;
            this.m_buttonSaveAndQuit.Text = "&Save && Quit";
            this.m_buttonSaveAndQuit.UseVisualStyleBackColor = true;
            this.m_buttonSaveAndQuit.Click += new System.EventHandler(this.m_buttonSaveAndQuit_Click);
            // 
            // CreateFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 260);
            this.Controls.Add(this.m_groupboxOptions);
            this.Controls.Add(this.m_buttonSaveAndQuit);
            this.Controls.Add(this.m_treeview);
            this.Controls.Add(this.m_labelSelected);
            this.Controls.Add(this.m_labelErrorMessage);
            this.Controls.Add(labelSelectTheProjectsYouWishToKeep);
            this.Controls.Add(labelSourceSolution);
            this.Controls.Add(this.m_textboxSourceSolution);
            this.Controls.Add(this.m_menuStrip);
            this.MainMenuStrip = this.m_menuStrip;
            this.MinimumSize = new System.Drawing.Size(480, 294);
            this.Name = "CreateFilterForm";
            this.Text = "Create Filter";
            this.m_groupboxOptions.ResumeLayout(false);
            this.m_groupboxOptions.PerformLayout();
            this.m_menuStrip.ResumeLayout(false);
            this.m_menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_textboxSourceSolution;
        private System.Windows.Forms.Label m_labelErrorMessage;
        private System.Windows.Forms.Label m_labelSelected;
        private System.Windows.Forms.MenuStrip m_menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_menuitemOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem m_menuitemSave;
        private System.Windows.Forms.ToolStripMenuItem m_menuitemSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem m_menuitemExit;
        private System.Windows.Forms.TreeView m_treeview;
        private System.Windows.Forms.Button m_buttonSaveAndQuit;
        private System.Windows.Forms.CheckBox m_checkboxWatchForChangesOnFilteredSolution;
        private System.Windows.Forms.GroupBox m_groupboxOptions;
    }
}