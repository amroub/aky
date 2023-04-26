using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Akeneo.Model
{
    [ExcludeFromCodeCoverage]
    public class MediaProduct
    {
        /// <summary>
        /// Product Identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Attribute Code
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// Channel Code
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Locale Code
        /// </summary>
        public string Locale { get; set; }
    }
}
