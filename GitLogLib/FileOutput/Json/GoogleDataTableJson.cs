﻿using GitLogLib.Models;
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
            writer.Write(string.Join(", ", colList));
            writer.Write("], ");

            writer.Write("rows: [");
            bool isFirstLoop = true;
            foreach (GitLogPivotModel pItem in pivotList)
            {
                writer.Write((isFirstLoop ? "" : ", ") + string.Format(@"{{c: [{{v: '{0}'}}", pItem.CommitDate));

                for (int i = 0; i < authorList.Count; i++)
                {
                    int count = 0;

                    if (pItem.SumList.ContainsKey(authorList[i]))
                    {
                        count = getCountFromModel(pItem.SumList[authorList[i]], outType);
                    }

                    writer.Write(string.Format(@", {{v: {0}}}", count));
                }

                writer.Write(@"]}");
                isFirstLoop = false;
            }

            writer.Write("]}");
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
