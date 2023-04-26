using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Model.Attributes
{
    [ExcludeFromCodeCoverage]
    public class MultiSelectAttribute : TypedAttributeBase
	{
		public override string Type => AttributeType.MultiSelect;
	}
}