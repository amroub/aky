using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Model
{
    [ExcludeFromCodeCoverage]
    public class ProductAssociation
	{
		public List<string> Products { get; set; }
		public List<string> Groups { get; set; }
	}
}