using Akeneo.Model.ProductValues;
using Akeneo.Search;
using Akeneo.Consts;

namespace Akeneo.Search
{
    public class ProductPriceValue : ProductValue
    {
        public static ProductPriceValue Equal(string attributeCode, float amount, string currency)
        {
            return new ProductPriceValue
            {
                AttributeCode = attributeCode,
                Operator = Operators.Equal,
                Value = new { Amount = amount, Currency = Currency.EUR }
            };
        }

        public static ProductPriceValue NotEqual(string attributeCode, float amount, string currency)
        {
            return new ProductPriceValue
            {
                AttributeCode = attributeCode,
                Operator = Operators.NotEqual,
                Value = new { Amount = amount, Currency = Currency.EUR }
            };
        }

        public static ProductPriceValue Greater(string attributeCode, float amount, string currency)
        {
            return new ProductPriceValue
            {
                AttributeCode = attributeCode,
                Operator = Operators.Greater,
                Value = new { Amount = amount, Currency = Currency.EUR }
            };
        }

        public static ProductPriceValue GreaterOrEqual(string attributeCode, float amount, string currency)
        {
            return new ProductPriceValue
            {
                AttributeCode = attributeCode,
                Operator = Operators.GreaterOrEqual,
                Value = new { Amount = amount, Currency = Currency.EUR }
            };
        }

        public static ProductPriceValue Less(string attributeCode, float amount, string currency)
        {
            return new ProductPriceValue
            {
                AttributeCode = attributeCode,
                Operator = Operators.Lower,
                Value = new { Amount = amount, Currency = Currency.EUR }
            };
        }

        public static ProductPriceValue LessOrEqual(string attributeCode, float amount, string currency)
        {
            return new ProductPriceValue
            {
                AttributeCode = attributeCode,
                Operator = Operators.LowerOrEqual,
                Value = new { Amount = amount, Currency = Currency.EUR }
            };
        }
    }
}
