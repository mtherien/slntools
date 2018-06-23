using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CWDev.SLNTools.CommandErrorReporters
{
    internal class WindowErrorReporter : ICommandUsageReporter
    {
        internal WindowErrorReporter()
        {
            CommandName = "";
            CommandUsage = null;
        }

        public string CommandName { get; set; }

        public string CommandUsage { get; set; }

        public void ReportUsage(string message)
        {
            MessageBox.Show(
                string.Format(
                    "{0}\n\nUsage:\n{1} {2}\n{3}",
                    message,
                    Path.GetFileName(Assembly.GetEntryAssembly().Location),
                    CommandName,
                    CommandUsage),
                "SLNTools");
        }
    }
}