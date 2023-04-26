using System;
using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Search
{
    [ExcludeFromCodeCoverage]
    [Obsolete("Not used any more", true)]
    public class Status : Criteria
	{
		public const string Key = "enabled";

		public static Status Enabled()
		{
			return new Status
			{
				Operator = Operators.Equal,
				Value = true
			};
		}

		public static Status Disabled()
		{
			return new Status
			{
				Operator = Operators.Equal,
				Value = false
			};
		}
	}
}
