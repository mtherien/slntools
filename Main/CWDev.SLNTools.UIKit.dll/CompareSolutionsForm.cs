using System.Collections.Generic;
using System.Windows.Forms;

namespace CWDev.SLNTools.UIKit
{
    using Core.Merge;

    public partial class CompareSolutionsForm : Form
    {
        public CompareSolutionsForm(Difference difference)
        {
            InitializeComponent();

            m_differences.Data = ((NodeDifference) difference).Subdifferences;
        }

        // TODO add button to send the file to Araxis Merge
    }
}