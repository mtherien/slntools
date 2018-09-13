using CWDev.SLNTools.CommandLine;
using CWDev.SLNTools.UIKit;

namespace CWDev.SLNTools
{
    class CreateSolutionFromFilterFileCommand : Command
    {
        private class Arguments
        {
            [DefaultArgument(ArgumentType.AtMostOnce)]
            public string FilterFile = null;
        }

        public override void Run(string[] args, MessageBoxErrorReporter reporter)
        {
            var parsedArguments = new Arguments();
            reporter.CommandUsage = Parser.ArgumentsUsage(parsedArguments.GetType());

            if (Parser.ParseArguments(args, parsedArguments, reporter.Handler))
            {
                new CreateSolutionFromFilterFileNoForm().Save(parsedArguments.FilterFile);
            }
        }
    }
}
