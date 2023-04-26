using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Model.Attributes
{
    [ExcludeFromCodeCoverage]
    public class SimpleSelectAttribute : TypedAttributeBase
	{
		public override string Type => AttributeType.SimpleSelect;
	}
}
