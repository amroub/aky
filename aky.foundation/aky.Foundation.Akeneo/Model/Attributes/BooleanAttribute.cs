﻿using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Model.Attributes
{
    [ExcludeFromCodeCoverage]
    public class BooleanAttribute : TypedAttributeBase
	{
		public override string Type => AttributeType.Boolean;
	}
}
