using GitLogLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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
        public static List<GitCommitModel> Read(string gitCommand, string gitPath)
        {
            // git.exe process configulation
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = gitCommand;
            psInfo.CreateNoWindow = true;
            psInfo.UseShellExecute = false;
            psInfo.RedirectStandardOutput = true;
            psInfo.StandardOutputEncoding = Encoding.UTF8;
            psInfo.WorkingDirectory = gitPath;
            psInfo.Arguments = @" log --oneline --date=short --numstat --no-merges --pretty=format:""<<%h>> <<%an>> <<%ad>> <<%s>>""";
            psInfo.EnvironmentVariables.Add("TERM", "msys");

            // start git log
            Process p = Process.Start(psInfo);
            List<GitCommitModel> result = streamRead(p.StandardOutput);

            p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return result;
        }

        /// <summary>
        /// Parse each line from the git.exe output
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        private static List<GitCommitModel> streamRead(StreamReader sr)
        {
            List<GitCommitModel> commitList = new List<GitCommitModel>();
            GitCommitModel model = null;

            // Regular expression
            Regex r = new Regex("^<<([\\w\\W]+)>> <<([\\w\\W]+)>> <<([\\w\\W]+)>> <<([\\w\\W]+)>>$");
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
                    model = new GitCommitModel();
                    model.CommitId = m.Groups[1].Value;
                    model.Author = m.Groups[2].Value;
                    model.CommitDate = DateTime.ParseExact(
                        m.Groups[3].Value,
                        "yyyy-MM-dd",
                        System.Globalization.DateTimeFormatInfo.InvariantInfo).ToShortDateString();
                    model.Comment = m.Groups[4].Value;
                    commitList.Add(model);
                }
                else
                {
                    // search for each edited source file
                    Match mx = rx.Match(ln);
                    if (mx.Success)
                    {
                        // error check
                        if (model == null)
                        {
                            throw new FormatException("Invalid git log output format");
                        }

                        // add to the list
                        GitCommitSourceModel row = new GitCommitSourceModel();
                        row.RowsAdded = mx.Groups[1].Value == "-" ? 0 : int.Parse(mx.Groups[1].Value);
                        row.RowsDeleted = mx.Groups[2].Value == "-" ? 0 : int.Parse(mx.Groups[2].Value);
                        row.SourceFile = mx.Groups[3].Value;
                        model.SourceList.Add(row);
                    }
                }
            }

            return commitList;
        }
    }
}
