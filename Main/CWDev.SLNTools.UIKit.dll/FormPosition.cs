using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace CWDev.SLNTools.UIKit
{
    /// <summary>
    /// Allows the easy loading and saving of control position and size.
    /// Also saves and loads the window position of forms.
    /// </summary>
    public static class FormPosition
    {
        private const string SUBKEY = @"Software\SLNTools\";

        /// <summary>
        /// Saves the position and size of the given form.
        /// Best placed in the OnClosing method of the (parent) Form.
        /// </summary>
        /// <param name="form">The form to be saved.</param>
        public static void SaveInRegistry(
                        Form form)
        {
            if (form == null)
            {
                throw new ArgumentException("The form cannot be null");
            }

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(SUBKEY))
            {
                key.SetValue(form.Name + "_windowState", (int)form.WindowState);

                // if window is not in normal mode set to normal before saving
                if (form.WindowState != FormWindowState.Normal)
                {
                    // save normal windo size
                    form.WindowState = FormWindowState.Normal;
                }

                // save screen res
                key.SetValue(form.Name + "_screenHeight", Screen.PrimaryScreen.Bounds.Height);
                key.SetValue(form.Name + "_screenWidth", Screen.PrimaryScreen.Bounds.Width);

                // control position/size
                key.SetValue(form.Name + "_top", form.Top);
                key.SetValue(form.Name + "_left", form.Left);
                key.SetValue(form.Name + "_width", form.Width);
                key.SetValue(form.Name + "_height", form.Height);
            }
        }

        /// <summary>
        /// Loads the position and size of a form.
        /// Best placed in the Form's constructor.
        /// </summary>
        /// <param name="form">The control to be loaded.</param>
        public static void LoadFromRegistry(
                    Form form)
        {
            if (form == null)
            {
                throw new ArgumentException("The form cannot be null");
            }

            // open key
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(SUBKEY))
            {
                try
                {
                    // check that screen res is the same
                    // if not or can't be found revert to origional.
                    if ((Screen.PrimaryScreen.Bounds.Height != (int)key.GetValue(form.Name + "_screenHeight"))
                        || (Screen.PrimaryScreen.Bounds.Width != (int)key.GetValue(form.Name + "_screenWidth")))
                    {
                        return;
                    }

                    form.SuspendLayout();
                    try
                    {
                        form.WindowState = (FormWindowState)key.GetValue(form.Name + "_windowState");
                        form.StartPosition = FormStartPosition.Manual;
                        form.Top = (int)key.GetValue(form.Name + "_top");
                        form.Left = (int)key.GetValue(form.Name + "_left");
                        form.Width = (int)key.GetValue(form.Name + "_width");
                        form.Height = (int)key.GetValue(form.Name + "_height");
                    }
                    finally
                    {
                        form.ResumeLayout();
                    }
                }
                catch (Exception)
                {
                    // Nothing to do.
                }
            }
        }
    }
}