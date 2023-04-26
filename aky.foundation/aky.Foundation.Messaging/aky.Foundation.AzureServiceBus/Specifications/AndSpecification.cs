namespace aky.Foundation.AzureServiceBus.Specifications
{
    using System;

    public class AndSpecification : ISpecification
    {
        private readonly ISpecification leftSpecification;
        private readonly ISpecification rightSpecification;

        public AndSpecification(ISpecification left, ISpecification right)
        {
            this.leftSpecification = left;
            this.rightSpecification = right;
        }

        public string Result() => string.Format("({0} And {1})", this.leftSpecification.Result(), this.rightSpecification.Result());
    }
}
