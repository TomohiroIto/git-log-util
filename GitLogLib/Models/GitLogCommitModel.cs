using System;

namespace GitLogLib.Models
{
    public class GitLogCommitModel
    {
        public string Author { get; set; }
        public string CommitDate { get; set; }
        public int RowsAdded { get; set; }
        public int RowsDeleted { get; set; }
        public string SourceFile { get; set; }
        public string CommitId { get; set; }
    }
}
