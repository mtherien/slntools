using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CWDev.SLNTools
{
    public partial class FileSelection : UserControl
    {
        public FileSelection()
        {
            ForOpen = true;
            InitializeComponent();
        }

        public string FileMask { get; set; }

        public FileInfo File { get; set; }

        public bool ForOpen { get; set; }

        public bool ForSave { get; set; }

        private void FileSelection_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ForOpen)
            {
                if (!string.IsNullOrWhiteSpace(txtFileName.Text))
                {
                    var fileInfo = new FileInfo(txtFileName.Text);
                    if (fileInfo.Directory.Exists)
                    {
                        openFileDialog.InitialDirectory = fileInfo.Directory.FullName;
                    }
                }

                openFileDialog.FileName = txtFileName.Text;
                openFileDialog.Filter = FileMask ?? "All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFileName.Text = openFileDialog.FileName;
                    File = new FileInfo(txtFileName.Text);
                }


            }
        }
    }
}
