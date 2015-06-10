using GitLogLib.Models;
using System.Collections.Generic;
using System.IO;

namespace GitLogLib.FileOutput.Csv
{
    /// <summary>
    /// CSV File writer.
    /// </summary>
    public class GitLogCsv
    {
        /// <summary>
        /// output daily report csv to the given stream.
        /// csv is easily converted to a pivot table in Excel.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="sumList"></param>
        public void OutputDailyReportCsv(TextWriter writer, List<GitLogSumModel> sumList)
        {
            writer.WriteLine("Commit Date,Author,Added Rows,Deleted Rows,Commit Count");

            foreach (GitLogSumModel sum in sumList)
            {
                writer.WriteLine("{0},\"{1}\",{2},{3},{4}", sum.CommitDate, sum.Author.Replace("\"", ""), sum.RowsAdded, sum.RowsDeleted, sum.CommitCount);
            }
        }

        /// <summary>
        /// output pivot table of specified output type data
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="pivotList"></param>
        /// <param name="authorList"></param>
        /// <param name="outType"></param>
        public void OutputPivotDataCsv(TextWriter writer, List<GitLogPivotModel> pivotList, List<string> authorList, OutputType outType)
        {
            for (int i = 0; i < authorList.Count; i++)
            {
                writer.Write(",{0}", authorList[i]);
            }
            writer.WriteLine();

            foreach (GitLogPivotModel pItem in pivotList)
            {
                writer.Write(pItem.CommitDate);

                for (int i = 0; i < authorList.Count; i++)
                {
                    int count = 0;

                    if (pItem.SumList.ContainsKey(authorList[i]))
                    {
                        count = getCountFromModel(pItem.SumList[authorList[i]], outType);
                    }

                    writer.Write(",{0}", count);
                }

                writer.WriteLine();
            }
        }

        private static int getCountFromModel(GitLogSumModel model, OutputType oType)
        {
            switch (oType)
            {
                case OutputType.CommitCount:
                    return model.CommitCount;
                case OutputType.ModifiedRows:
                    return model.Rows;
                case OutputType.AddedRows:
                    return model.RowsAdded;
                case OutputType.DeletedRows:
                    return model.RowsDeleted;
            }

            return 0;
        }
    }
}
