using GitLogLib.GitLog;
using GitLogLib.Models;
using GitLogLib.Util;
using System.Collections.Generic;

namespace GitLogLib
{
    public class GitLogOperator
    {
        private List<GitLogCommitModel> commitList;
        private List<GitLogSumModel> logSumList;
        private List<GitLogPivotModel> pivotList;
        private List<string> authorList;

        public List<GitLogCommitModel> CommitList
        {
            get { return commitList; }
        }

        public List<GitLogSumModel> LogSumList
        {
            get { return logSumList; }
        }

        public List<GitLogPivotModel> PivotList
        {
            get { return pivotList; }
        }

        public List<string> AuthorList
        {
            get { return authorList; }
        }

        public GitLogOperator()
        {
        }

        public void Read(string repositoryPath, string gitExePath = null)
        {
            if (gitExePath == null)
            {
                gitExePath = GitPathFinder.FindGitPath();
            }

            commitList = GitLogReader.Read(gitExePath, repositoryPath);
            logSumList = GitLogSum.MakeSummary(commitList);
            pivotList = GitLogPivot.CalcPivotTable(logSumList);
            authorList = AuthorListUtil.GetAuthorList(logSumList);
        }
    }
}
