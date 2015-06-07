using GitLogLib.Models;
using System.Collections.Generic;
using System.Linq;

namespace GitLogLib.Util
{
    /// <summary>
    /// Utility to get author list from git log summary list
    /// </summary>
    public class AuthorListUtil
    {
        public static List<string> GetAuthorList(List<GitLogSumModel> list)
        {
            List<string> authorList = new List<string>();

            foreach (GitLogSumModel sum in list)
            {
                if (!authorList.Contains<string>(sum.Author))
                {
                    authorList.Add(sum.Author);
                }
            }

            return authorList;
        }
    }
}
