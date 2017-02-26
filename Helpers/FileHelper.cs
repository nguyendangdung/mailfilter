using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class FileHelper
    {
        public static bool IsFileLocked(string path)
        {
            FileStream stream = null;
            if (File.Exists(path))
            {
                var file = new FileInfo(path);
                try
                {
                    // Checking with FileAccess.ReadWrite will fail for Read-Only files so the solution 
                    // has been modified to check with FileAccess.Read. While this solution works because
                    // trying to check with FileAccess.Read will fail if the file has a Write or Read lock 
                    // on it, however, this solution will not work if the file doesn't have a Write or Read 
                    // lock on it, i.e. it has been opened (for reading or writing) with FileShare.Read or
                    // FileShare.Write access.
                    stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
                catch (IOException)
                {
                    //the file is unavailable because it is:
                    //still being written to
                    //or being processed by another thread
                    //or does not exist (has already been processed)
                    return true;
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }

                //file is not locked
                return false;
            }

            // todo need to handle in production
            throw new Exception("File is not exist");
        }

        public static async Task<string> ReadContentAsync(string path)
        {
            // todo need to wrap in try/catch or a way to handle exceptions in production
            using (var reader = File.OpenText(path))
            {
                var fileText = await reader.ReadToEndAsync();
                return fileText;
            }
            // return File.ReadAllText(path);
        }
    }
}
