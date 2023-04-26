namespace aky.Foundation.AzureServiceBus.Specifications
{
    using System;

    public class EqualSpecification : ISpecification
    {
        private readonly string propertyName;
        private readonly object value;

        public EqualSpecification(string propertyName, object value)
        {
            this.propertyName = propertyName;
            this.value = value;

            if (propertyName == null || value == null)
            {
                throw new ArgumentNullException($"propertyName or value is empty.");
            }
        }

        public string Result() => string.Format("[{0}] = '{1}'", this.propertyName, this.value);
    }
}
