using System;
using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Model.Attributes
{
    [ExcludeFromCodeCoverage]
    public class DateAttribute : TypedAttributeBase
	{
		public override string Type => AttributeType.Date;

		/// <summary>
		/// Minimum date allowed when the attribute type is `pim_catalog_date`
		/// </summary>
		public DateTime DateMin { get; set; }

		/// <summary>
		/// Maximum date allowed when the attribute type is `pim_catalog_date`
		/// </summary>
		public DateTime DateMax { get; set; }
	}
}