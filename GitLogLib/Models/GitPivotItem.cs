using System.Collections.Generic;

namespace GitLogLib.Models
{
    public class GitLogPivotModel
    {
        public string CommitDate { get; set; }
        public Dictionary<string, GitLogSumModel> SumList { get; set; }
    }
}
