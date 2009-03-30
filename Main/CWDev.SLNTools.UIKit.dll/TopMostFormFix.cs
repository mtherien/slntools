using System;
using System.Drawing;
using System.Windows.Forms;

namespace CWDev.SLNTools.UIKit
{
    public class TopMostFormFix : IDisposable
    {
        public TopMostFormFix()
        {
            m_form = new Form();

            // We do not want anyone to see this window so position it off the 
            // visible screen and make it as small as possible
            m_form.Size = new System.Drawing.Size(1, 1);
            m_form.StartPosition = FormStartPosition.Manual;
            m_form.ShowInTaskbar = false;
            m_form.Location = new Point(0, SystemInformation.VirtualScreen.Right + 10);
            m_form.Show();

            // Make this form the active form and make it TopMost
            m_form.Focus();
            m_form.BringToFront();
            m_form.TopMost = true;
        }

        private Form m_form;

        public void Dispose()
        {
            m_form.Dispose();
        }
    }
}
