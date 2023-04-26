using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Akeneo.Model
{
    [ExcludeFromCodeCoverage]
    public class Locale : ModelBase
	{
		/// <summary>
		/// Locale code
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Whether the locale is enabled
		/// </summary>
		public bool Enabled { get; set; }
	}
}
