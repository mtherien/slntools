namespace CWDev.SLNTools.UIKit
{
    partial class UpdateOriginalSolutionForm
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
            System.Windows.Forms.Label labelOriginalSolutionFileState;
            System.Windows.Forms.Label labelMessage;
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateOriginalSolutionForm));
            this.m_buttonYes = new System.Windows.Forms.Button();
            this.m_buttonNo = new System.Windows.Forms.Button();
            this.m_labelState = new System.Windows.Forms.Label();
            this.m_labelStateDescription = new System.Windows.Forms.Label();
            this.m_differencesInFilteredSolution = new CWDev.SLNTools.UIKit.DifferencesControl();
            labelOriginalSolutionFileState = new System.Windows.Forms.Label();
            labelMessage = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelOriginalSolutionFileState
            // 
            labelOriginalSolutionFileState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            labelOriginalSolutionFileState.AutoSize = true;
            labelOriginalSolutionFileState.Location = new System.Drawing.Point(12, 214);
            labelOriginalSolutionFileState.Name = "labelOriginalSolutionFileState";
            labelOriginalSolutionFileState.Size = new System.Drawing.Size(133, 13);
            labelOriginalSolutionFileState.TabIndex = 3;
            labelOriginalSolutionFileState.Text = "Original Solution File State:";
            // 
            // labelMessage
            // 
            labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            labelMessage.Location = new System.Drawing.Point(12, 9);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new System.Drawing.Size(381, 17);
            labelMessage.TabIndex = 0;
            labelMessage.Text = "The following changes has been detected in the filtered solution:";
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            label1.Location = new System.Drawing.Point(12, 151);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(381, 16);
            label1.TabIndex = 6;
            label1.Text = "Do you want to apply these changes to the original solution?";
            // 
            // m_buttonYes
            // 
            this.m_buttonYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_buttonYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.m_buttonYes.Location = new System.Drawing.Point(237, 179);
            this.m_buttonYes.Name = "m_buttonYes";
            this.m_buttonYes.Size = new System.Drawing.Size(75, 23);
            this.m_buttonYes.TabIndex = 1;
            this.m_buttonYes.Text = "&Yes";
            this.m_buttonYes.UseVisualStyleBackColor = true;
            // 
            // m_buttonNo
            // 
            this.m_buttonNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_buttonNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.m_buttonNo.Location = new System.Drawing.Point(318, 179);
            this.m_buttonNo.Name = "m_buttonNo";
            this.m_buttonNo.Size = new System.Drawing.Size(75, 23);
            this.m_buttonNo.TabIndex = 2;
            this.m_buttonNo.Text = "&No";
            this.m_buttonNo.UseVisualStyleBackColor = true;
            // 
            // m_labelState
            // 
            this.m_labelState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_labelState.AutoSize = true;
            this.m_labelState.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_labelState.Location = new System.Drawing.Point(151, 209);
            this.m_labelState.Name = "m_labelState";
            this.m_labelState.Size = new System.Drawing.Size(75, 20);
            this.m_labelState.TabIndex = 4;
            this.m_labelState.Text = "Writable";
            // 
            // m_labelStateDescription
            // 
            this.m_labelStateDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_labelStateDescription.Location = new System.Drawing.Point(12, 236);
            this.m_labelStateDescription.Name = "m_labelStateDescription";
            this.m_labelStateDescription.Size = new System.Drawing.Size(381, 52);
            this.m_labelStateDescription.TabIndex = 5;
            this.m_labelStateDescription.Text = resources.GetString("m_labelStateDescription.Text");
            // 
            // m_differencesInFilteredSolution
            // 
            this.m_differencesInFilteredSolution.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_differencesInFilteredSolution.Location = new System.Drawing.Point(15, 36);
            this.m_differencesInFilteredSolution.Name = "m_differencesInFilteredSolution";
            this.m_differencesInFilteredSolution.Size = new System.Drawing.Size(378, 102);
            this.m_differencesInFilteredSolution.TabIndex = 7;
            // 
            // UpdateOriginalSolutionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 297);
            this.Controls.Add(this.m_differencesInFilteredSolution);
            this.Controls.Add(label1);
            this.Controls.Add(this.m_labelStateDescription);
            this.Controls.Add(labelMessage);
            this.Controls.Add(this.m_labelState);
            this.Controls.Add(labelOriginalSolutionFileState);
            this.Controls.Add(this.m_buttonNo);
            this.Controls.Add(this.m_buttonYes);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(413, 331);
            this.Name = "UpdateOriginalSolutionForm";
            this.Text = "SLNTools - Update Original Solution";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_buttonYes;
        private System.Windows.Forms.Button m_buttonNo;
        private System.Windows.Forms.Label m_labelState;
        private System.Windows.Forms.Label m_labelStateDescription;
        private DifferencesControl m_differencesInFilteredSolution;
    }
}