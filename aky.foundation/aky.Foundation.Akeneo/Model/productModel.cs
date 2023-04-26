using Akeneo.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Diatly.Foundation.Akeneo.Model
{
    [ExcludeFromCodeCoverage]
    public class ProductModel : ModelBase
    {
        /// <summary>
        ///  Product Model code 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///  Family code from which the product inherits its attributes and attributes requirements 
        /// </summary>
        public string Family_Variant { get; set; }

        /// <summary>
        /// Parent
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        ///  Codes of the categories in which the product is classified 
        /// </summary>
        public List<string> Categories { get; set; }
        

        /// <summary>
        ///  Product Model attributes values
        /// </summary>
        public Dictionary<string, List<ProductValue>> Values { get; set; }
        
        
        public ProductModel()
        {            
            Categories = new List<string>();            
            Values = new Dictionary<string, List<ProductValue>>();
        }
    }
}
