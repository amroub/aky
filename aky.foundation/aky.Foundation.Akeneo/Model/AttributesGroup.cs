using Akeneo.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diatly.Foundation.Akeneo.Model
{
    public class AttributeGroup : ModelBase
    {
        /// <summary>
        /// AttributesGroup code (required)
        /// </summary>
        public string Code { get; set; }

        public string[] Attributes { get; set; }

        public Dictionary<string, string> Labels { get; set; }

    }
}
