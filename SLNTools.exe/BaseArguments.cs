using CWDev.SLNTools.CommandLine;

namespace CWDev.SLNTools
{
    internal class BaseArguments
    {
        [DefaultArgument(ArgumentType.Required, HelpText = "Command Name (CompareSolutions|MergeSolutions|CreateFilterFileFromSolution|EditFilterFile|OpenFilterFile|SortProjects)")]
        public CommandOption Command = (CommandOption)(-1);
    }
}