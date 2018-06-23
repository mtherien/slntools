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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CWDev.SLNTools.CommandErrorReporters;

namespace CWDev.SLNTools
{
    using CommandLine;

    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new NoArgumentsStart());
                return;
            }

            try
            {
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

                var parsedArguments = new BaseArguments();
                Parser.ParseArguments(commandName, parsedArguments);

                var commandErrorReporter = new ConsoleCommandErrorReporter
                            {
                                CommandUsage = Parser.ArgumentsUsage(parsedArguments.GetType()),
                                CommandName = commandName.First()
                            };

                var commandRunner = new CommandRunner();
                commandRunner.RunCommand(parsedArguments.Command, commandErrorReporter, commandArguments);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected Exception: {ex.Message}");
            }
        }

        
    }
}
