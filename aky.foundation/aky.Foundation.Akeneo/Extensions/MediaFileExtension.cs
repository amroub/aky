using Akeneo.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class MediaFileExtension
    {
        private const string DownloadKey = "download";

        public static Uri GetDownloadUri(this MediaFile file)
        {
            var link = file.Links.ContainsKey(DownloadKey)
                ? file.Links[DownloadKey]
                : throw new KeyNotFoundException($"Expected link collection to contain {DownloadKey}.");
            return new Uri(link.Href);
        }
    }
}
