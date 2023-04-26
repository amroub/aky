﻿using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Model.Attributes
{
    [ExcludeFromCodeCoverage]
    public class NumberAttribute : TypedAttributeBase
	{
		public override string Type => AttributeType.Number;

		/// <summary>
		/// Minimum integer value allowed when the attribute type is `pim_catalog_metric`, `pim_catalog_price` or `pim_catalog_number`
		/// </summary>
		public float? NumberMin { get; set; }

		/// <summary>
		/// Maximum integer value allowed when the attribute type is `pim_catalog_metric`, `pim_catalog_price` or `pim_catalog_number`
		/// </summary>
		public float? NumberMax { get; set; }

		/// <summary>
		/// Whether decimals are allowed when the attribute type is `pim_catalog_metric`, `pim_catalog_price` or `pim_catalog_number`
		/// </summary>
		public bool DecimalsAllowed { get; set; }

		/// <summary>
		/// Whether negative values are allowed when the attribute type is `pim_catalog_metric` or `pim_catalog_number`
		/// </summary>
		public bool NegativeAllowed { get; set; }
	}
}