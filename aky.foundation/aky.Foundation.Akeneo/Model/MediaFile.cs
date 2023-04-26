using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Akeneo.Client;
using Newtonsoft.Json;

namespace Akeneo.Model
{
    [ExcludeFromCodeCoverage]
    public class MediaFile : ModelBase
	{
		/// <summary>
		/// Media file code
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Original filename of the media file
		/// </summary>
		public string OriginalFilename { get; set; }

		/// <summary>
		/// Mime type of the media file
		/// </summary>
		public string MimeType { get; set; }

		/// <summary>
		/// Size of the media file
		/// </summary>
		public int Size { get; set; }

		/// <summary>
		/// Extension of the media file
		/// </summary>
		public string Extension { get; set; }

		[JsonProperty("_links")]
		public Dictionary<string, PaginationLink> Links { get; set; }
	}
}
