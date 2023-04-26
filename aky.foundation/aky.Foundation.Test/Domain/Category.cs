namespace Diatly.Foundation.Test.Domain
{
    using global::Diatly.Foundation.Ddd.Domain;
    using System.Collections.Generic;

    public class Category : Entity
    {
        public string Name { get; set; }

        public List<Product> Products { get; set; }

        public Category()
        {
            Products = new List<Product>();
        }
    }
}
