namespace G2Libsys.Library.Extensions
{
    using System.IO;

    /// <summary>
    /// FileExtensions
    /// </summary>
    public static class FileExtensions
    {
        public static bool IsFileBusy(this string file)
        {
            try
            {
                using FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
