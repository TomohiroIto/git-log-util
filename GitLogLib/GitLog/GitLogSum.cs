using GitLogLib.Models;
using System.Collections.Generic;
using System.Linq;

namespace GitLogLib.GitLog
{
    /// <summary>
    /// class for grouping commit log
    /// </summary>
    public class GitLogSum
    {
        /// <summary>
        /// grouping logic
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        public static List<GitLogSumModel> MakeSummary(List<GitLogCommitModel> logs)
        {
            // make group of each commit
            var queryGroups =
                from item in logs
                group item by new
                {
                    AUTHOR = item.Author,
                    COMMIT_DT = item.CommitDate,
                    COMMIT_ID = item.CommitId
                } into newGroup
                orderby newGroup.Key.COMMIT_DT, newGroup.Key.AUTHOR
                select newGroup;

            List<GitLogSumModel> resultList = new List<GitLogSumModel>();
            foreach (var logGroup in queryGroups)
            {
                GitLogSumModel sumRow = new GitLogSumModel();
                sumRow.Author = logGroup.Key.AUTHOR;
                sumRow.CommitDate = logGroup.Key.COMMIT_DT;
                sumRow.CommitId = logGroup.Key.COMMIT_ID;

                sumRow.CommitCount = 0;
                sumRow.RowsAdded = 0;
                sumRow.RowsDeleted = 0;

                foreach (GitLogCommitModel lg in logGroup)
                {
                    sumRow.RowsAdded += lg.RowsAdded;
                    sumRow.RowsDeleted += lg.RowsDeleted;
                    sumRow.CommitCount += 1;
                }

                resultList.Add(sumRow);
            }

            // grouping by each author and commit date
            var qGroupByNmDt =
                from item in resultList
                group item by new
                {
                    AUTHOR = item.Author,
                    COMMIT_DT = item.CommitDate
                } into newGroup
                orderby newGroup.Key.COMMIT_DT, newGroup.Key.AUTHOR
                select newGroup;

            resultList = new List<GitLogSumModel>();
            foreach (var logGroup in qGroupByNmDt)
            {
                GitLogSumModel sumRow = new GitLogSumModel();
                sumRow.Author = logGroup.Key.AUTHOR;
                sumRow.CommitDate = logGroup.Key.COMMIT_DT;

                sumRow.CommitCount = 0;
                sumRow.RowsAdded = 0;
                sumRow.RowsDeleted = 0;

                foreach (GitLogSumModel lg in logGroup)
                {
                    sumRow.RowsAdded += lg.RowsAdded;
                    sumRow.RowsDeleted += lg.RowsDeleted;
                    sumRow.CommitCount += 1;
                }

                resultList.Add(sumRow);
            }

            return resultList;
        }
    }
}
