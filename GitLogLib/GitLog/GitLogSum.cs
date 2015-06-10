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
                    RowsAdded = newGroup.Sum(p => p.RowsAdded),
                    RowsDeleted = newGroup.Sum(p => p.RowsDeleted)
                };

            return q.ToList();
        }
    }
}
