namespace Akeneo.Model.Attributes
{
    public abstract class TypedAttributeBase : AttributeBase
    {
        /// <summary>
        /// Attribute type
        /// </summary>
        public abstract string Type { get; }
    }
}
