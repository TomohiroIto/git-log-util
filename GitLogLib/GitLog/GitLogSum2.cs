using GitLogLib.Models;
using System.Collections.Generic;
using System.Linq;

namespace GitLogLib.GitLog
{
    public class GitLogSum2
    {
        /// <summary>
        /// grouping logic
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        public static List<GitLogSumModel> MakeSummary(List<GitCommitModel> logs)
        {
            // make groups by author and commit date
            IEnumerable<GitLogSumModel> q =
                from item in logs
                group item by new
                {
                    Author = item.Author,
                    CommitDate = item.CommitDate
                } into newGroup
                orderby newGroup.Key.CommitDate, newGroup.Key.Author
                select new GitLogSumModel
                {
                    CommitCount = newGroup.GroupBy(p => p.CommitId).Count(),
                    Author = newGroup.Key.Author,
                    CommitDate = newGroup.Key.CommitDate,
                    RowsAdded = newGroup.Sum(p => p.SourceList.Sum(r => r.RowsAdded)),
                    RowsDeleted = newGroup.Sum(p => p.SourceList.Sum(r => r.RowsDeleted))
                };

            return q.ToList();
        }
    }
}
