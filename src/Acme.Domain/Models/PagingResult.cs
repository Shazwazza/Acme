using System.Collections.Generic;

namespace Acme.Domain.Models
{
    public class PagingResult<T>
    {
        public int TotalHits { get; set; }
        public IReadOnlyList<T> Hits { get; set; }
        public PagingInformation PagingInformation { get; set; }
    }
}