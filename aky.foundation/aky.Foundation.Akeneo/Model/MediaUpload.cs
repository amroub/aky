using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Model
{
    [ExcludeFromCodeCoverage]
    public class MediaUpload
	{
		/// <summary>
		/// The product to which the media file will be associated.
		/// </summary>
		public MediaProduct Product { get; set; }

		/// <summary>
		/// Path to the file
		/// </summary>
		public string FilePath { get; set; }

		/// <summary>
		/// Name of the file
		/// </summary>
		public string FileName { get; set; }

		public MediaUpload() {
			Product = new MediaProduct();
		}
	}

    
}
