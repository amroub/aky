using System.Collections.Generic;

namespace Akeneo.Model
{
    public class Variant
    {
        public int Level { get; set; }
        public List<string> Axes { get; set; }
        public List<string> Attributes { get; set; }
    }
}