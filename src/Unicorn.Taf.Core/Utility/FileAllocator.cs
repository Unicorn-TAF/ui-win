using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unicorn.Taf.Core.Utility.Synchronization;

namespace Unicorn.Taf.Core.Utility
{
    /// <summary>
    /// Provides with ability to wait for and allocate files in file system.
    /// </summary>
    public class FileAllocator
    {
        private readonly string _destinationFolder;
        private readonly HashSet<string> _fileNamesBefore = null;
        private readonly DefaultWait _wait;
        private string expectedFileName = null;
        private string[] fileNamesToExclude = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAllocator"/> class.
        /// Used in case when file name is unknown.
        /// </summary>
        /// <param name="destinationFolder">folder containing downloaded file</param>
        /// <exception cref="DirectoryNotFoundException">is thrown when target directory does not exist</exception>
        public FileAllocator(string destinationFolder) : this()
        {
            if (destinationFolder == null)
            {
                throw new ArgumentNullException(nameof(destinationFolder));
            }

            if (!Directory.Exists(destinationFolder))
            {
                throw new DirectoryNotFoundException($"Targer directory '{destinationFolder}' does not exist.");
            }

            _destinationFolder = destinationFolder;
            _fileNamesBefore = GetFileNamesFromDestinationFolder();
            _wait.ErrorMessage = "File was not appeared in time or properly allocated";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAllocator"/> class.
        /// </summary>
        /// <param name="destinationFolder">folder containing downloaded file</param>
        /// <param name="expectedFileName">file name to wait for</param>
        /// <exception cref="DirectoryNotFoundException">is thrown when target directory does not exist</exception>
        public FileAllocator(string destinationFolder, string expectedFileName) : this()
        {
            if (destinationFolder == null)
            {
                throw new ArgumentNullException(nameof(destinationFolder));
            }

            if (expectedFileName == null)
            {
                throw new ArgumentNullException(nameof(expectedFileName));
            }

            if (!Directory.Exists(destinationFolder))
            {
                throw new DirectoryNotFoundException($"Targer directory '{destinationFolder}' does not exist.");
            }

            _destinationFolder = destinationFolder;
            this.expectedFileName = expectedFileName;
            _wait.ErrorMessage = $"File '{expectedFileName}' was not appeared in time";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAllocator"/> class.
        /// Common constructor
        /// </summary>
        protected FileAllocator()
        {
            _wait = new DefaultWait
            {
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
        }

        /// <summary>
        /// Gets or sets value for expected file name part to simplify allocation process (optional).
        /// </summary>
        public string ExpectedFileNamePart { get; set; }

        /// <summary>
        /// Set list of file names or endings to exclude from allocation.
        /// </summary>
        /// <param name="fileNames">array of file names endings</param>
        public void ExcludeFileNames(params string[] fileNames) =>
            fileNamesToExclude = fileNames;

        /// <summary>
        /// Wait for file to appear in filesystem.
        /// </summary>
        /// <param name="timeout">timeout to wait for file appearance</param>
        /// <returns>desired file name string</returns>
        /// <exception cref="TimeoutException">thrown if more than one file matches search criteria</exception> 
        public string WaitForFileToAppear(TimeSpan timeout) => WaitForFile(timeout);

        /// <summary>
        /// Wait for file to be downloaded.
        /// </summary>
        /// <param name="timeout">timeout to wait for download</param>
        /// <returns>downloaded file name string</returns>
        /// <exception cref="TimeoutException">thrown if more than one file matches search criteria</exception> 
        [Obsolete("please use " + nameof(WaitForFileToAppear) + "instead")]
        public string WaitForFileToBeDownloaded(TimeSpan timeout) => WaitForFile(timeout);

        private string WaitForFile(TimeSpan timeout)
        {
            _wait.Timeout = timeout;

            if (!string.IsNullOrEmpty(expectedFileName))
            {
                _wait.Until(ExpectedFileExists);
            }
            else
            {
                _wait.Until(FileIsAllocated);
                _fileNamesBefore?.Clear();
            }

            return expectedFileName;
        }

        private bool FileIsAllocated()
        {
            // Get current files list from destination folder.
            var currentFiles = GetFileNamesFromDestinationFolder();

            // Filter out files existed already before downloading.s
            currentFiles.ExceptWith(_fileNamesBefore);

            // If there are files to exclude specified, 
            // filter out all files which names end with exclusions list.
            if (fileNamesToExclude != null)
            {
                var listToExclude = new List<string>();

                foreach (var file in fileNamesToExclude)
                {
                    listToExclude.AddRange(currentFiles.Where(f => f.EndsWith(file)));
                }

                currentFiles.ExceptWith(listToExclude);
            }

            if (!string.IsNullOrEmpty(ExpectedFileNamePart))
            {
                var notMatchingFiles = currentFiles.Where(f => !f.Contains(ExpectedFileNamePart));
                currentFiles.ExceptWith(notMatchingFiles);
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

        private bool ExpectedFileExists()
        {
            var file = Path.IsPathRooted(expectedFileName) ? 
                expectedFileName : 
                Path.Combine(_destinationFolder, expectedFileName);

            return File.Exists(file);
        }

        /// <summary>
        /// Find all files in destination directory.
        /// </summary>
        /// <returns>set of files names</returns>
        private HashSet<string> GetFileNamesFromDestinationFolder() =>
            new HashSet<string>(Directory.GetFiles(_destinationFolder));
    }
}
