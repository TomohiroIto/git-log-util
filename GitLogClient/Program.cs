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
            try
            {
                // git log query
                GitLogOperator op = new GitLogOperator();
                op.Read(@"C:\gitrepo\enterprisesystem");

                // file output
                GitLogCsv ser = new GitLogCsv();
                Encoding sjisEnc = Encoding.GetEncoding("UTF-8");
                using (StreamWriter writer = new StreamWriter(@"C:\Temp\a.csv", false, sjisEnc))
                {
                    ser.OutputDailyReportCsv(writer, op.LogSumList);
                }
                using (StreamWriter writer = new StreamWriter(@"C:\Temp\b.csv", false, sjisEnc))
                {
                    ser.OutputPivotDataCsv(writer, op.PivotList, op.AuthorList, OutputType.CommitCount);
                }
                using (StreamWriter writer = new StreamWriter(@"C:\Temp\c.csv", false, sjisEnc))
                {
                    ser.OutputPivotDataCsv(writer, op.PivotList, op.AuthorList, OutputType.ModifiedRows);
                }
                using (StreamWriter writer = new StreamWriter(@"C:\Temp\d.json", false, sjisEnc))
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
    }
}
