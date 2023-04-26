using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Akeneo.Client
{
    public class PaginationResult<TContent>
    {
        public string Message { get; set; }

        public HttpStatusCode Code { get; set; }

        [JsonProperty("_links")]
        public Dictionary<string, PaginationLink> Links { get; set; }

        public int CurrentPage { get; set; }

        [JsonProperty("_embedded")]
        public EmbeddedItems<TContent> Embedded { get; set; }

        public static PaginationResult<TContent> Empty => new PaginationResult<TContent>
        {
            CurrentPage = -1,
            Embedded = new EmbeddedItems<TContent>(),
            Links = new Dictionary<string, PaginationLink>()
        };
    }
}
