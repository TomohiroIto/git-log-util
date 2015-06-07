using System.IO;

namespace GitLogLib.Util
{
    /// <summary>
    /// class to find git.exe from standard installation direcitories
    /// </summary>
    class GitPathFinder
    {
        public static string FindGitPath()
        {
            // 32bit 
            string program_files = System.Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            if (!string.IsNullOrWhiteSpace(program_files))
            {
                string path = Path.Combine(program_files, @"Git\bin\git.exe");
                if (File.Exists(path))
                {
                    return path;
                }
            }

            // 64bit
            program_files = System.Environment.GetEnvironmentVariable("ProgramFiles");
            if (!string.IsNullOrWhiteSpace(program_files))
            {
                string path = Path.Combine(program_files, @"Git\bin\git.exe");
                if (File.Exists(path))
                {
                    return path;
                }
            }

            // default (this works if git.exe is included in PATH environment variable)
            return "git.exe";
        }
    }
}
