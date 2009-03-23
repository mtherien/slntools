namespace CWDev.VSSolutionTools
{
    using CommandLine;
    using UIKit;

    internal class EditFilterFileCommand : Command
    {
        private class Arguments
        {
            [DefaultArgument(ArgumentType.AtMostOnce)]
            public string FilterFile = null;
        }

        public override void Run(string[] args)
        {
            Arguments parsedArguments = new Arguments();
            if (Parser.ParseArgumentsWithUsage(args, parsedArguments))
            {
                using (CreateFilterForm form = new CreateFilterForm(parsedArguments.FilterFile))
                {
                    form.ShowDialog();
                }
            }
        }
    }
}
