using GitLogLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace GitLogLib.GitLog
{
    /// <summary>
    /// git log output parser
    /// </summary>
    public class GitLogReader
    {
        /// <summary>
        /// Function to read standard output of git log
        /// </summary>
        /// <param name="gitCommand"></param>
        /// <param name="gitPath"></param>
        /// <returns></returns>
        public static List<GitLogCommitModel> Read(string gitCommand, string gitPath)
        {
            // git.exe process configulation
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = gitCommand;
            psInfo.CreateNoWindow = true;
            psInfo.UseShellExecute = false;
            psInfo.RedirectStandardOutput = true;
            psInfo.WorkingDirectory = gitPath;
            psInfo.Arguments = @" log --oneline --date=short --numstat --no-merges --pretty=format:""<<%an>> <<%ad>>""";
            psInfo.EnvironmentVariables.Add("TERM", "msys");

            // start git log
            Process p = Process.Start(psInfo);
            List<GitLogCommitModel> result = streamRead(p.StandardOutput);

            p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return result;
        }

        /// <summary>
        /// Parse each line from the git.exe output
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        private static List<GitLogCommitModel> streamRead(StreamReader sr)
        {
            List<GitLogCommitModel> commitList = new List<GitLogCommitModel>();

            string name = null;
            DateTime dt = DateTime.Now;
            int commitId = 0;

            // Regular expression for author and commit date, effected lines
            Regex r = new Regex("^<<([\\w\\W]+)>> <<([\\w\\W]+)>>$");
            Regex rx = new Regex(@"^([\-\d]+)\t([\-\d]+)\t([^\t]+)$");

            string ln;
            while ((ln = sr.ReadLine()) != null)
            {
                ln = ln.Trim('\t', '\r', '\n', ' ');

                // check blank line
                if (string.IsNullOrWhiteSpace(ln)) continue;

                // search author and commit date
                Match m = r.Match(ln);
                if (m.Success)
                {
                    name = m.Groups[1].Value;
                    dt = DateTime.ParseExact(
                        m.Groups[2].Value,
                        "yyyy-MM-dd",
                        System.Globalization.DateTimeFormatInfo.InvariantInfo);

                    commitId += 1;
                }
                else
                {
                    // search for each edited source file
                    Match mx = rx.Match(ln);
                    if (mx.Success)
                    {
                        // add to the list
                        GitLogCommitModel row = new GitLogCommitModel();
                        row.Author = name;
                        row.CommitDate = dt.Date.ToShortDateString();
                        row.RowsAdded = mx.Groups[1].Value == "-" ? 0 : int.Parse(mx.Groups[1].Value);
                        row.RowsDeleted = mx.Groups[2].Value == "-" ? 0 : int.Parse(mx.Groups[2].Value);
                        row.SourceFile = mx.Groups[3].Value;
                        row.CommitId = commitId.ToString();
                        commitList.Add(row);
                    }
                }
            }

            return commitList;
        }
    }
}
