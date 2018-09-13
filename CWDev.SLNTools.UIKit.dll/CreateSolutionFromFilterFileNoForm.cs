using System;
using System.IO;
using System.Windows.Forms;
using CWDev.SLNTools.Core;
using CWDev.SLNTools.Core.Filter;

namespace CWDev.SLNTools.UIKit
{
    public class CreateSolutionFromFilterFileNoForm
    {
        public void Save(string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                    throw new ArgumentException("No slnfilter file specified.");

                switch (Path.GetExtension(filename).ToLower())
                {
                    case ".slnfilter":
                        var filterFile = FilterFile.FromFile(filename);
                        SolutionFile filteredSolution = filterFile.Apply();
                        filteredSolution.Save();
                        break;

                    default:
                        throw new ArgumentException("File type must be .slnfilter");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error while creating the solution file.\nException: {0}", ex));
            }
        }
    }
}
