using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unicorn.Core.Utility.Synchronization;

namespace Unicorn.Core.Utility
{
    public class FileDownloader
    {
        private readonly string destinationFolder;
        private readonly HashSet<string> fileNamesBeforeDownload;
        private readonly TimeSpan pollingInterval = TimeSpan.FromMilliseconds(500);
        private readonly DefaultWait wait;
        private string downloadFileName;
        private string[] fileNamesToExclude = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDownloader"/> class.
        /// Used in case when file name is unknown.
        /// </summary>
        /// <param name="destinationFolder">folder containing downloaded file</param>
        public FileDownloader(string destinationFolder) 
            : this(destinationFolder, null, "File was not downloaded in time or properly allocated")
        {
            this.fileNamesBeforeDownload = GetFileNamesFromDestinationFolder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDownloader"/> class.
        /// </summary>
        /// <param name="destinationFolder">folder containing downloaded file</param>
        /// <param name="downloadFileName">file name to wait for</param>
        public FileDownloader(string destinationFolder, string downloadFileName)
            : this(destinationFolder, downloadFileName, $"File '{downloadFileName}' was not downloaded in time")
        {
            this.fileNamesBeforeDownload = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileDownloader"/> class.
        /// Common constructor
        /// </summary>
        /// <param name="destinationFolder">folder containing downloaded file</param>
        /// <param name="downloadFileName">file name to wait for</param>
        /// <param name="errorMessage">error message text in case of fail</param>
        protected FileDownloader(string destinationFolder, string downloadFileName, string errorMessage)
        {
            this.destinationFolder = destinationFolder;
            this.downloadFileName = downloadFileName;

            this.wait = new DefaultWait
            {
                PollingInterval = this.pollingInterval,
                Message = errorMessage
            };
        }

        public string ExpectedFileNamePart { get; set; }

        /// <summary>
        /// Set list of file names or endings to exclude from allocation.
        /// </summary>
        /// <param name="fileNames">array of file names endings</param>
        public void ExcludeFileNames(params string[] fileNames) =>
            this.fileNamesToExclude = fileNames;

        /// <summary>
        /// Wait for file to be downloaded.
        /// </summary>
        /// <param name="timeout">timeout to wait for download</param>
        /// <returns>downloaded file name string</returns>
        public string WaitForFileToBeDownloaded(TimeSpan timeout)
        {
            this.wait.Timeout = timeout;

            if (!string.IsNullOrEmpty(downloadFileName))
            {
                this.wait.Until(ExpectedFileExists);
            }
            else
            {
                this.wait.Until(FileIsAllocated);
                fileNamesBeforeDownload?.Clear();
            }

            return downloadFileName;
        }

        private bool FileIsAllocated()
        {
            // Get current files list from destination folder.
            var currentFiles = GetFileNamesFromDestinationFolder();

            // Filter out files existed already before downloading.s
            currentFiles.ExceptWith(fileNamesBeforeDownload);

            // If there are files to exclude specified, 
            // filter out all files which names end with exclusions list.
            if (fileNamesToExclude != null)
            {
                foreach (var file in fileNamesToExclude)
                {
                    currentFiles.ExceptWith(currentFiles.Where(f => f.EndsWith(file)));
                }
            }

            if (!string.IsNullOrEmpty(this.ExpectedFileNamePart))
            {
                currentFiles.ExceptWith(currentFiles.Where(f => !f.Contains(this.ExpectedFileNamePart)));
            }

            if (!currentFiles.Any())
            {
                return false;
            }

            if (currentFiles.Count > 1)
            {
                throw new FileNotFoundException($"Unable to allocate file: {currentFiles.Count} new files found.");
            }

            downloadFileName = Path.GetFileName(currentFiles.First());
            return true;
        }

        private bool ExpectedFileExists() => 
            File.Exists(downloadFileName);

        /// <summary>
        /// Find all files in destination directory.
        /// </summary>
        /// <returns>set of files names</returns>
        private HashSet<string> GetFileNamesFromDestinationFolder() =>
            new HashSet<string>(Directory.GetFiles(this.destinationFolder));
    }
}
