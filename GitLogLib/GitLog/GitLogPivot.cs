using GitLogLib.Models;
using System.Collections.Generic;
using System.Linq;

namespace GitLogLib.GitLog
{
    /// <summary>
    /// class for calculating GitLogPivotModel list
    /// </summary>
    public class GitLogPivot
    {
        /// <summary>
        /// calculation
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<GitLogPivotModel> CalcPivotTable(List<GitLogSumModel> list)
        {
            // make groups by commit date
            IEnumerable<GitLogPivotModel> q =
                from item in list
                group item by new
                {
                    CommitDate = item.CommitDate
                } into newGroup
                orderby newGroup.Key.CommitDate
                select new GitLogPivotModel
                {
                    CommitDate = newGroup.Key.CommitDate,
                    SumList = newGroup.ToDictionary(p => p.Author)
                };

            return q.ToList();
        }
    }
}
