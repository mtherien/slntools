namespace CWDev.SLNTools
{
    using CommandLine;
    using UIKit;

    internal class CreateFilterFileFromSolutionCommand : Command
    {
        private class Arguments
        {
            [DefaultArgument(ArgumentType.AtMostOnce)]
            public string SolutionFile = null;
        }

        public override void Run(string[] args)
        {
            Arguments parsedArguments = new Arguments();
            if (Parser.ParseArgumentsWithUsage(args, parsedArguments))
            {
                using (CreateFilterForm form = new CreateFilterForm(parsedArguments.SolutionFile))
                {
                    form.ShowDialog();
                }
            }
        }
    }
}
