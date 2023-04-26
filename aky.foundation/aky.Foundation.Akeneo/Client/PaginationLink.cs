using System.Diagnostics;

namespace Akeneo.Client
{
    [DebuggerDisplay("{Href}")]
    public class PaginationLink
    {
        public string Href { get; set; }
    }
}
