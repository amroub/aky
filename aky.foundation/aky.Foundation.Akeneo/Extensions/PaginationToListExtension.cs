using Akeneo.Client;
using System.Collections.Generic;
using System.Linq;

namespace Akeneo.Extensions
{
    public static class PaginationToListExtension
    {
        public static List<TItem> GetItems<TItem>(this PaginationResult<TItem> result)
        {
            return result?.Embedded?.Items.Select(item => item).ToList();
        }
    }
}
