namespace Diatly.Foundation.Test.Domain
{
    using System.Collections.Generic;
    using global::Diatly.Foundation.Ddd.Domain;

    public class Product : AggregateRoot
    {
        public string ProductName { get; set; }

        public string Description { get; set; }

        public int CategoryID { get; set; }

        public Category Category { get; set; }

        public decimal Price { get; set; }

        public int BrandID { get; set; }

        public int AvailableStock { get; set; }

        public Product()
        {
        }

        // public Product(string ProductName, string description, int category, decimal price, int BrandID, int AvailableStock)
        // {
        //    this.ProductName = ProductName;
        //    this.AvailableStock = AvailableStock;
        //    this.BrandID = BrandID;
        //    this.category = category;
        //    this.description = description;
        //    this.price = price;

        // this.ApplyChange(new ProductCreated() { AggregateRootId = this.Id, ProductName = this.ProductName, BrandID = this.BrandID, price = this.price });
        // }

        // public void ChangePrice(decimal price)
        // {
        // this.ApplyChange(new ProductPriceUpdated() { AggregateRootId = this.Id, UpdatedPrice = price, ProductName = this.ProductName });
        // }

        // private void Apply(ProductPriceUpdated @event)
        // {
        //    this.price = @event.UpdatedPrice;
        // }
    }
}
