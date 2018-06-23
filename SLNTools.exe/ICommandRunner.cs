using CWDev.SLNTools.CommandErrorReporters;

namespace CWDev.SLNTools
{
    internal interface ICommandRunner
    {
        void RunCommand(CommandOption command, ICommandUsageReporter commandErrorReporter, params string[] parameters);

        string GetCommandUsage(CommandOption command);
    }
}