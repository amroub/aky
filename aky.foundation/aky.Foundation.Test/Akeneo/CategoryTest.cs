namespace Diatly.Foundation.Test.Akeneo
{
    using System.Threading.Tasks;
    using global::Akeneo;
    using global::Akeneo.Extensions;
    using global::Akeneo.Model;
    using Xunit;

    public class CategoryTest : BaseFixture
    {
        private IAkeneoClient akeneoClient;

        public CategoryTest()
        {
            this.akeneoClient = this.Resolve<IAkeneoClient>();
        }

        [Fact]
        public async Task GetCategoriesAsync()
        {
            var categories = await this.akeneoClient.GetManyAsync<Category>();

            // Assert
            Assert.NotNull(categories);
            Assert.True(categories.Embedded.Items.Count > 0);
        }

        [Fact]
        public async Task GetCategories_AccessPaginatedLink()
        {
            // Get initial response
            var categories = await this.akeneoClient.GetManyAsync<Category>();

            // Assert
            Assert.NotNull(categories);

            var nextPageLink = categories.Links["next"].Href;
            string queryString = new System.Uri(nextPageLink).Query;
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);

            int nextPage = int.Parse(queryDictionary["page"], System.Globalization.CultureInfo.InvariantCulture);

            var page2Categories = await this.akeneoClient.GetManyAsync<Category>(nextPage);

            Assert.True(page2Categories.Embedded.Items.Count > 0);
        }

        [Fact]
        public async Task GetCategories_GetAll()
        {
            // Get initial response
            var categories = await this.akeneoClient.GetAllAsync<Category>();

            // Assert
            Assert.True(categories.Count > 0);
        }
    }
}
