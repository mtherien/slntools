using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;

namespace CWDev.SLNTools.UIKit
{
    using Core.Merge;

    public partial class CompareSolutionsForm : Form
    {
        public CompareSolutionsForm(Difference difference)
        {
            InitializeComponent();
            FormPosition.LoadFromRegistry(this);

            m_differences.Data = ((NodeDifference)difference).Subdifferences;
        }

        // TODO add button to send the file to Default comparer defined in Visual Studio (i.e. *.*)

        protected override void OnClosing(CancelEventArgs e)
        {
            FormPosition.SaveInRegistry(this);
            base.OnClosing(e);
        }
    }
}