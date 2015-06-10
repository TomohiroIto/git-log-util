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
        /// <summary>
        /// get a list of authors
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<string> GetAuthorList(List<GitLogSumModel> list)
        {
            // group by author
            var p =
                from item in list
                group item by new
                {
                    Author = item.Author
                } into authorGroup
                select authorGroup.Key.Author;

            return p.ToList();
        }
    }
}
