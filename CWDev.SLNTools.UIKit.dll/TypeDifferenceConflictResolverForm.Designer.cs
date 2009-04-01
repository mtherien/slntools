namespace CWDev.SLNTools.UIKit
{
    partial class TypeDifferenceConflictResolverForm
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
            System.Windows.Forms.Button buttonCancel;
            this.m_buttonAccept = new System.Windows.Forms.Button();
            this.m_radioKeepSource = new System.Windows.Forms.RadioButton();
            this.m_radioKeepDestination = new System.Windows.Forms.RadioButton();
            this.m_textboxChangeDescriptionFromSourceBranch = new System.Windows.Forms.TextBox();
            this.m_textboxChangeDescriptionFromDestinationBranch = new System.Windows.Forms.TextBox();
            this.m_conflictcontextcontrol = new CWDev.SLNTools.UIKit.ConflictContextControl();
            buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            buttonCancel.Location = new System.Drawing.Point(379, 141);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(75, 23);
            buttonCancel.TabIndex = 6;
            buttonCancel.Text = "&Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // m_buttonAccept
            // 
            this.m_buttonAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_buttonAccept.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_buttonAccept.Location = new System.Drawing.Point(298, 141);
            this.m_buttonAccept.Name = "m_buttonAccept";
            this.m_buttonAccept.Size = new System.Drawing.Size(75, 23);
            this.m_buttonAccept.TabIndex = 5;
            this.m_buttonAccept.Text = "&Accept";
            this.m_buttonAccept.UseVisualStyleBackColor = true;
            this.m_buttonAccept.Click += new System.EventHandler(this.m_buttonAccept_Click);
            // 
            // m_radioKeepSource
            // 
            this.m_radioKeepSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_radioKeepSource.Location = new System.Drawing.Point(12, 82);
            this.m_radioKeepSource.Name = "m_radioKeepSource";
            this.m_radioKeepSource.Size = new System.Drawing.Size(193, 24);
            this.m_radioKeepSource.TabIndex = 1;
            this.m_radioKeepSource.TabStop = true;
            this.m_radioKeepSource.Text = "Keep change from &source branch:";
            this.m_radioKeepSource.UseVisualStyleBackColor = true;
            this.m_radioKeepSource.CheckedChanged += new System.EventHandler(this.m_radio_CheckedChanged);
            // 
            // m_radioKeepDestination
            // 
            this.m_radioKeepDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_radioKeepDestination.Location = new System.Drawing.Point(12, 112);
            this.m_radioKeepDestination.Name = "m_radioKeepDestination";
            this.m_radioKeepDestination.Size = new System.Drawing.Size(210, 17);
            this.m_radioKeepDestination.TabIndex = 3;
            this.m_radioKeepDestination.TabStop = true;
            this.m_radioKeepDestination.Text = "Keep change from &destination branch:";
            this.m_radioKeepDestination.UseVisualStyleBackColor = true;
            this.m_radioKeepDestination.CheckedChanged += new System.EventHandler(this.m_radio_CheckedChanged);
            // 
            // m_textboxChangeDescriptionFromSourceBranch
            // 
            this.m_textboxChangeDescriptionFromSourceBranch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_textboxChangeDescriptionFromSourceBranch.Location = new System.Drawing.Point(224, 85);
            this.m_textboxChangeDescriptionFromSourceBranch.Name = "m_textboxChangeDescriptionFromSourceBranch";
            this.m_textboxChangeDescriptionFromSourceBranch.ReadOnly = true;
            this.m_textboxChangeDescriptionFromSourceBranch.Size = new System.Drawing.Size(230, 20);
            this.m_textboxChangeDescriptionFromSourceBranch.TabIndex = 2;
            // 
            // m_textboxChangeDescriptionFromDestinationBranch
            // 
            this.m_textboxChangeDescriptionFromDestinationBranch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_textboxChangeDescriptionFromDestinationBranch.Location = new System.Drawing.Point(224, 111);
            this.m_textboxChangeDescriptionFromDestinationBranch.Name = "m_textboxChangeDescriptionFromDestinationBranch";
            this.m_textboxChangeDescriptionFromDestinationBranch.ReadOnly = true;
            this.m_textboxChangeDescriptionFromDestinationBranch.Size = new System.Drawing.Size(230, 20);
            this.m_textboxChangeDescriptionFromDestinationBranch.TabIndex = 4;
            // 
            // m_conflictcontextcontrol
            // 
            this.m_conflictcontextcontrol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_conflictcontextcontrol.Location = new System.Drawing.Point(12, 12);
            this.m_conflictcontextcontrol.Name = "m_conflictcontextcontrol";
            this.m_conflictcontextcontrol.Size = new System.Drawing.Size(442, 64);
            this.m_conflictcontextcontrol.TabIndex = 0;
            // 
            // TypeDifferenceConflictResolverForm
            // 
            this.AcceptButton = this.m_buttonAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = buttonCancel;
            this.ClientSize = new System.Drawing.Size(466, 202);
            this.ControlBox = false;
            this.Controls.Add(this.m_conflictcontextcontrol);
            this.Controls.Add(this.m_textboxChangeDescriptionFromDestinationBranch);
            this.Controls.Add(this.m_textboxChangeDescriptionFromSourceBranch);
            this.Controls.Add(this.m_buttonAccept);
            this.Controls.Add(buttonCancel);
            this.Controls.Add(this.m_radioKeepDestination);
            this.Controls.Add(this.m_radioKeepSource);
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimumSize = new System.Drawing.Size(470, 210);
            this.Name = "TypeDifferenceConflictResolverForm";
            this.Text = "Type difference conflict";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton m_radioKeepSource;
        private System.Windows.Forms.RadioButton m_radioKeepDestination;
        private System.Windows.Forms.TextBox m_textboxChangeDescriptionFromSourceBranch;
        private System.Windows.Forms.TextBox m_textboxChangeDescriptionFromDestinationBranch;
        private System.Windows.Forms.Button m_buttonAccept;
        private ConflictContextControl m_conflictcontextcontrol;
    }
}