#region License

// SLNTools
// Copyright (c) 2009 
// by Christian Warren
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

#endregion

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
