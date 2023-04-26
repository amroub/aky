using System.Diagnostics.CodeAnalysis;

namespace Akeneo.Model.Attributes
{
    [ExcludeFromCodeCoverage]
    public class FileAttribute : TypedAttributeBase
	{
		public override string Type => AttributeType.File;
	}
}
