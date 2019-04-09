using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unicorn.Taf.Core.Utility.Synchronization;

namespace Unicorn.Taf.Core.Utility
{
    public class FileAllocator
    {
        private readonly string destinationFolder;
        private readonly HashSet<string> fileNamesBefore = null;
        private readonly DefaultWait wait;
        private string expectedFileName = null;
        private string[] fileNamesToExclude = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAllocator"/> class.
        /// Used in case when file name is unknown.
        /// </summary>
        /// <param name="destinationFolder">folder containing downloaded file</param>
        public FileAllocator(string destinationFolder) : this()
        {
            this.destinationFolder = destinationFolder;
            this.fileNamesBefore = GetFileNamesFromDestinationFolder();
            this.wait.ErrorMessage = "File was not appeared in time or properly allocated";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAllocator"/> class.
        /// </summary>
        /// <param name="destinationFolder">folder containing downloaded file</param>
        /// <param name="expectedFileName">file name to wait for</param>
        public FileAllocator(string destinationFolder, string expectedFileName) : this()
        {
            this.destinationFolder = destinationFolder;
            this.expectedFileName = expectedFileName;
            this.wait.ErrorMessage = $"File '{expectedFileName}' was not appeared in time";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAllocator"/> class.
        /// Common constructor
        /// </summary>
        protected FileAllocator()
        {
            this.wait = new DefaultWait
            {
                PollingInterval = TimeSpan.FromMilliseconds(500)
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

            if (!string.IsNullOrEmpty(expectedFileName))
            {
                this.wait.Until(ExpectedFileExists);
            }
            else
            {
                this.wait.Until(FileIsAllocated);
                fileNamesBefore?.Clear();
            }

            return expectedFileName;
        }

        private bool FileIsAllocated()
        {
            // Get current files list from destination folder.
            var currentFiles = GetFileNamesFromDestinationFolder();

            // Filter out files existed already before downloading.s
            currentFiles.ExceptWith(fileNamesBefore);

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

            expectedFileName = Path.GetFileName(currentFiles.First());
            return true;
        }

        private bool ExpectedFileExists() => 
            File.Exists(expectedFileName);

        /// <summary>
        /// Find all files in destination directory.
        /// </summary>
        /// <returns>set of files names</returns>
        private HashSet<string> GetFileNamesFromDestinationFolder() =>
            new HashSet<string>(Directory.GetFiles(this.destinationFolder));
    }
}
