namespace CWDev.SLNTools.CommandErrorReporters
{
    public interface ICommandUsageReporter
    {
        string CommandName { get; set; }
        
        string CommandUsage { get; set; }

        void ReportUsage(string message);
    }
}