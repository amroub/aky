using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Model.Attributes
{
    [ExcludeFromCodeCoverage]
    public class ImageAttribute : TypedAttributeBase
	{
		public override string Type => AttributeType.Image;
	}
}