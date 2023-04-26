namespace aky.Foundation.AzureServiceBus.Specifications
{
    using System;

    public class OrSpecification : ISpecification
    {
        private readonly ISpecification leftSpecification;
        private readonly ISpecification rightSpecification;

        public OrSpecification(ISpecification left, ISpecification right)
        {
            this.leftSpecification = left;
            this.rightSpecification = right;
        }

        public string Result() => string.Format("({0} OR {1})", this.leftSpecification.Result(), this.rightSpecification.Result());
    }
}
