using Akeneo.Client;
using System.Collections.Generic;

namespace Akeneo.Extensions
{
    public static class PaginationDictionaryExtensions
    {
        private static readonly string Next = "next";

        public static PaginationLink GetNext(this IDictionary<string, PaginationLink> links)
        {
            return links.ContainsKey(Next) ? links[Next] : null;
        }
    }
}
