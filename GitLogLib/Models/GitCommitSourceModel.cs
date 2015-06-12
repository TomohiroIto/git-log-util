namespace GitLogLib.Models
{
    public class GitCommitSourceModel
    {
        public int RowsAdded { get; set; }
        public int RowsDeleted { get; set; }
        public string SourceFile { get; set; }
    }
}
