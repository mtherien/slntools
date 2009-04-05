namespace CWDev.SLNTools.UIKit
{
    partial class CompareSolutionsForm
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
            System.Windows.Forms.GroupBox groupBoxDifferencesFoundInSourceBranch;
            this.m_differences = new CWDev.SLNTools.UIKit.DifferencesControl();
            groupBoxDifferencesFoundInSourceBranch = new System.Windows.Forms.GroupBox();
            groupBoxDifferencesFoundInSourceBranch.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxDifferencesFoundInSourceBranch
            // 
            groupBoxDifferencesFoundInSourceBranch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            groupBoxDifferencesFoundInSourceBranch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            groupBoxDifferencesFoundInSourceBranch.Controls.Add(this.m_differences);
            groupBoxDifferencesFoundInSourceBranch.Location = new System.Drawing.Point(12, 12);
            groupBoxDifferencesFoundInSourceBranch.Name = "groupBoxDifferencesFoundInSourceBranch";
            groupBoxDifferencesFoundInSourceBranch.Size = new System.Drawing.Size(448, 617);
            groupBoxDifferencesFoundInSourceBranch.TabIndex = 0;
            groupBoxDifferencesFoundInSourceBranch.TabStop = false;
            groupBoxDifferencesFoundInSourceBranch.Text = "Differences found";
            // 
            // m_differences
            // 
            this.m_differences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_differences.Location = new System.Drawing.Point(6, 19);
            this.m_differences.Name = "m_differences";
            this.m_differences.Size = new System.Drawing.Size(436, 592);
            this.m_differences.TabIndex = 0;
            // 
            // CompareSolutionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 641);
            this.Controls.Add(groupBoxDifferencesFoundInSourceBranch);
            this.Name = "CompareSolutionsForm";
            this.Text = "Solution Comparer";
            groupBoxDifferencesFoundInSourceBranch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DifferencesControl m_differences;

    }
}