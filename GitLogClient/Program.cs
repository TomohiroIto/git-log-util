using CommandLine.Text;
using GitLogLib;
using GitLogLib.FileOutput;
using GitLogLib.FileOutput.Csv;
using GitLogLib.FileOutput.Json;
using GitLogLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

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
                op.Read(options.GitRepositoryDir);

                // output directory
                string outputDir = options.OutputDir;
                if(string.IsNullOrWhiteSpace( outputDir))
                {
                    outputDir = Environment.CurrentDirectory;
                }

                // file output
                GitLogCsv glCsv = new GitLogCsv();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                using (StreamWriter writer = new StreamWriter(Path.Combine(outputDir, @"a.csv"), false, encoding))
                {
                    glCsv.OutputDailyReportCsv(writer, op.LogSumList);
                }
                using (StreamWriter writer = new StreamWriter(Path.Combine(outputDir, @"b.csv"), false, encoding))
                {
                    glCsv.OutputPivotDataCsv(writer, op.PivotList, op.AuthorList, OutputType.CommitCount);
                }
                using (StreamWriter writer = new StreamWriter(Path.Combine(outputDir, @"c.csv"), false, encoding))
                {
                    glCsv.OutputPivotDataCsv(writer, op.PivotList, op.AuthorList, OutputType.ModifiedRows);
                }
                using (StreamWriter writer = new StreamWriter(Path.Combine(outputDir, @"d.json"), false, encoding))
                {
                    new GoogleDataTableJson().OutputGoogleDTJson(writer, op.PivotList, op.AuthorList, OutputType.CommitCount);
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

            [CommandLine.HelpOption('h', "help", HelpText = "Display this help screen.")]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }
    }
}
