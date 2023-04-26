namespace aky.Foundation.AzureServiceBus.Specifications
{
    using System;

    public class LikeSpecification : ISpecification
    {
        private readonly string propertyName;
        private readonly object value;

        public LikeSpecification(string propertyName, object value)
        {
            this.propertyName = propertyName;
            this.value = value;

            if (propertyName == null || value == null)
            {
                throw new ArgumentNullException($"propertyName or value is empty.");
            }
        }

        public string Result() => string.Format("[{0}] LIKE '%{1}%'", this.propertyName, this.value);
    }
}
