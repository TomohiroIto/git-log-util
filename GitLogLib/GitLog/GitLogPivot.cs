using GitLogLib.Models;
using System.Collections.Generic;
using System.Linq;

namespace GitLogLib.GitLog
{
    /// <summary>
    /// class for calculating pivot table like data
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
            List<GitLogPivotModel> result = new List<GitLogPivotModel>();
            // LINQ
            var queryGroups =
                from item in list
                group item by new
                {
                    CommitDt = item.CommitDate
                } into newGroup
                orderby newGroup.Key.CommitDt
                select newGroup;

            foreach (var gp in queryGroups)
            {
                GitLogPivotModel pItem = new GitLogPivotModel();
                pItem.CommitDate = gp.Key.CommitDt;

                foreach (var user in gp)
                {
                    pItem.SumList.Add(user.Author, user);
                }

                result.Add(pItem);
            }

            return result;
        }
    }
}
