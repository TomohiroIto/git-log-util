using CommandLine.Text;
using GitLogLib;
using GitLogLib.Extension;
using GitLogLib.FileOutput;
using GitLogLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GitLogClient
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineOptions options = new CommandLineOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            try
            {
                // git log query
                GitLogOperator op = new GitLogOperator();
                op.Read(options.GitRepositoryDir, options.GitExePath);

                // output directory
                string outputDir = options.OutputDir;
                if(string.IsNullOrWhiteSpace( outputDir))
                {
                    outputDir = Environment.CurrentDirectory;
                }

                // file output
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                using (StreamWriter writer = new StreamWriter(Path.Combine(outputDir, @"a.csv"), false, encoding))
                {
                    op.OutputDailyReportCsv(writer);
                }

                using (StreamWriter writer = new StreamWriter(Path.Combine(outputDir, @"b.json"), false, encoding))
                {
                    op.OutputGoogleDTJson(writer, OutputType.CommitCount);
                }

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                Console.WriteLine("\nSource ---\n{0}", ex.Source);
                Console.WriteLine("\nStackTrace ---\n{0}", ex.StackTrace);
                Environment.Exit(-1);
            }
        }

        public class CommandLineOptions
        {
            [CommandLine.Option('r', "repository", Required = true, HelpText = "Local git repository directory to be processed.")]
            public string GitRepositoryDir { get; set; }

            [CommandLine.Option('o', "output", HelpText = "Output directory for the output.")]
            public string OutputDir { get; set; }

            [CommandLine.Option('g', "git", HelpText = "git.exe full path.")]
            public string GitExePath { get; set; }

            [CommandLine.HelpOption('h', "help", HelpText = "Display this help screen.")]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }
    }
}
