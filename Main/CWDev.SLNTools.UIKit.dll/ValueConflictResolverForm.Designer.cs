namespace CWDev.SLNTools.UIKit
{
    partial class ValueConflictResolverForm
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
            this.m_radioSelectCustomValue = new System.Windows.Forms.RadioButton();
            this.m_textboxCustomValue = new System.Windows.Forms.TextBox();
            this.m_textboxValueFromSourceBranch = new System.Windows.Forms.TextBox();
            this.m_textboxValueFromDestinationBranch = new System.Windows.Forms.TextBox();
            this.m_conflictcontextcontrol = new CWDev.SLNTools.UIKit.ConflictContextControl();
            buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            buttonCancel.Location = new System.Drawing.Point(376, 183);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(75, 23);
            buttonCancel.TabIndex = 8;
            buttonCancel.Text = "&Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // m_buttonAccept
            // 
            this.m_buttonAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_buttonAccept.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_buttonAccept.Location = new System.Drawing.Point(295, 183);
            this.m_buttonAccept.Name = "m_buttonAccept";
            this.m_buttonAccept.Size = new System.Drawing.Size(75, 23);
            this.m_buttonAccept.TabIndex = 7;
            this.m_buttonAccept.Text = "&Accept";
            this.m_buttonAccept.UseVisualStyleBackColor = true;
            this.m_buttonAccept.Click += new System.EventHandler(this.m_buttonAccept_Click);
            // 
            // m_radioKeepSource
            // 
            this.m_radioKeepSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_radioKeepSource.Location = new System.Drawing.Point(12, 98);
            this.m_radioKeepSource.Name = "m_radioKeepSource";
            this.m_radioKeepSource.Size = new System.Drawing.Size(190, 24);
            this.m_radioKeepSource.TabIndex = 1;
            this.m_radioKeepSource.TabStop = true;
            this.m_radioKeepSource.Text = "Keep value from &source branch:";
            this.m_radioKeepSource.UseVisualStyleBackColor = true;
            this.m_radioKeepSource.CheckedChanged += new System.EventHandler(this.m_radio_CheckedChanged);
            // 
            // m_radioKeepDestination
            // 
            this.m_radioKeepDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_radioKeepDestination.Location = new System.Drawing.Point(12, 128);
            this.m_radioKeepDestination.Name = "m_radioKeepDestination";
            this.m_radioKeepDestination.Size = new System.Drawing.Size(207, 17);
            this.m_radioKeepDestination.TabIndex = 3;
            this.m_radioKeepDestination.TabStop = true;
            this.m_radioKeepDestination.Text = "Keep value from &destination branch:";
            this.m_radioKeepDestination.UseVisualStyleBackColor = true;
            this.m_radioKeepDestination.CheckedChanged += new System.EventHandler(this.m_radio_CheckedChanged);
            // 
            // m_radioSelectCustomValue
            // 
            this.m_radioSelectCustomValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_radioSelectCustomValue.Location = new System.Drawing.Point(12, 154);
            this.m_radioSelectCustomValue.Name = "m_radioSelectCustomValue";
            this.m_radioSelectCustomValue.Size = new System.Drawing.Size(190, 17);
            this.m_radioSelectCustomValue.TabIndex = 5;
            this.m_radioSelectCustomValue.TabStop = true;
            this.m_radioSelectCustomValue.Text = "Select &custom value:";
            this.m_radioSelectCustomValue.UseVisualStyleBackColor = true;
            this.m_radioSelectCustomValue.CheckedChanged += new System.EventHandler(this.m_radio_CheckedChanged);
            // 
            // m_textboxCustomValue
            // 
            this.m_textboxCustomValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_textboxCustomValue.Location = new System.Drawing.Point(224, 153);
            this.m_textboxCustomValue.Name = "m_textboxCustomValue";
            this.m_textboxCustomValue.Size = new System.Drawing.Size(227, 20);
            this.m_textboxCustomValue.TabIndex = 6;
            this.m_textboxCustomValue.TextChanged += new System.EventHandler(this.m_textboxCustomValue_TextChanged);
            // 
            // m_textboxValueFromSourceBranch
            // 
            this.m_textboxValueFromSourceBranch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_textboxValueFromSourceBranch.Location = new System.Drawing.Point(224, 101);
            this.m_textboxValueFromSourceBranch.Name = "m_textboxValueFromSourceBranch";
            this.m_textboxValueFromSourceBranch.ReadOnly = true;
            this.m_textboxValueFromSourceBranch.Size = new System.Drawing.Size(227, 20);
            this.m_textboxValueFromSourceBranch.TabIndex = 2;
            // 
            // m_textboxValueFromDestinationBranch
            // 
            this.m_textboxValueFromDestinationBranch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_textboxValueFromDestinationBranch.Location = new System.Drawing.Point(224, 127);
            this.m_textboxValueFromDestinationBranch.Name = "m_textboxValueFromDestinationBranch";
            this.m_textboxValueFromDestinationBranch.ReadOnly = true;
            this.m_textboxValueFromDestinationBranch.Size = new System.Drawing.Size(227, 20);
            this.m_textboxValueFromDestinationBranch.TabIndex = 4;
            // 
            // m_conflictcontextcontrol
            // 
            this.m_conflictcontextcontrol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_conflictcontextcontrol.Location = new System.Drawing.Point(12, 12);
            this.m_conflictcontextcontrol.Name = "m_conflictcontextcontrol";
            this.m_conflictcontextcontrol.Size = new System.Drawing.Size(439, 80);
            this.m_conflictcontextcontrol.TabIndex = 0;
            // 
            // ValueConflictResolverForm
            // 
            this.AcceptButton = this.m_buttonAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = buttonCancel;
            this.ClientSize = new System.Drawing.Size(463, 218);
            this.ControlBox = false;
            this.Controls.Add(this.m_conflictcontextcontrol);
            this.Controls.Add(this.m_textboxValueFromDestinationBranch);
            this.Controls.Add(this.m_textboxValueFromSourceBranch);
            this.Controls.Add(this.m_textboxCustomValue);
            this.Controls.Add(this.m_buttonAccept);
            this.Controls.Add(buttonCancel);
            this.Controls.Add(this.m_radioSelectCustomValue);
            this.Controls.Add(this.m_radioKeepDestination);
            this.Controls.Add(this.m_radioKeepSource);
            this.MaximumSize = new System.Drawing.Size(10000, 10000);
            this.MinimumSize = new System.Drawing.Size(470, 210);
            this.Name = "ValueConflictResolverForm";
            this.Text = "Value conflict";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton m_radioKeepSource;
        private System.Windows.Forms.RadioButton m_radioKeepDestination;
        private System.Windows.Forms.RadioButton m_radioSelectCustomValue;
        private System.Windows.Forms.TextBox m_textboxCustomValue;
        private System.Windows.Forms.TextBox m_textboxValueFromSourceBranch;
        private System.Windows.Forms.TextBox m_textboxValueFromDestinationBranch;
        private System.Windows.Forms.Button m_buttonAccept;
        private ConflictContextControl m_conflictcontextcontrol;
    }
}