using GitLogLib.FileOutput;
using GitLogLib.FileOutput.Csv;
using GitLogLib.FileOutput.Json;
using GitLogLib.Models;
using System.Collections.Generic;
using System.IO;

namespace GitLogLib.Extension
{
    public static class GitLogOperatorExtensionMethods
    {
        public static void OutputGoogleDTJson(this GitLogOperator op, TextWriter writer, OutputType outType)
        {
            new GoogleDataTableJson().OutputGoogleDTJson(writer, op.PivotList, op.AuthorList, outType);
        }

        public static void OutputPivotDataCsv(this GitLogOperator op, TextWriter writer, OutputType outType)
        {
            new GitLogCsv().OutputPivotDataCsv(writer, op.PivotList, op.AuthorList, outType);
        }

        public static void OutputDailyReportCsv(this GitLogOperator op, TextWriter writer)
        {
            new GitLogCsv().OutputDailyReportCsv(writer, op.LogSumList);
        }
    }
}
