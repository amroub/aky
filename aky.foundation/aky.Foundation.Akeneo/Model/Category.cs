using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Model
{
    [ExcludeFromCodeCoverage]
    public class Category : ModelBase
	{
		/// <summary>
		/// Category code (required)
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Category code of the parent's category
		/// </summary>
		public string Parent { get; set; }

		/// <summary>
		/// Category labels for each locale
		/// </summary>
		public Dictionary<string, string> Labels { get; set; }

		public Category()
		{
			Labels = new Dictionary<string, string>();
		}
	}
}
