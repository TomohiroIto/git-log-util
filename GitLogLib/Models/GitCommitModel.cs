using System.Collections.Generic;

namespace GitLogLib.Models
{
    public class GitCommitModel
    {
        public GitCommitModel()
        {
            SourceList = new List<GitCommitSourceModel>();
        }

        public string CommitId { get; set; }
        public string Author { get; set; }
        public string CommitDate { get; set; }
        public string Comment { get; set; }

        public List<GitCommitSourceModel> SourceList { get; set; }
    }
}
