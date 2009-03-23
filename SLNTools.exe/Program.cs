using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CWDev.VSSolutionTools
{
    using CommandLine;
    using Core;
    using Core.Filter;
    using Core.Merge;
    using System.Windows.Forms;

    public enum CommandList
    {
        SortProjects,
        CompareSolutions,
        MergeSolutions,
        CreateFilterFileFromSolution,
        EditFilterFile,
        OpenFilterFile
    }

    class Arguments
    {
        [DefaultArgument(ArgumentType.Required, HelpText = "Command Name")]
        public CommandList Command = (CommandList) 0;
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //args = new string[] { "CompareSolutions", @"C:\Dev\VSSolutionMerger.root\VSSolutionMerger\VSSolutionTools.exe\All Projects (Lors du Branch).sln", @"C:\Dev\VSSolutionMerger.root\VSSolutionMerger\VSSolutionTools.exe\All Projects (Latest Branche WinFixFrs).sln" };
            //args = new string[] { "MergeSolutions", @"C:\Dev\VSSolutionMerger.root\VSSolutionMerger\VSSolutionTools.exe\All Projects (Lors du Branch).sln", @"C:\Dev\VSSolutionMerger.root\VSSolutionMerger\VSSolutionTools.exe\All Projects (Latest Branche WinFixFrs).sln", @"C:\Dev\VSSolutionMerger.root\VSSolutionMerger\VSSolutionTools.exe\All Projects (Latest Main).sln", @"C:\Dev\VSSolutionMerger.root\VSSolutionMerger\VSSolutionTools.exe\Results.sln" };
            //args = new string[] { "CreateFilterFileFromSolution", @"E:\Dev\CC\Main\TousLesProjets.sln" };
            //args = new string[] { "OpenFilterFile", "/SolutionStarter:# Visual Studio 2005|C:\\Program Files (x86)\\Microsoft Visual Studio 8\\Common7\\IDE\\devenv.exe|\"{SolutionFullPath}\"", @"E:\Dev\CC\Main\TestFilter.slnfilter" };
            args = new string[] { "/?" };

            string[] commandName;
            string[] commandArguments;
            if (args.Length > 1)
            {
                commandName = new string[1];
                Array.ConstrainedCopy(args, 0, commandName, 0, 1);
                commandArguments = new string[args.Length - 1];
                Array.ConstrainedCopy(args, 1, commandArguments, 0, commandArguments.Length);
            }
            else
            {
                commandName = args;
                commandArguments = new string[0];
            }

            Arguments parsedArguments = new Arguments();
            if (Parser.ParseArguments(commandName, parsedArguments))
            {
                Command command;
                switch (parsedArguments.Command)
                {
                    case CommandList.CompareSolutions:
                        command = new CompareSolutionsCommand();
                        break;

                    case CommandList.MergeSolutions:
                        command = new MergeSolutionsCommand();
                        break;

                    case CommandList.CreateFilterFileFromSolution:
                        command = new CreateFilterFileFromSolutionCommand();
                        break;

                    case CommandList.OpenFilterFile:
                        command = new OpenFilterFileCommand();
                        break;

                    case CommandList.EditFilterFile:
                        command = new EditFilterFileCommand();
                        break;

                    default:
                        throw new Exception("TODO");
                }
                try
                {
                    command.Run(commandArguments);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("VSSolutionTool.exe usage:\n" + Parser.ArgumentsUsage(parsedArguments.GetType()));
            }
        }
    }
}
