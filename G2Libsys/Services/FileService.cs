namespace G2Libsys.Services
{
    using G2Libsys.Library.Extensions;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// FileService
    /// </summary>
    public class FileService : IFileService
    {
        private SaveFileDialog FileDialog { get; set; }

        /// <summary>
        /// Creates a csv file
        /// </summary>
        /// <param name="fileName">Desired file name</param>
        /// <returns>Bool if file is created</returns>
        public bool CreateFile(string fileName = null)
        {
            FileDialog = new SaveFileDialog()
            {
                FileName = fileName ?? "Rapport",
                DefaultExt = ".csv",
                Filter = "Excel documents (.csv)|*.csv"
            };

            return FileDialog?.ShowDialog() ?? false;
        }

        /// <summary>
        /// Export list to the csv file created
        /// NOTE: CreateFile must be called first
        /// </summary>
        /// <param name="itemList">List of items to export</param>
        /// <returns>Bool save success</returns>
        public bool ExportCSV<T>(List<T> itemList)
        {
            if ((FileDialog?.FileName.IsFileBusy() ?? true) || itemList?.Count <= 0)
            {
                return false;
            }

            // Connect to the file
            FileStream stream = new FileStream(FileDialog.FileName, FileMode.OpenOrCreate);

            // Clear file
            stream.SetLength(0);

            // Create a StreamWriter from FileStream  
            using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

            // Header property names
            writer.WriteLine(string.Join(",", itemList[0].GetType().GetProperties().Select(p => p.Name)));

            // Write each item to the file on a new row
            foreach (var item in itemList)
            {
                // Get item values
                var b = item.GetType().GetProperties().Select(p => p.GetValue(item, null).ToString()).ToList();

                // Write item on new row
                writer.WriteLine(string.Join(",", b));
            }

            return true;
        }
    }
}
