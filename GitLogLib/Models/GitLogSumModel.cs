namespace GitLogLib.Models
{
    public class GitLogSumModel
    {
        public string Author { get; set; }
        public string CommitDate { get; set; }
        public int RowsAdded { get; set; }
        public int RowsDeleted { get; set; }
        public int CommitCount { get; set; }
        public int Rows { get { return RowsAdded + RowsDeleted; } }
    }
}
