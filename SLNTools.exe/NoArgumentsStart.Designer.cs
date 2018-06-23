namespace CWDev.SLNTools
{
    partial class NoArgumentsStart
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.cmbCommand = new System.Windows.Forms.ComboBox();
            this.solution2File = new CWDev.SLNTools.FileSelection();
            this.solution1File = new CWDev.SLNTools.FileSelection();
            this.lblSolution2 = new System.Windows.Forms.Label();
            this.lblSolution1 = new System.Windows.Forms.Label();
            this.solution3File = new CWDev.SLNTools.FileSelection();
            this.lblSolution3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCommandLineExample = new System.Windows.Forms.Label();
            this.chkIgnoreWarnings = new System.Windows.Forms.CheckBox();
            this.chkWait = new System.Windows.Forms.CheckBox();
            this.chkCreateOnly = new System.Windows.Forms.CheckBox();
            this.solution4File = new CWDev.SLNTools.FileSelection();
            this.lblSolution4 = new System.Windows.Forms.Label();
            this.lblWaiting = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select a function to perform";
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(1070, 638);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(179, 75);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // cmbCommand
            // 
            this.cmbCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCommand.FormattingEnabled = true;
            this.cmbCommand.Items.AddRange(new object[] {
            "Compare Solutions",
            "Merge Solutions",
            "Create Filter File From Solution",
            "Edit A Filtered Solution"});
            this.cmbCommand.Location = new System.Drawing.Point(332, 30);
            this.cmbCommand.Name = "cmbCommand";
            this.cmbCommand.Size = new System.Drawing.Size(612, 33);
            this.cmbCommand.TabIndex = 14;
            this.cmbCommand.SelectedIndexChanged += new System.EventHandler(this.cmbCommand_SelectedIndexChanged);
            // 
            // solution2File
            // 
            this.solution2File.AllowDrop = true;
            this.solution2File.File = null;
            this.solution2File.FileMask = "Solution File (*.sln)|*.sln|All Files (*.*)|*.*";
            this.solution2File.ForOpen = true;
            this.solution2File.ForSave = false;
            this.solution2File.Location = new System.Drawing.Point(332, 365);
            this.solution2File.Name = "solution2File";
            this.solution2File.Size = new System.Drawing.Size(927, 40);
            this.solution2File.TabIndex = 18;
            // 
            // solution1File
            // 
            this.solution1File.AllowDrop = true;
            this.solution1File.File = null;
            this.solution1File.FileMask = "Solution File (*.sln)|*.sln|All Files (*.*)|*.*";
            this.solution1File.ForOpen = true;
            this.solution1File.ForSave = false;
            this.solution1File.Location = new System.Drawing.Point(332, 308);
            this.solution1File.Name = "solution1File";
            this.solution1File.Size = new System.Drawing.Size(927, 40);
            this.solution1File.TabIndex = 17;
            // 
            // lblSolution2
            // 
            this.lblSolution2.AutoSize = true;
            this.lblSolution2.Location = new System.Drawing.Point(75, 365);
            this.lblSolution2.Name = "lblSolution2";
            this.lblSolution2.Size = new System.Drawing.Size(138, 25);
            this.lblSolution2.TabIndex = 16;
            this.lblSolution2.Text = "New Solution";
            // 
            // lblSolution1
            // 
            this.lblSolution1.AutoSize = true;
            this.lblSolution1.Location = new System.Drawing.Point(75, 308);
            this.lblSolution1.Name = "lblSolution1";
            this.lblSolution1.Size = new System.Drawing.Size(129, 25);
            this.lblSolution1.TabIndex = 15;
            this.lblSolution1.Text = "Old Solution";
            // 
            // solution3File
            // 
            this.solution3File.AllowDrop = true;
            this.solution3File.File = null;
            this.solution3File.FileMask = "Solution File (*.sln)|*.sln|All Files (*.*)|*.*";
            this.solution3File.ForOpen = true;
            this.solution3File.ForSave = false;
            this.solution3File.Location = new System.Drawing.Point(332, 434);
            this.solution3File.Name = "solution3File";
            this.solution3File.Size = new System.Drawing.Size(927, 40);
            this.solution3File.TabIndex = 20;
            // 
            // lblSolution3
            // 
            this.lblSolution3.AutoSize = true;
            this.lblSolution3.Location = new System.Drawing.Point(75, 434);
            this.lblSolution3.Name = "lblSolution3";
            this.lblSolution3.Size = new System.Drawing.Size(90, 25);
            this.lblSolution3.TabIndex = 19;
            this.lblSolution3.Text = "Solution";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOk.Location = new System.Drawing.Point(863, 638);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(179, 75);
            this.btnOk.TabIndex = 21;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(230, 25);
            this.label2.TabIndex = 22;
            this.label2.Text = "Command Line Usage:";
            // 
            // lblCommandLineExample
            // 
            this.lblCommandLineExample.AutoSize = true;
            this.lblCommandLineExample.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommandLineExample.Location = new System.Drawing.Point(327, 90);
            this.lblCommandLineExample.Name = "lblCommandLineExample";
            this.lblCommandLineExample.Size = new System.Drawing.Size(272, 25);
            this.lblCommandLineExample.TabIndex = 23;
            this.lblCommandLineExample.Text = "Command Line Example:";
            // 
            // chkIgnoreWarnings
            // 
            this.chkIgnoreWarnings.AutoSize = true;
            this.chkIgnoreWarnings.Location = new System.Drawing.Point(80, 237);
            this.chkIgnoreWarnings.Name = "chkIgnoreWarnings";
            this.chkIgnoreWarnings.Size = new System.Drawing.Size(201, 29);
            this.chkIgnoreWarnings.TabIndex = 24;
            this.chkIgnoreWarnings.Text = "Ignore Warnings";
            this.chkIgnoreWarnings.UseVisualStyleBackColor = true;
            // 
            // chkWait
            // 
            this.chkWait.AutoSize = true;
            this.chkWait.Location = new System.Drawing.Point(792, 237);
            this.chkWait.Name = "chkWait";
            this.chkWait.Size = new System.Drawing.Size(513, 29);
            this.chkWait.TabIndex = 25;
            this.chkWait.Text = "Wait for solution to open (needed in some cases)";
            this.chkWait.UseVisualStyleBackColor = true;
            // 
            // chkCreateOnly
            // 
            this.chkCreateOnly.AutoSize = true;
            this.chkCreateOnly.Location = new System.Drawing.Point(332, 237);
            this.chkCreateOnly.Name = "chkCreateOnly";
            this.chkCreateOnly.Size = new System.Drawing.Size(432, 29);
            this.chkCreateOnly.TabIndex = 26;
            this.chkCreateOnly.Text = "Create Only. Don\'t open created solution";
            this.chkCreateOnly.UseVisualStyleBackColor = true;
            this.chkCreateOnly.CheckedChanged += new System.EventHandler(this.chkCreateOnly_CheckedChanged);
            // 
            // solution4File
            // 
            this.solution4File.AllowDrop = true;
            this.solution4File.File = null;
            this.solution4File.FileMask = "Solution File (*.sln)|*.sln|All Files (*.*)|*.*";
            this.solution4File.ForOpen = true;
            this.solution4File.ForSave = false;
            this.solution4File.Location = new System.Drawing.Point(332, 505);
            this.solution4File.Name = "solution4File";
            this.solution4File.Size = new System.Drawing.Size(927, 40);
            this.solution4File.TabIndex = 28;
            // 
            // lblSolution4
            // 
            this.lblSolution4.AutoSize = true;
            this.lblSolution4.Location = new System.Drawing.Point(75, 505);
            this.lblSolution4.Name = "lblSolution4";
            this.lblSolution4.Size = new System.Drawing.Size(90, 25);
            this.lblSolution4.TabIndex = 27;
            this.lblSolution4.Text = "Solution";
            // 
            // lblWaiting
            // 
            this.lblWaiting.AutoSize = true;
            this.lblWaiting.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWaiting.ForeColor = System.Drawing.Color.Red;
            this.lblWaiting.Location = new System.Drawing.Point(72, 646);
            this.lblWaiting.Name = "lblWaiting";
            this.lblWaiting.Size = new System.Drawing.Size(735, 46);
            this.lblWaiting.TabIndex = 29;
            this.lblWaiting.Text = "Waiting For Spawned Process To Quit";
            this.lblWaiting.Visible = false;
            // 
            // NoArgumentsStart
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(1315, 760);
            this.Controls.Add(this.lblWaiting);
            this.Controls.Add(this.solution4File);
            this.Controls.Add(this.lblSolution4);
            this.Controls.Add(this.chkCreateOnly);
            this.Controls.Add(this.chkWait);
            this.Controls.Add(this.chkIgnoreWarnings);
            this.Controls.Add(this.lblCommandLineExample);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.solution3File);
            this.Controls.Add(this.lblSolution3);
            this.Controls.Add(this.solution2File);
            this.Controls.Add(this.solution1File);
            this.Controls.Add(this.lblSolution2);
            this.Controls.Add(this.lblSolution1);
            this.Controls.Add(this.cmbCommand);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "NoArgumentsStart";
            this.Text = "Solution File Tools";
            this.Load += new System.EventHandler(this.NoArgumentsStart_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ComboBox cmbCommand;
        private FileSelection solution2File;
        private FileSelection solution1File;
        private System.Windows.Forms.Label lblSolution2;
        private System.Windows.Forms.Label lblSolution1;
        private FileSelection solution3File;
        private System.Windows.Forms.Label lblSolution3;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCommandLineExample;
        private System.Windows.Forms.CheckBox chkIgnoreWarnings;
        private System.Windows.Forms.CheckBox chkWait;
        private System.Windows.Forms.CheckBox chkCreateOnly;
        private FileSelection solution4File;
        private System.Windows.Forms.Label lblSolution4;
        private System.Windows.Forms.Label lblWaiting;
    }
}