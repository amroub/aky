using Akeneo.Model;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Akeneo.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class MediaDownloadExtensions
    {
        public static void WriteToFile(this MediaDownload download, string filePath, string fileName = null)
        {
            if (download == null)
            {
                throw new ArgumentNullException(nameof(download));
            }
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            fileName = fileName ?? download.FileName.Substring(1, download.FileName.Length - 2);
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var fullPath = Path.Combine(filePath, fileName);
            if (download.Stream.CanSeek)
            {
                download.Stream.Seek(0, SeekOrigin.Begin);
            }
            using (var fileStream = File.Create(fullPath))
            {
                download.Stream.CopyTo(fileStream);
            }
        }
    }
}
