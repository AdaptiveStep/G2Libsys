namespace G2Libsys.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// IFileService
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Creates a csv file
        /// </summary>
        /// <param name="fileName">Desired file name</param>
        /// <returns>Bool if file is created</returns>
        bool? CreateFile(string fileName = null);

        /// <summary>
        /// Export list to the csv file created
        /// NOTE: CreateFile must be called first
        /// </summary>
        /// <param name="itemList">List of items to export</param>
        /// <returns>Bool save success</returns>
        bool ExportCSV<T>(List<T> itemList);
    }
}