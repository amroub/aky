using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Akeneo.Model
{
    [ExcludeFromCodeCoverage]
    public class MediaDownload : IDisposable
	{
		public string FileName { get; set; }
		public Stream Stream { get; set; }

		public void Dispose()
		{
			Stream?.Dispose();
		}
	}
}
