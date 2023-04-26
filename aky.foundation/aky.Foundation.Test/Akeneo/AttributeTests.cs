namespace Diatly.Foundation.Test.Akeneo
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using global::Akeneo;
    using global::Akeneo.Exceptions;
    using global::Akeneo.Model.Attributes;
    using Xunit;

    public class AttributeTests : BaseFixture
    {
        private IAkeneoClient akeneoClient;

        public AttributeTests()
        {
            this.akeneoClient = this.Resolve<IAkeneoClient>();
        }

        [Fact]
        public async Task GetSingleAttributeAsync_Using_AttributeBase()
        {
            var attribute = await this.akeneoClient.GetAsync<AttributeBase>("description");

            // Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        public async Task GetSingleAttributeAsync_Using_NumberType_Attribute()
        {
            var attribute = await this.akeneoClient.GetAsync<NumberAttribute>("auto_focus_points");

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("pim_catalog_number", attribute.Type);
        }

        [Fact]
        public async Task GetSingleAttributeAsync_Using_SimpleSelect_Attribute()
        {
            var attribute = await this.akeneoClient.GetAsync<SimpleSelectAttribute>("headphone_connectivity");

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("pim_catalog_simpleselect", attribute.Type);
        }

        [Fact]
        public async Task GetSingleAttributeAsync_Using_SimpleSelect_Attribute_GetOptions()
        {
            var options = await this.akeneoClient.GetManyAsync<AttributeOption>("headphone_connectivity");

            // Assert
            Assert.NotNull(options);
            Assert.True(options.Embedded.Items.Count > 0);
        }

        [Fact]
        public async Task GetSingleAttributeAsync_Using_SimpleSelect_Attribute_GetSingleOption()
        {
            var option = await this.akeneoClient.GetAsync<AttributeOption>("headphone_connectivity", "6_3mm");

            // Assert
            Assert.NotNull(option);
        }

        [Fact]
        public async Task GetSingleAttributeAsync_That_Dont_Exists()
        {
            var attribute = await this.akeneoClient.GetAsync<AttributeBase>("xyz");

            // Assert
            Assert.Null(attribute);
        }

        // [Fact]
        public async Task CreateAsync_SimpleTextTypeAttribute()
        {
            var newAtttribute = new TextAttribute()
            {
                Code = "new_attribute",
                Group = "technical",
                Labels = new Dictionary<string, string>() {
                    { "en_US", "new attribute label US" },
                    { "fr_FR", "new attribute label FR" },
                    { "de_DE", "new attribute label DE" }, },
            };
            var response = await this.akeneoClient.CreateAsync(newAtttribute);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.Code);
        }

        [Fact]
        public async Task DeleteAsync_DeleteActionNotSupportedException()
        {
            await Assert.ThrowsAsync<NotSupportedActionException>(async () => await this.akeneoClient.DeleteAsync<TextAttribute>("new_attribute"));
        }
    }
}
