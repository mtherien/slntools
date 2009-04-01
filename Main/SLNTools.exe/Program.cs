#region License

// SLNTools
// Copyright (c) 2009 
// by Christian Warren
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CWDev.SLNTools
{
    using CommandLine;
    using Core;
    using Core.Filter;
    using Core.Merge;

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
            //args = new string[] { "CompareSolutions", @"C:\Dev\VSSolutionMerger.root\VSSolutionMerger\SLNTools.exe\All Projects (Lors du Branch).sln", @"C:\Dev\VSSolutionMerger.root\VSSolutionMerger\SLNTools.exe\All Projects (Latest Branche WinFixFrs).sln" };
            //args = new string[] { "MergeSolutions", @"C:\Dev\VSSolutionMerger.root\VSSolutionMerger\SLNTools.exe\All Projects (Lors du Branch).sln", @"C:\Dev\VSSolutionMerger.root\VSSolutionMerger\SLNTools.exe\All Projects (Latest Branche WinFixFrs).sln", @"C:\Dev\VSSolutionMerger.root\VSSolutionMerger\SLNTools.exe\All Projects (Latest Main).sln", @"C:\Dev\VSSolutionMerger.root\VSSolutionMerger\SLNTools.exe\Results.sln" };
            //args = new string[] { "CreateFilterFileFromSolution", @"C:\DevCodePlex\SLNTools\Main\SLNTools.sln" };
            //args = new string[] { "OpenFilterFile", @"C:\DevCodePlex\SLNTools\Main\Test.slnfilter" };
            //args = new string[] { "/?" };

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
