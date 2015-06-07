using GitLogLib.Models;
using System.Collections.Generic;
using System.IO;

namespace GitLogLib.FileOutput.Json
{
    public class GoogleDataTableJson
    {
        public void OutputGoogleDTJson(TextWriter writer, List<GitLogPivotModel> pivotList, List<string> authorList, OutputType outType)
        {
            List<string> colList = new List<string>();
            colList.Add("{id: 'commitDate', label: 'Commit Date', type: 'string'}");
            int lp = 0;
            foreach (string author in authorList)
            {
                colList.Add(string.Format(@"{{id: 'num{1}', label: '{0}', type: 'number'}}", author, lp++));
            }

            writer.Write("{ cols: [");
            writer.Write(string.Join(",", colList));
            writer.WriteLine("],");

            writer.WriteLine("rows: [");
            foreach (GitLogPivotModel pItem in pivotList)
            {
                writer.Write(string.Format(@"{{c:[{{v: '{0}'}}", pItem.CommitDate));

                for (int i = 0; i < authorList.Count; i++)
                {
                    int count = 0;

                    if (pItem.SumList.ContainsKey(authorList[i]))
                    {
                        switch (outType)
                        {
                            case OutputType.CommitCount:
                                count = pItem.SumList[authorList[i]].CommitCount;
                                break;
                            case OutputType.ModifiedRows:
                                count = pItem.SumList[authorList[i]].Rows;
                                break;
                            case OutputType.AddedRows:
                                count = pItem.SumList[authorList[i]].RowsAdded;
                                break;
                            case OutputType.DeletedRows:
                                count = pItem.SumList[authorList[i]].RowsDeleted;
                                break;
                        }
                    }

                    writer.Write(string.Format(@", {{v: {0}}}", count));
                }

                writer.WriteLine(@"]}");
            }

            writer.Write("]}");
        }

        private static string getLabelString(OutputType outType)
        {
            switch (outType)
            {
                case OutputType.CommitCount:
                    return "Commit Count";
                case OutputType.ModifiedRows:
                    return "Modified Rows";
                case OutputType.AddedRows:
                    return "Added Rows";
                case OutputType.DeletedRows:
                    return "Deleted Rows";
            }

            return "Count";
        }
    }
}
