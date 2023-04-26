namespace aky.EmailService.Tests.Emails.TemplateEngine
{
    using aky.EmailService.TemplateEngine;
    using Xunit;

    public partial class DotLiquidTemplateEngineTest : BaseFixture
    {
        private const string _template =
            @"<!DOCTYPE html>
            <html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
            <body>
            {{Customer.Salutation}} {{Customer.Firstname}} {{Customer.Lastname}}
            <br/><br/>
            thank you for the following order made for items :
            <ul>
                {% for Item in Itemlist -%}
                <li>
                    {{Item.Name}} - Quantity: {{Item.Quantity}}, Amount:{{Item.Amount}} - Total: {{Item.Quantity | times: Item.Amount}} <br/>
                </li>
                {% endfor -%}
            </ul>
            Your Sales Company.
            </body>
            </html>";

        private ITemplateEngine templateEngine;

        public DotLiquidTemplateEngineTest()
        {
            this.templateEngine = this.TemplateEngine;
        }

        [Fact]
        public void DotLiquidTemplateEngine_Prepare_SimpleTemplateRendering_Pass()
        {
            var order = new
            {
                Customer = new
                { Salutation = "Mr.", Firstname = "Jatin", Lastname = "Kacha", },
                Itemlist = new[]
                {
                    new { Name="Test product", Quantity = 2, Amount = 10.25 },
                    new { Name="Test product 2", Quantity = 5, Amount = 5.10 },
                },
            };

            var result = this.templateEngine.Prepare<object>(_template, order);

            Assert.True(result.IndexOf("Test product", System.StringComparison.InvariantCultureIgnoreCase) != -1);
        }

        [Fact]
        public void DotLiquidTemplateEngine_Prepare_SimpleTemplateRenderingWithMissingOneProperty_Pass()
        {
            var order = new
            {
                Itemlist = new[]
                {
                    new { Name="Test product", Quantity = 2, Amount = 10.25 },
                    new { Name="Test product 2", Quantity = 5, Amount = 5.10 },
                },
            };

            var result = this.templateEngine.Prepare<object>(_template, order);

            Assert.True(result.IndexOf("Jatin", System.StringComparison.InvariantCultureIgnoreCase) == -1);
        }

        [Fact]
        public void DotLiquidTemplateEngine_Prepare_TemplateRenderingWithFrenchCharacters_Pass()
        {
            var order = new
            {
                Itemlist = new[]
                {
                    new { Name="Trousse Cuir pailleté Vincénnés 27", Quantity = 2, Amount = 10.25 },
                    new { Name="Test product Àâæçê€", Quantity = 5, Amount = 5.10 },
                },
            };

            var result = this.templateEngine.Prepare<object>(_template, order);

            Assert.True(result.IndexOf("pailleté", System.StringComparison.InvariantCultureIgnoreCase) != -1);
        }

        [Fact]
        public void DotLiquidTemplateEngine_Prepare_TemplateRenderingWithTemplateHasSyntaxError_Pass()
        {
            string templateWithSyntaxError =
            "<!DOCTYPE html>" +
            "<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>" +
            "<body>" +
            "{{Customer.Salutation}} {{Customer.Firstname}} {{Customer.Lastname}}" +
            "<br/><br/>" +
            "thank you for the following order made for items :" +
            "<ul>" +
            "    { for Item in Itemlist -}" +
            "    <li>" +
            "        {{Item.Name}} - Quantity: {{Item.Quantity}}, Amount:{{Item.Amount}} - Total: {{Item.Quantity | times: Item.Amount}} <br/>" +
            "    </li>" +
            "    {% endfor -%}" +
            "</ul>" +
            "Your Sales Company." +
            "</body>" +
            "</html>";

            var order = new
            {
                Itemlist = new[]
                {
                    new { Name="Trousse Cuir pailleté Vincénnés 27", Quantity = 2, Amount = 10.25 },
                    new { Name="Tapis de bain Grand Hôtel Lin Inversé", Quantity = 5, Amount = 5.10 },
                },
            };

            Assert.Throws<DotLiquid.Exceptions.SyntaxException>(() => this.templateEngine.Prepare<object>(templateWithSyntaxError, order));
        }
    }
}
