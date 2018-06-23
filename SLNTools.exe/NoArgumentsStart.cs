using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CWDev.SLNTools.CommandErrorReporters;
using CWDev.SLNTools.Commands;

namespace CWDev.SLNTools
{
    public partial class NoArgumentsStart : Form
    {
        private ICommandRunner _commandRunner;

        public NoArgumentsStart()
        {
            InitializeComponent();
            _commandRunner = new CommandRunner();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCommand = (CommandOption) cmbCommand.SelectedItem;
            switch (selectedCommand)
            {
                case CommandOption.CompareSolutions:
                    lblSolution1.Text = "First Solution";
                    lblSolution1.Visible = true;
                    solution1File.FileMask = "Solution File (*.sln)|*.sln|All Files (*.*)|*.*";
                    solution1File.Visible = true;

                    lblSolution2.Text = "Second Solution";
                    lblSolution2.Visible = true;
                    solution2File.FileMask = "Solution File (*.sln)|*.sln|All Files (*.*)|*.*";
                    solution2File.Visible = true;

                    lblSolution3.Visible = false;
                    solution3File.Visible = false;

                    chkCreateOnly.Visible = false;
                    chkIgnoreWarnings.Visible = true;
                    chkWait.Visible = false;
                    break;
                case CommandOption.MergeSolutions:
                    lblSolution1.Text = "First Solution";
                    lblSolution1.Visible = true;
                    solution1File.FileMask = "Solution File (*.sln)|*.sln|All Files (*.*)|*.*";
                    solution1File.Visible = true;

                    lblSolution2.Text = "Second Solution";
                    lblSolution2.Visible = true;
                    solution2File.FileMask = "Solution File (*.sln)|*.sln|All Files (*.*)|*.*";
                    solution2File.Visible = true;

                    lblSolution3.Text = "Destination Solution";
                    lblSolution3.Visible = true;
                    solution3File.FileMask = "Solution File (*.sln)|*.sln|All Files (*.*)|*.*";
                    solution3File.Visible = true;

                    chkCreateOnly.Visible = false;
                    chkIgnoreWarnings.Visible = true;
                    chkWait.Visible = false;
                    break;
                case CommandOption.CreateFilterFileFromSolution:
                    lblSolution1.Text = "Solution";
                    lblSolution1.Visible = true;
                    solution1File.FileMask = "Solution File (*.sln)|*.sln|All Files (*.*)|*.*";
                    solution1File.Visible = true;

                    lblSolution2.Visible = false;
                    solution2File.Visible = false;

                    lblSolution3.Visible = false;
                    solution3File.Visible = false;

                    chkCreateOnly.Visible = false;
                    chkIgnoreWarnings.Visible = false;
                    chkWait.Visible = false;

                    break;
                case CommandOption.EditFilterFile:
                    lblSolution1.Text = "Filter File";
                    lblSolution1.Visible = true;
                    solution1File.FileMask = "Solution File (*.slnfilter)|*.slnfilter|All Files (*.*)|*.*";
                    solution1File.Visible = true;

                    lblSolution2.Visible = false;
                    solution2File.Visible = false;

                    lblSolution3.Visible = false;
                    solution3File.Visible = false;

                    chkCreateOnly.Visible = false;
                    chkIgnoreWarnings.Visible = false;
                    chkWait.Visible = false;
                    break;
                case CommandOption.OpenFilterFile:
                    lblSolution1.Text = "Filter File";
                    lblSolution1.Visible = true;
                    solution1File.FileMask = "Solution File (*.slnfilter)|*.slnfilter|All Files (*.*)|*.*";
                    solution1File.Visible = true;

                    lblSolution2.Visible = false;
                    solution2File.Visible = false;

                    lblSolution3.Visible = false;
                    solution3File.Visible = false;

                    chkCreateOnly.Visible = true;
                    chkIgnoreWarnings.Visible = false;
                    chkWait.Visible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            lblCommandLineExample.Text = $"{Path.GetFileName(Assembly.GetEntryAssembly().Location)} {selectedCommand}\r\n" + 
                _commandRunner.GetCommandUsage(selectedCommand);

        }

        private void NoArgumentsStart_Load(object sender, EventArgs e)
        {
            cmbCommand.Items.Clear();
            cmbCommand.Items.Add(CommandOption.CompareSolutions);
            cmbCommand.Items.Add(CommandOption.MergeSolutions);
            cmbCommand.Items.Add(CommandOption.CreateFilterFileFromSolution);
            cmbCommand.Items.Add(CommandOption.OpenFilterFile);
            cmbCommand.Items.Add(CommandOption.EditFilterFile);
            cmbCommand.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (solution1File.Visible && solution1File.File == null)
            {
                MessageBox.Show(this,
                    $"Please select {lblSolution1.Text}", 
                    "Required field not populated",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

            if (solution2File.Visible && solution2File.File == null)
            {
                MessageBox.Show(this,
                    $"Please select {lblSolution2.Text}",
                    "Required field not populated",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (solution3File.Visible && solution3File.File == null)
            {
                MessageBox.Show(this,
                    $"Please select {lblSolution3.Text}",
                    "Required field not populated",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var arguments = new List<string>();

            if (chkIgnoreWarnings.Visible && chkIgnoreWarnings.Checked)
            {
                arguments.Add("/I");
            }

            if (chkWait.Visible && chkWait.Checked)
            {
                arguments.Add("/W");
            }

            if (chkCreateOnly.Visible && chkCreateOnly.Checked)
            {
                arguments.Add("/C");
            }

            if (solution1File.Visible)
            {
                arguments.Add(solution1File.File.FullName);
            }

            if (solution2File.Visible)
            {
                arguments.Add(solution2File.File.FullName);
            }

            if (solution3File.Visible)
            {
                arguments.Add(solution3File.File.FullName);
            }

            _commandRunner.RunCommand(
                (CommandOption) cmbCommand.SelectedItem, 
                new WindowErrorReporter(),
                arguments.ToArray());
        }
    }
}
