using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CWDev.SLNTools.CommandErrorReporters;
using CWDev.SLNTools.CommandLine;
using CWDev.SLNTools.Commands;

namespace CWDev.SLNTools
{
    internal class CommandRunner : ICommandRunner
    {
        public void RunCommand(CommandOption command, ICommandUsageReporter commandErrorReporter, params string[] parameters)
        {
  
            GetCommand(command).Run(parameters, commandErrorReporter);

        }

        public string GetCommandUsage(CommandOption commandOption)
        {
            var command = GetCommand(commandOption);

            return command.CommandLineUsage;

        }

        private Command GetCommand(CommandOption commandOption)
        {
            switch (commandOption)
            {
                case CommandOption.CompareSolutions:
                    return new CompareSolutionsCommand();
                case CommandOption.MergeSolutions:
                    return new MergeSolutionsCommand();
                case CommandOption.CreateFilterFileFromSolution:
                    return new CreateFilterFileFromSolutionCommand();
                case CommandOption.OpenFilterFile:
                    return new OpenFilterFileCommand();
                case CommandOption.EditFilterFile:
                    return new EditFilterFileCommand();
                default:
                    throw new ArgumentException($"Command {commandOption} is not valid");
            }

        }
    }
}
