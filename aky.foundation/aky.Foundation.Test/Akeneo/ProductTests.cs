namespace Diatly.Foundation.Test.Akeneo
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Akeneo;
    using global::Akeneo.Common;
    using global::Akeneo.Consts;
    using global::Akeneo.Model;
    using global::Akeneo.Search;
    using Xunit;

    public class ProductTests : BaseFixture
    {
        private IAkeneoClient akeneoClient;

        public ProductTests()
        {
            this.akeneoClient = this.Resolve<IAkeneoClient>();
        }

        [Fact]
        public async Task GetSingleProduct_ByCode_Async()
        {
            var product = await this.akeneoClient.GetAsync<Product>("1111111171");

            // Assert
            Assert.NotNull(product);
        }

        [Fact]
        public async Task GetProducts_Using_Different_Search_Criteria_On_ProductValue_Equal()
        {
            var searchCriteria = new List<Criteria>() { global::Akeneo.Search.ProductValue.Equal("name", "Military St Tropez") };

            var products = await this.akeneoClient.SearchAsync<Product>(searchCriteria);

            // Assert
            Assert.True(products.Embedded.Items.Count > 0);
        }

        [Fact]
        public async Task GetProducts_Using_Different_Search_Criteria_On_ProductValue_Equal_With_Locale()
        {
            var searchCriteria = new List<Criteria>() { global::Akeneo.Search.ProductValue.Equal("name", "Military St Tropez", null, Locales.FrenchFr) };

            var products = await this.akeneoClient.SearchAsync<Product>(searchCriteria);

            // Assert
            Assert.True(products.Embedded.Items.Count > 0);
        }

        [Fact]
        public async Task GetProducts_Using_Different_Search_Criteria_On_ProductValue_In_For_SimpleSelectType_With_Locale()
        {
            // valid operator to use for simple type / multisimple type are: IN, NOT IN, EMPTY, NOT EMPTY.
            // other operator are not supported.
            var searchCriteria = new List<Criteria>() { global::Akeneo.Search.ProductValue.In("sensor_type", "ccd", null, Locales.FrenchFr) };

            var products = await this.akeneoClient.SearchAsync<Product>(searchCriteria);

            // Assert
            Assert.True(products.Embedded.Items.Count > 0);
        }

        [Fact]
        public async Task GetProducts_Using_Different_Search_Criteria_On_ProductPriceValue_Greater()
        {
            var searchCriteria = new List<Criteria>() { global::Akeneo.Search.ProductPriceValue.Greater("price", 100, Currency.EUR) };

            var products = await this.akeneoClient.SearchAsync<Product>(searchCriteria);

            // Assert
            Assert.True(products.Embedded.Items.Count > 0);
        }

        [Fact]
        public async Task GetProducts_Using_Different_Search_Criteria_On_ProductValue_Contains()
        {
            var searchCriteria = new List<Criteria>() { global::Akeneo.Search.ProductValue.Contains("name", "Military") };

            var products = await this.akeneoClient.SearchAsync<Product>(searchCriteria);

            // Assert
            Assert.True(products.Embedded.Items.Count > 0);
        }

        [Fact]
        public async Task GetProducts_Using_Different_Search_Criteria_On_ProductValue_GreaterThan_For_NumberType()
        {
            var searchCriteria = new List<Criteria>() { global::Akeneo.Search.ProductValue.Greater("optical_zoom", 4) };

            var products = await this.akeneoClient.SearchAsync<Product>(searchCriteria);

            // Assert
            Assert.True(products.Embedded.Items.Count > 0);
        }

        [Fact]
        public async Task GetProducts_Using_Different_Search_Criteria_On_Category_In()
        {
            var searchCriteria = new List<Criteria>() { global::Akeneo.Search.Category.In("print_accessories") };

            var products = await this.akeneoClient.SearchAsync<Product>(searchCriteria);

            // Assert
            Assert.True(products.Embedded.Items.Count > 0);
        }

        [Fact]
        public async Task GetProducts_Using_Different_Search_Criteria_On_Category_In_WithMultipleCode()
        {
            var searchCriteria = new List<Criteria>() { global::Akeneo.Search.Category.In("print_accessories", "samsung") };

            var products = await this.akeneoClient.SearchAsync<Product>(searchCriteria);

            // Assert
            Assert.True(products.Embedded.Items.Count > 0);
        }

        [Fact]
        public async Task GetProducts_Using_Different_Search_Criteria_On_Family_NotIn()
        {
            var searchCriteria = new List<Criteria>() { global::Akeneo.Search.Family.NotIn("accessories") };

            var products = await this.akeneoClient.SearchAsync<Product>(searchCriteria);

            // Assert
            Assert.True(products.Embedded.Items.Count > 0);
        }

        [Fact]
        public async Task GetProducts_Using_Different_Search_Using_MultipleCriteria()
        {
            var searchCriteria = new List<Criteria>()
            {
                global::Akeneo.Search.ProductValue.Equal("name", "Military St Tropez"),
                global::Akeneo.Search.Category.In("print_accessories"),
                global::Akeneo.Search.Family.In("accessories"),
                global::Akeneo.Search.Completeness.Equal(100, AkeneoDefaults.Channel),
            };

            var products = await this.akeneoClient.SearchAsync<Product>(searchCriteria);

            // Assert
            Assert.True(products.Embedded.Items.Count > 0);
        }
    }
}
